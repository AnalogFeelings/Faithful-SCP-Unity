using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*[System.Serializable]
public class CubeMapSettings
{
    public float xBox, yBox, xOff, yOff, cubeInten;
    public int cubeRes, cubeImpor;
}*/

public class AlignCubeMap : MonoBehaviour
{
    public CubeMapSettings cube;
    // Start is called before the first frame update
    void Start()
    {
        Transform parrot = GetComponentInParent<Transform>();

        // Add the reflection probe component
        ReflectionProbe probeComponent = GetComponent<ReflectionProbe>() as ReflectionProbe;


        // Reflection will be used for objects in 10 units around the position of the probe

        if (transform.eulerAngles.y == 0)
        {
            probeComponent.size = new Vector3(cube.xBox, cube.uBox, cube.yBox);
            probeComponent.center = new Vector3(cube.xOff, cube.uOff, cube.yOff);
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

        /*probeComponent.mode = UnityEngine.Rendering.ReflectionProbeMode.Realtime;
        probeComponent.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.ViaScripting;

        probeComponent.RenderProbe();*/
    }

    // Update is called once per frame
    void Update()
    {

    }
}
