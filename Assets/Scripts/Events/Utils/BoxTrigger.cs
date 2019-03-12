using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTrigger : MonoBehaviour
{
    bool Triggered;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            Triggered = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            Triggered = false;
    }

    public bool GetState()
    {
        return Triggered;
    }
}
