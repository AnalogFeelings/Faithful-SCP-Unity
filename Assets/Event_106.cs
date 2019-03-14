using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_106 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject trigger, spawnHere;
    bool spawned = false;
    // Update is called once per frame
    void Update()
    {
        if (spawned == false && trigger.GetComponent<BoxTrigger>().GetState())
        {
            GameController.instance.Warp106(spawnHere.transform);
            spawned = true;
        }
    }
}
