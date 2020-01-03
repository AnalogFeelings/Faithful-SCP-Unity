using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTriggerController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject emergencylight, cubemap, primarylight;

    public void setExtendedCubeMap(bool Setting)
    {
        if (Setting)
        {
            Vector3 size = new Vector3(45, 20, 45);
            cubemap.GetComponent<BoxCollider>().size = size;
        }
        else
        {
            Vector3 size = new Vector3(15, 20, 15);
            cubemap.GetComponent<BoxCollider>().size = size;
        }
    }

}
