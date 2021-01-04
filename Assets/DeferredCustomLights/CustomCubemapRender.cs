using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using System.Collections.Generic;

// See _ReadMe.txt

public class CustomCubemapEngine
{
    static CustomCubemapEngine m_Instance;
    static public CustomCubemapEngine instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new CustomCubemapEngine();
            return m_Instance;
        }
    }

    internal HashSet<CustomCubemap> m_Cubes = new HashSet<CustomCubemap>();

    public void Add(CustomCubemap o)
    {
        Remove(o);
        m_Cubes.Add(o);
    }
    public void Remove(CustomCubemap o)
    {
        m_Cubes.Remove(o);
    }
}


[ExecuteInEditMode]
public class CustomCubemapRender : MonoBehaviour
{
    public Shader m_CubeShader;
    private Material m_CubeMaterial;

    public Mesh m_Mesh;

    // We'll be adding two command buffers to each camera:
    // - one to calculate illumination from lights (after regular lighting)
    // - another to draw light "objects" themselves (before transparencies are rendered)
    private struct CmdBufferEntry
    {
        public CommandBuffer m_AfterLighting;
        public CommandBuffer m_BeforeAlpha;
    }

    private Dictionary<Camera, CmdBufferEntry> m_Cameras = new Dictionary<Camera, CmdBufferEntry>();


    public void OnDisable()
    {
        foreach (var cam in m_Cameras)
        {
            if (cam.Key)
            {
                cam.Key.RemoveCommandBuffer(CameraEvent.AfterReflections, cam.Value.m_AfterLighting);
                cam.Key.RemoveCommandBuffer(CameraEvent.BeforeForwardAlpha, cam.Value.m_BeforeAlpha);
            }
        }
        Object.DestroyImmediate(m_CubeMaterial);
    }


    public void Update()
    {
        var act = gameObject.activeInHierarchy && enabled;
        if (!act)
        {
            OnDisable();
            return;
        }

        var cam = Camera.current;
        if (!cam)
            return;

        // create material used to render lights
        if (!m_CubeMaterial)
        {
            m_CubeMaterial = new Material(m_CubeShader);
            m_CubeMaterial.hideFlags = HideFlags.HideAndDontSave;
        }

        CmdBufferEntry buf = new CmdBufferEntry();
        if (m_Cameras.ContainsKey(cam))
        {
            // use existing command buffers: clear them
            buf = m_Cameras[cam];
            buf.m_AfterLighting.Clear();
            buf.m_BeforeAlpha.Clear();
        }
        else
        {
            // create new command buffers
            buf.m_AfterLighting = new CommandBuffer();
            buf.m_AfterLighting.name = "Deferred custom cubemaps";
            buf.m_BeforeAlpha = new CommandBuffer();
            buf.m_BeforeAlpha.name = "Draw cubet shapes";
            m_Cameras[cam] = buf;

            cam.AddCommandBuffer(CameraEvent.AfterLighting, buf.m_AfterLighting);
            cam.AddCommandBuffer(CameraEvent.BeforeForwardAlpha, buf.m_BeforeAlpha);
        }

        //@TODO: in a real system should cull lights, and possibly only
        // recreate the command buffer when something has changed.

        var system = CustomCubemapEngine.instance;

        //var propParams = Shader.PropertyToID("_cubemapParams");
        var propColor = Shader.PropertyToID("_customCubeTexture");
        var propHDR = Shader.PropertyToID("_customCubeHDR");
        var propPos = Shader.PropertyToID("_customCubePosition");
        var propMin = Shader.PropertyToID("_customCubeMin");
        var propMax = Shader.PropertyToID("_customCubeMax");
        var propRot = Shader.PropertyToID("_customCubeRotation");
        Vector4 param = Vector4.zero;
        Vector4 pos = Vector4.zero;
        Vector4 min = Vector4.zero;
        Vector4 max = Vector4.zero;
        Matrix4x4 trs = Matrix4x4.identity;

        // construct command buffer to draw lights and compute illumination on the scene
        foreach (var o in system.m_Cubes)
        {
            // light parameters we'll use in the shader
            /*param.x = o.m_TubeLength;
            param.y = o.m_Size;
            param.z = 1.0f / (o.m_Range * o.m_Range);
            param.w = (float)o.m_Kind;
            buf.m_AfterLighting.SetGlobalVector(propParams, param);*/
            pos = o.cube.bounds.center;
            pos.w = o.cube.blendDistance;

            min = o.cube.bounds.min;
            max = o.cube.bounds.max;

            //buf.m_AfterLighting.SetGlobalVector(propParams, param);
            buf.m_AfterLighting.SetGlobalVector(propPos, pos);
            buf.m_AfterLighting.SetGlobalVector(propMin, min);
            buf.m_AfterLighting.SetGlobalVector(propMax, max);
            buf.m_AfterLighting.EnableShaderKeyword("UNITY_SPECCUBE_BOX_PROJECTION");
            buf.m_AfterLighting.SetGlobalVector(propHDR, o.cube.textureHDRDecodeValues);
            buf.m_AfterLighting.SetGlobalTexture(propColor, o.cube.texture);
            buf.m_AfterLighting.SetGlobalVector(propRot, o.transform.forward);

            // draw sphere that covers light area, with shader
            // pass that computes illumination on the scene
            //trs = Matrix4x4.TRS(o.transform.position, o.transform.rotation, new Vector3(o.m_Range * 2, o.m_Range * 2, o.m_Range * 2));
            Vector3 bounds = o.cube.bounds.size;
            bounds.x += o.cube.blendDistance*2;
            bounds.y += o.cube.blendDistance * 2;
            bounds.z += o.cube.blendDistance * 2;
            trs = Matrix4x4.TRS(o.cube.bounds.center, Quaternion.identity, bounds);
            buf.m_AfterLighting.DrawMesh(m_Mesh, trs, m_CubeMaterial, 0, 0);
        }

        // construct buffer to draw light shapes themselves as simple objects in the scene
        /*foreach (var o in system.m_Lights)
		{
			// light color
			buf.m_BeforeAlpha.SetGlobalColor (propColor, o.GetLinearColor());

			// draw light "shape" itself as a small sphere/tube
			if (o.m_Kind == CustomLight.Kind.Sphere)
			{
				trs = Matrix4x4.TRS(o.transform.position, o.transform.rotation, new Vector3(o.m_Size*2,o.m_Size*2,o.m_Size*2));
				buf.m_BeforeAlpha.DrawMesh (m_SphereMesh, trs, m_LightMaterial, 0, 1);
			}
			else if (o.m_Kind == CustomLight.Kind.Tube)
			{
				trs = Matrix4x4.TRS(o.transform.position, o.transform.rotation, new Vector3(o.m_TubeLength*2,o.m_Size*2,o.m_Size*2));
				buf.m_BeforeAlpha.DrawMesh (m_CubeMesh, trs, m_LightMaterial, 0, 1);
			}
		}	*/
    }
}
