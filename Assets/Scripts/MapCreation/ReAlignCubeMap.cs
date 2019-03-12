using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CubeMapSettings
{
    public float xBox, yBox, uBox, xOff, yOff, uOff, cubeInten;
    public int cubeRes, cubeImpor;
}

public class ReAlignCubeMap : MonoBehaviour
{
    public CubeMapSettings cube;
    // Start is called before the first frame update
    void Start()
    {
        Transform parrot = GetComponentInParent<Transform>();

        
        // Make a game object
            GameObject probeGameObject = new GameObject("The Reflection Probe");

        // Add the reflection probe component
        ReflectionProbe probeComponent = probeGameObject.AddComponent<ReflectionProbe>() as ReflectionProbe;

        // Set texture resolution
        probeComponent.resolution = cube.cubeRes;

        probeComponent.boxProjection = true;
        probeComponent.intensity = cube.cubeInten;
        probeComponent.importance = cube.cubeImpor;


        // Reflection will be used for objects in 10 units around the position of the probe

        if (transform.eulerAngles.y == 0)
        {
            probeComponent.size = new Vector3(cube.xBox, 20, cube.yBox);
            probeComponent.center = new Vector3(cube.xOff, 0, cube.yOff);
        }
        if (transform.eulerAngles.y == 90)
        {
            probeComponent.size = new Vector3(cube.yBox, 20, cube.xBox);
            probeComponent.center = new Vector3(cube.yOff, 0, cube.xOff);
        }
        if (transform.eulerAngles.y == 180)
        {
            probeComponent.size = new Vector3(-cube.xBox, 20, -cube.yBox);
            probeComponent.center = new Vector3(-cube.xOff, 0, -cube.yOff);
        }
        if (transform.eulerAngles.y == -90)
        {
            probeComponent.size = new Vector3(-cube.yBox, 20, -cube.xBox);
            probeComponent.center = new Vector3(-cube.yOff, 0, -cube.xOff);
        }
        // Set the position (or any transform property)
        probeGameObject.transform.position = this.transform.position;

        probeComponent.mode = UnityEngine.Rendering.ReflectionProbeMode.Realtime;
        probeComponent.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.ViaScripting;

        probeComponent.RenderProbe();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
