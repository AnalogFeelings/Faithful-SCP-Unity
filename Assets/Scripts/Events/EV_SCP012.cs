using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EV_SCP012 : Event_Parent
{
    public Object_Door door;
    public AudioSource golgotha;
    public AudioClip [] Dvoice;
    public MeshRenderer Pages;
    public Texture [] bloodPages;
    public Transform[] Path;
    public Transform blood;
    public BoxTrigger trigger, scp012;
    public Animator box;
    public float StartTimer = 90;
    float Timer;
    bool check=true, check2=true, check3 = true, shesamaniac = false, audio1=false, audio2=false, audio3=false, audio4=false, audio5=false, audio6=false, audio7=false;


    // Start is called before the first frame update
    void Start()
    {
        
    }


    public override void EventUpdate()
    {
        if (trigger.GetComponent<BoxTrigger>().GetState())
        {
            if (check == true)
            {
                EventFinished();
            }
            if (check2 == true)
            {
                GameController.instance.player.GetComponent<Player_Control>().ForceWalk(Path);
                check2 = false;
            }

        }
        else
        {
            if (check2 == false)
            {
                check2 = true;
                GameController.instance.player.GetComponent<Player_Control>().StopWalk();
            }
        }

        if (scp012.GetComponent<BoxTrigger>().GetState() && check3 == true)
        {
            Timer -= Time.deltaTime;

            if (audio1 == false)
            {
                GameController.instance.GlobalSFX.PlayOneShot(Dvoice[0]);
                audio1 = true;
            }

            if (audio2 == false && Timer < StartTimer-12)
            {
                GameController.instance.GlobalSFX.PlayOneShot(Dvoice[1]);
                DecalSystem.instance.Decal(new Vector3(blood.transform.position.x + 0.3f, blood.transform.position.y, blood.transform.position.z - 0.15f), new Vector3(90f, 0f, 0f), 1.0f, false, 0.5f, 0, 1);
                audio2 = true;
                

            }
            if (audio3 == false && Timer < StartTimer - 30)
            {
                GameController.instance.GlobalSFX.PlayOneShot(Dvoice[2]);
                audio3 = true;
                DecalSystem.instance.Decal(new Vector3(blood.transform.position.x - 0.1f, blood.transform.position.y, blood.transform.position.z + 0.25f), new Vector3(90f, 0f, 0f), 2.0f, false, 0.5f, 0, 0);
                Pages.material.mainTexture = bloodPages[0];
            }

            if (audio4 == false && Timer < StartTimer - 48)
            {
                GameController.instance.GlobalSFX.PlayOneShot(Dvoice[3]);
                Pages.material.mainTexture = bloodPages[1];
                audio4 = true;
            }
            if (audio5 == false && Timer < StartTimer - 60)
            {
                GameController.instance.GlobalSFX.PlayOneShot(Dvoice[4]);
                DecalSystem.instance.Decal(new Vector3(blood.transform.position.x + 0.1f, blood.transform.position.y, blood.transform.position.z - 0.15f), new Vector3(90f, 0f, 0f), 2.0f, false, 0.5f, 0, 1);
                audio5 = true;
            }

            if (audio6 == false && Timer < StartTimer - 74)
            {
                GameController.instance.GlobalSFX.PlayOneShot(Dvoice[5]);
                DecalSystem.instance.Decal(new Vector3(blood.transform.position.x + 0.2f, blood.transform.position.y, blood.transform.position.z - 0.1f), new Vector3(90f, 0f, 0f), 2.0f, false, 0.5f, 0, 2);
                audio6 = true;
                Pages.material.mainTexture = bloodPages[2];
            }
            if (audio7 == false && Timer < StartTimer - 86)
            {
                GameController.instance.GlobalSFX.PlayOneShot(Dvoice[6]);
                audio7 = true;
            }

            if (Timer <= 0)
            {
                GameController.instance.player.GetComponent<Player_Control>().Death(0);
                DecalSystem.instance.Decal(new Vector3(blood.transform.position.x - 0.03f, blood.transform.position.y, blood.transform.position.z + 0.02f), new Vector3(90f, 0f, 0f), 3.0f, false, 3.0f, 1, 2);
                check3 = false;
            }
            if (shesamaniac == false)
            {
                GameController.instance.player.GetComponent<Player_Control>().CognitoHazard(true);
                shesamaniac = true;
            }
        }
        else
        {
            if (shesamaniac == true)
            {
                GameController.instance.player.GetComponent<Player_Control>().CognitoHazard(false);
                shesamaniac = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isStarted == true)
            EventUpdate();
    }

    public override void EventFinished()
    {
        box.SetBool("start", true);
        golgotha.Play();
        door.DoorSwitch();
        check = false;
        isStarted = true;
        Timer = StartTimer;

        base.EventFinished();

    }
}
