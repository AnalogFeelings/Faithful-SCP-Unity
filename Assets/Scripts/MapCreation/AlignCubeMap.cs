using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AlignCubeMap : MonoBehaviour
{
   /* public float angle = -2;
    void Start()
    {
        if (QualitySettings.realtimeReflectionProbes == false)
            Destroy(this.gameObject);

        ReflectionProbe probeComponent = GetComponent<ReflectionProbe>();


        var vec = transform.eulerAngles;
        vec.x = Mathf.Round(vec.x / 90) * 90;
        vec.y = Mathf.Round(vec.y / 90) * 90;
        vec.z = Mathf.Round(vec.z / 90) * 90;
        transform.eulerAngles = vec;


        if (transform.eulerAngles.y == 0)
        {
            probeComponent.size = new Vector3(probeComponent.size.x, probeComponent.size.y, probeComponent.size.z);
            probeComponent.center = new Vector3(probeComponent.center.x, probeComponent.center.y, probeComponent.center.z);
            angle = 0;
        }
        if (transform.eulerAngles.y == 90)
        {
            probeComponent.size = new Vector3(probeComponent.size.z, probeComponent.size.y, probeComponent.size.x);
            probeComponent.center = new Vector3(probeComponent.center.z, probeComponent.center.y, -probeComponent.center.x);
            angle = 90;
        }
        if (transform.eulerAngles.y == 180)
        {
            probeComponent.size = new Vector3(probeComponent.size.x, probeComponent.size.y, probeComponent.size.z);
            probeComponent.center = new Vector3(-probeComponent.center.x, probeComponent.center.y, -probeComponent.center.z);
            angle = 180;
        }
        if (transform.eulerAngles.y == -90 || transform.eulerAngles.y == 270)
        {
            probeComponent.size = new Vector3(probeComponent.size.z, probeComponent.size.y, probeComponent.size.x);
            probeComponent.center = new Vector3(-probeComponent.center.z, probeComponent.center.y, probeComponent.center.x);
            angle = -90;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }*/
}
