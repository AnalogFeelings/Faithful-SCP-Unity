using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EV_Decontamiantion : Event_Parent
{
    // Start is called before the first frame update
    public AudioClip gas;
    public Transform pos1, pos2;
    public VisualEffect smoke1, smoke2;
    public Object_Door door1, door2;
    bool ActiveTimer, spawnedsmoke, passed, opened, stoppedsmoke;
    float Timer;

    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        if (isStarted)
        {
            Timer -= Time.deltaTime;
            if (ActiveTimer)
            {
                if (Timer <= 10 && !spawnedsmoke)
                {
                    smoke1.Play();
                    smoke2.Play();
                    spawnedsmoke = true;
                    GameController.instance.GlobalSFX.PlayOneShot(gas);
                }
                if (Timer <= 5 && !stoppedsmoke)
                {
                    smoke1.Stop();
                    smoke2.Stop();
                    spawnedsmoke = true;
                    GameController.instance.GlobalSFX.PlayOneShot(gas);
                }

                if (Timer <= 1 && !opened)
                {
                    door1.DoorSwitch();
                    door2.DoorSwitch();
                }

                if (Timer <= 0)
                    ActiveTimer = false;
                
            }

        }

    }

    void OnTriggerStay(Collider other)
    {
        if (isStarted && ActiveTimer == false && other.tag == "Player" && !passed)
        {
            spawnedsmoke = false;
            stoppedsmoke = false;
            ActiveTimer = true;
            Timer = 11;
            passed= true;
            opened = false;
            door1.DoorSwitch();
            door2.DoorSwitch();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (isStarted && other.tag == "Player")
        {
            passed = false;
        }
    }
}
