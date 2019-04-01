using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EV_Spawn173 : Event_Parent
{
    // Start is called before the first frame update
    public override void EventStart()
    {
        base.EventStart();
        GameController.instance.Warp173(true, transform);
        GameController.instance.places_173.Add(transform.position);

        EventFinished();
    }

}
