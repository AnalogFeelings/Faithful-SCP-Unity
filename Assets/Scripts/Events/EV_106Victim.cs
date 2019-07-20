using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EV_106Victim : Event_Parent
{
    public Transform decal;
    public EV_Puppet_Controller victim;
    public AudioClip hit, corrosion, horror;
    bool Hit;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isStarted && !Hit)
        {
            if (victim._controller.isGrounded)
            {

                victim.PlaySFX(hit);
                GameController.instance.PlayHorror(horror, victim.transform, npc.none);
                Hit = true;
            }
        }
    }

    public override void EventStart()
    {
        if (!isStarted)
        {
            DecalSystem.instance.Decal(decal.position, decal.rotation.eulerAngles, 8f, false, 5f, 2, 0);
            GameController.instance.GlobalSFX.PlayOneShot(corrosion);
            EventFinished();
            isStarted = true;
        }
    }

    public override void EventFinished()
    {
        base.EventFinished();
        victim.gameObject.SetActive(true);
        victim.AnimTrigger(-1, true);
    }





}
