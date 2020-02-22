using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PD_Kneel : MonoBehaviour
{
    bool setup = false, startscene;
    bool Played, crouched;
    public AudioClip kneel,laugh;
    float Timer;
    public GameObject lights, spot106;
    public PD_Teleports tele;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (startscene && setup == false)
        {
            lights.SetActive(true);
            setup = true;
            Timer = 6;
            
        }

        if (setup)
        {
            Timer -= Time.deltaTime;

            if (Timer <= 0 && Played == false && crouched == false)
            {
                GameController.instance.PlayHorror(kneel,spot106.transform, npc.none);
                Played = true;
                SubtitleEngine.instance.playVoice("kneel106", true);
                GameController.instance.playercache.ForceLook(spot106.transform.position, 2);
            }
            if (Played && !crouched)
            {
                if (GameController.instance.playercache.Crouch)
                {
                    Timer = 3;
                    crouched = true;
                }
            }
            if(Played && crouched && Timer <= 0)
            {
                GameController.instance.playercache.StopLook();
                GameController.instance.GlobalSFX.PlayOneShot(laugh);
                lights.SetActive(false);
                tele.Teleport();
                crouched = false;
                Played = false;
                setup = false;
                startscene = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            startscene = true;
    }
}
