using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EV_light_testroom : MonoBehaviour
{
    public GameObject Anchor1, Anchor2, SCP;
    public float Timer;
    bool StopTimer=false, ActiveTimer=false;
    // Update is called once per frame
    void Update()
    {   if (ActiveTimer)
            Timer -= Time.deltaTime;
        if (Timer <= 0.0f && StopTimer == false)
        {
            GameController.instance.Warp173(true, Anchor2.transform);
            StopTimer = true;
        }
    }


    void OnEnable()
    {
        GameController.instance.Warp173(false, Anchor1.transform);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            ActiveTimer = true;
    }
}
