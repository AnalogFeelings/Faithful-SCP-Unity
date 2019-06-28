using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EV_BreachStart : Event_Parent
{
    public GameObject trigger2, Sci, Gua, Anchor1;
    EV_Puppet_Controller Sci_, Gua_;
    public Transform[] Path;
    public Transform[] Path2;
    bool check2 = true, StopTimer =true;
    float Timer;
    public AudioClip Dialog;

    // Update is called once per frame
    private void Awake()
    {
        Sci_ = Sci.GetComponent<EV_Puppet_Controller>();
        Gua_ = Gua.GetComponent<EV_Puppet_Controller>();
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
            GameController.instance.player.GetComponent<Player_Control>().FakeBlink(0.5f);
            GameController.instance.DefaultAmbiance();
            GameController.instance.Warp173(false, GameController.instance.transform);
            StopTimer = true;
            EventFinished();
        }

        /*if (check == true)
        {
            if (trigger.GetComponent<BoxTrigger>().GetState())
            {
                check = false;
                GameController.instance.DefMusic();
                RenderSettings.fog = true;
                Destroy(GameController.instance.startEv);
            }
        }*/

        if (check2 == true)
        {
            if (trigger2.GetComponent<BoxTrigger>().GetState())
            {
                Sci_.SetPath(Path);
                Gua_.SetPath(Path);
                Gua_.PlaySound(Dialog);
                SubtitleEngine.instance.playSub(string.Format(GlobalValues.sceneStrings["scene_BreachStart_1"], GlobalValues.charaStrings["chara_franklin"]));
                SubtitleEngine.instance.playSub(string.Format(GlobalValues.sceneStrings["scene_BreachStart_2"], GlobalValues.charaStrings["chara_ulgrin"]));
                SubtitleEngine.instance.playSub(string.Format(GlobalValues.sceneStrings["scene_BreachStart_3"], GlobalValues.charaStrings["chara_franklin"]));
                SubtitleEngine.instance.playSub(string.Format(GlobalValues.sceneStrings["scene_BreachStart_4"], GlobalValues.charaStrings["chara_ulgrin"]));
                SubtitleEngine.instance.playSub(string.Format(GlobalValues.sceneStrings["scene_BreachStart_5"], GlobalValues.charaStrings["chara_franklin"]));

                GameController.instance.Warp173(false, Anchor1.transform);
                check2 = false;
                StopTimer = false;
                Timer = 14;
            }
        }
    }

    public override void EventFinished()
    {
        Destroy(Sci);
        Destroy(Gua);

        base.EventFinished();
    }

}


