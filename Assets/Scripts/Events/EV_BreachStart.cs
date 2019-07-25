using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EV_BreachStart : Event_Parent
{
    public GameObject trigger2, Sci, Gua, Anchor1, Anchor2;
    EV_Puppet_Controller Sci_, Gua_;
    public EV_Puppet_Controller ded;
    public Transform[] Path;
    bool check2 = true, StopTimer =true, step = false;
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
            GameController.instance.ChangeAmbiance(NewAmbiance, 3);
            GameController.instance.GlobalSFX.PlayOneShot(blackout);
            GameController.instance.Warp173(false, GameController.instance.transform);
            StopTimer = true;
            SCP_UI.instance.ShowTutorial("tutorun");
            EventFinished();
        }

        if (Timer <= 7f && step == false && StopTimer == false)
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
                SubtitleEngine.instance.playSub(string.Format(GlobalValues.sceneStrings["scene_BreachStart_1"], GlobalValues.charaStrings["chara_franklin"]), true);
                SubtitleEngine.instance.playSub(string.Format(GlobalValues.sceneStrings["scene_BreachStart_2"], GlobalValues.charaStrings["chara_ulgrin"]), true);
                SubtitleEngine.instance.playSub(string.Format(GlobalValues.sceneStrings["scene_BreachStart_3"], GlobalValues.charaStrings["chara_franklin"]), true);
                SubtitleEngine.instance.playSub(string.Format(GlobalValues.sceneStrings["scene_BreachStart_4"], GlobalValues.charaStrings["chara_ulgrin"]), true);
                SubtitleEngine.instance.playSub(string.Format(GlobalValues.sceneStrings["scene_BreachStart_5"], GlobalValues.charaStrings["chara_franklin"]), true);

                GameController.instance.Warp173(false, Anchor1.transform);
                GameController.instance.player.GetComponent<Player_Control>().FakeBlink(1f);
                
                GameController.instance.npcObjects[(int)npc.scp173].transform.rotation = Anchor1.transform.rotation;
                check2 = false;
                StopTimer = false;
                Timer = 14;
                SCP_UI.instance.ShowTutorial("tutodead");
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


