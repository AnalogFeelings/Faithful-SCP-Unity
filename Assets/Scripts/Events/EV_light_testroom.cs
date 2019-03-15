using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EV_light_testroom : MonoBehaviour
{
    public GameObject Anchor1, Anchor2, SCP, Screen;
    public float Timer;
    bool StopTimer=false, ActiveTimer=false;
    public AudioClip [] SFX;

    // Update is called once per frame
    void Update()
    {   if (ActiveTimer)
            Timer -= Time.deltaTime;
        if (Timer <= 0.0f && StopTimer == false)
        {
            if (GameController.instance.player.GetComponent<Player_Control>().IsBlinking())
            {
                GameController.instance.player.GetComponent<Player_Control>().FakeBlink(0.3f);
                GameController.instance.Warp173(true, Anchor2.transform);
                StopTimer = true;
                Screen.SetActive(false);
                GameController.instance.PlayHorror(SFX[0]);
                GameController.instance.PlayHorror(SFX[1]);
            }
        }
    }


    void OnEnable()
    {
        GameController.instance.Warp173(true, Anchor1.transform);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            ActiveTimer = true;
    }
}
