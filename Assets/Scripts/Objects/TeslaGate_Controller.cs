using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaGate_Controller : Event_Parent
{
    // Start is called before the first frame update
    public AudioClip idle, charge, shock, start;
    public AudioSource audio;
    public GameObject Shock;
    public Material elec;
    bool ActiveTimer, shocked, endshock, started;
    public float scrollSpeed;
    float Timer, offset = 0;

    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if (isStarted)
        {
            offset += Time.deltaTime * scrollSpeed;
            elec.mainTextureOffset = new Vector2(0, offset);

            if (ActiveTimer)
            {
                Timer += Time.deltaTime;

                if (Timer >= 0.5 && !shocked && !endshock)
                {
                    GameController.instance.deathmsg = GlobalValues.deathStrings["death_tesla"];
                    shocked = true;
                    audio.PlayOneShot(shock);
                    Shock.SetActive(true);
                }
                if (Timer >= 1.5 && shocked && !endshock)
                {
                    endshock = true;
                    audio.Stop();
                    Shock.SetActive(false);
                }
                if (Timer >= 2.5 && !started)
                {
                    audio.PlayOneShot(start);
                    started = true;
                }
                if (Timer >= 3)
                {
                    if (GameController.instance.isAlive)
                        GameController.instance.deathmsg = "";
                    endshock = false;
                    shocked = false;
                    started = false;
                    audio.Play();
                    ActiveTimer = false;
                }
            }

        }
        
    }

    void OnTriggerStay(Collider other)
    {
        if (isStarted && ActiveTimer == false && other.tag == "Player")
        {
            ActiveTimer = true;
            Timer = 0;
            audio.PlayOneShot(charge);
        }
    }
}
