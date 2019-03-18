using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EV_TimerDoor : MonoBehaviour
{
    public GameObject Door1, Door2, Button1, Button2;
    bool isActive, timerdone, bopity;
    float Timer;
    AudioSource BipBop;
    // Start is called before the first frame updat
    void Start()
    {
        BipBop = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        if (isActive == false && (Button1.GetComponent<Object_Button_Trigger>().activated == true || Button2.GetComponent<Object_Button_Trigger>().activated == true))
        {
            Timer = 6;
            isActive = true;
            Door1.GetComponent<Object_Door>().DoorSwitch();
            Door2.GetComponent<Object_Door>().DoorSwitch();
            bopity = true;
        }
        if (isActive)
        {
            Timer -= Time.deltaTime;
            if (Timer <= 2 && bopity == true)
            {
                BipBop.Play();
                bopity = false;
            }

            if (Timer <= 0)
            {
                isActive = false;
                Door1.GetComponent<Object_Door>().DoorSwitch();
                Door2.GetComponent<Object_Door>().DoorSwitch();
            }


        }
    }
}
