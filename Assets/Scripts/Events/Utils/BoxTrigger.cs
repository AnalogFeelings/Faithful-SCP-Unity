using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTrigger : MonoBehaviour
{
    bool Triggered;
    public bool autoFalse = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            Triggered = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
            Triggered = true;
    }


    public bool GetState()
    {
        bool value = Triggered;
        if (autoFalse)
            Triggered = false;
        return value;
        
    }
}
