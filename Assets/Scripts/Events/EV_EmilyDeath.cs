using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EV_EmilyDeath : Event_Parent
{
    public AudioClip Scream, voice079;
    public AudioSource Source;
    public Transform pos;
    public GameObject spark;
    // Start is called before the first frame update
    void Start()
    {
        if (GameController.instance.getValue(x,y,0)==0)
        {
            Source.PlayOneShot(Scream);
            GameController.instance.setValue(x, y, 0, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void EventStart()
    {
        base.EventStart();
        GameController.instance.GlobalSFX.PlayOneShot(voice079);
        Instantiate(spark, pos.transform.position, pos.transform.rotation);
        EventFinished();
    }
}
