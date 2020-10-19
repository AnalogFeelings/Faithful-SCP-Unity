using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EV_ElevJanitor : Event_Parent
{
    public Transform decal1, decal2;
    public EV_Puppet_Controller janitor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void EventStart()
    {
        if (!isStarted)
        {
            EventFinished();
            isStarted = true;
        }
    }
    public override void EventFinished()
    {
        DecalSystem.instance.Decal(decal1.position, decal1.rotation.eulerAngles, 2.5f, true, 3.5f, 1, 2);
        DecalSystem.instance.Decal(decal2.position, decal2.rotation.eulerAngles, 2f, true, 2f, 0, 2);
        janitor.AnimTrigger(-7, true);
        base.EventFinished();
    }
}
