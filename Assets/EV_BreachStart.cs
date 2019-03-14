using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EV_BreachStart : MonoBehaviour
{
    public GameObject trigger, trigger2, Sci, Gua, Anchor1;
    EV_Puppet_Controller Sci_, Gua_;
    public Transform[] Path;
    bool check2 = true, check = true, StopTimer =true;
    float Timer;
    public AudioClip Dialog;
    public AudioClip TheDread;

    // Update is called once per frame
    private void Start()
    {
        Sci_ = Sci.GetComponent<EV_Puppet_Controller>();
        Gua_ = Gua.GetComponent<EV_Puppet_Controller>();
    }

    void Update()
    {
            Timer -= Time.deltaTime;
        if (Timer <= 0.0f && StopTimer == false)
        {
            GameController.instance.player.GetComponent<Player_Control>().FakeBlink(0.3f);
            GameController.instance.DefaultAmbiance();
            GameController.instance.Warp173(false, GameController.instance.transform);
            Destroy(this.gameObject);
        }

            if (check == true)
        {
            if (trigger.GetComponent<BoxTrigger>().GetState())
            {
                check = false;
                GameController.instance.ChangeMusic(TheDread);
                GameController.instance.player.GetComponent<Player_Control>().DefPost();
                RenderSettings.fog = true;
            }
        }

        if (check2 == true)
        {
            if (trigger2.GetComponent<BoxTrigger>().GetState())
            {
                Sci_.SetPath(Path);
                Gua_.SetPath(Path);
                Gua_.PlaySound(Dialog);

                GameController.instance.Warp173(false, Anchor1.transform);
                check2 = false;
                StopTimer = false;
                Timer = 10;
            }
        }






    }
}
