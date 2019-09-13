using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EV_BreachStart : Event_Parent
{
    public GameObject trigger2, trigger1, Sci, Gua, Anchor1, Anchor2;
    EV_Puppet_Controller Sci_, Gua_;
    public EV_Puppet_Controller ded;
    public Transform[] Path;
    bool check2 = true, check1 = true, StopTimer =true, step = false;
    float Timer;
    public AudioClip Dialog, blackout;
    public AudioClip[] NewAmbiance;

    // Update is called once per frame
    private void Awake()
    {
        Sci_ = Sci.GetComponent<EV_Puppet_Controller>();
        Gua_ = Gua.GetComponent<EV_Puppet_Controller>();
        
    }

    public override void EventStart()
    {
        base.EventStart();
        ded.AnimTrigger(-3, true);
        ded.DeactivateCollision();
        GameController.instance.QuickSave();
    }

    void Update()
    {
        if (isStarted == true)
            EventUpdate();
        
    }

    public override void EventUpdate()
    {
        Timer -= Time.deltaTime;

        if (Timer <= 0.0f && StopTimer == false)
        {
            GameController.instance.player.GetComponent<Player_Control>().FakeBlink(1f);
            AmbianceController.instance.ChangeAmbiance(NewAmbiance, 6);
            GameController.instance.GlobalSFX.PlayOneShot(blackout);
            GameController.instance.Warp173(false, GameController.instance.transform);
            StopTimer = true;
            SCP_UI.instance.ShowTutorial("tutorun");
            EventFinished();
        }

        if (Timer <= 4f && step == false && StopTimer == false)
        {
            GameController.instance.player.GetComponent<Player_Control>().FakeBlink(1f);
            GameController.instance.GlobalSFX.PlayOneShot(blackout);
            GameController.instance.Warp173(false, Anchor2.transform);
            step = true;
        }


        if (check2 == true)
        {
            if (trigger2.GetComponent<BoxTrigger>().GetState())
            {
                Sci_.SetPath(Path);
                Gua_.SetPath(Path);
                Gua_.PlaySound(Dialog);
                SubtitleEngine.instance.playVoice("scene_BreachStart_1", true);

                GameController.instance.Warp173(false, Anchor1.transform);
                GameController.instance.player.GetComponent<Player_Control>().FakeBlink(0.5f);
                GameController.instance.GlobalSFX.PlayOneShot(blackout);

                GameController.instance.npcObjects[(int)npc.scp173].transform.rotation = Anchor1.transform.rotation;
                check2 = false;
                StopTimer = false;
                Timer = 10;
            }
        }

        if (check1 == true)
        {
            if (trigger1.GetComponent<BoxTrigger>().GetState())
            {
                SCP_UI.instance.ShowTutorial("tutodead");
                check1 = false;
            }
        }
    }

    public override void EventFinished()
    {
        Destroy(Sci);
        Destroy(Gua);
        ded.AnimTrigger(-3, true);


        base.EventFinished();
    }


}


