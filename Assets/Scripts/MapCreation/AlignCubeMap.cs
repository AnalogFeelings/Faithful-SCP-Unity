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
    public float angle = -2;
    // Start is called before the first frame update
    void Start()
    {
        
        // Add the reflection probe component
        ReflectionProbe probeComponent = GetComponent<ReflectionProbe>();


        var vec = transform.eulerAngles;
        vec.x = Mathf.Round(vec.x / 90) * 90;
        vec.y = Mathf.Round(vec.y / 90) * 90;
        vec.z = Mathf.Round(vec.z / 90) * 90;
        transform.eulerAngles = vec;

        // Reflection will be used for objects in 10 units around the position of the probe

        if (transform.eulerAngles.y == 0)
        {
            probeComponent.size = new Vector3(probeComponent.size.x, probeComponent.size.y, probeComponent.size.z);
            probeComponent.center = new Vector3(probeComponent.center.x, probeComponent.center.y, probeComponent.center.z);
            angle = 0;
        }
        if (transform.eulerAngles.y == 90)
        {
            probeComponent.size = new Vector3(probeComponent.size.z, probeComponent.size.y, probeComponent.size.x);
            probeComponent.center = new Vector3(probeComponent.center.z, probeComponent.center.y, probeComponent.center.x);
            angle = 90;
        }
        if (transform.eulerAngles.y == 180)
        {
            probeComponent.size = new Vector3(-probeComponent.size.x, probeComponent.size.y, -probeComponent.size.z);
            probeComponent.center = new Vector3(-probeComponent.center.x, probeComponent.center.y, -probeComponent.center.z);
            angle = 180;
        }
        if (transform.eulerAngles.y == -90 || transform.eulerAngles.y == 270)
        {
            probeComponent.size = new Vector3(-probeComponent.size.z, probeComponent.size.y, -probeComponent.size.x);
            probeComponent.center = new Vector3(-probeComponent.center.z, probeComponent.center.y, -probeComponent.center.x);
            angle = -90;
        }

        Debug.Log(transform.eulerAngles.y + " Detectado como angulo " + angle);

        /*probeComponent.mode = UnityEngine.Rendering.ReflectionProbeMode.Realtime;
        probeComponent.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.ViaScripting;

        probeComponent.RenderProbe();*/
    }

    // Update is called once per frame
    void Update()
    {

    }
}
