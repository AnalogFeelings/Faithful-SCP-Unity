using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Button : MonoBehaviour
{
    public GameObject Door01, Door02;
    public int function = 0;
    float deactivate;
    public bool activated;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (function == 1)
        {
            deactivate -= Time.deltaTime;
            if (deactivate <= 0)
                activated = false;
        }
    }

    public void Pressed()
    {
        if (function == 0)
        {
            Door01.GetComponent<Object_Door>().DoorSwitch();
            if (Door02 != null)
                Door02.GetComponent<Object_Door>().DoorSwitch();
        }
        if (function == 1)
        {
            deactivate = 1.0f;
                activated = true;
        }
    }
}
