using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventChance
{
    public int Chance;
    public string EventHandler;
    public Transform pos;
}


public class EventHandler : MonoBehaviour
{
    public EventChance[] EventList;


    public string UniqueHandler;
    public bool Spawned = false;
    public string SpecialName;
    public Transform UniquePos;
    public int EventChosen = -1;
    worldPos roomPos;
    bool IsActive, hasUnique;

    public int EventSet()
    {
        EventChosen = -1;
        for (int i=0; i < EventList.Length; i++)
        {
            if (EventList[i].Chance > Random.Range(0,100))
            {
                EventChosen = i;
                break;
            }
        }
        return (EventChosen);
    }

    public void ForceEvent(int forcedevent)
    {
        EventChosen = forcedevent;
    }

    public void EventSpecial()
    {
        EventChosen = -2;
    }

    public string GetEventName()
    {
        return (SpecialName);
    }

    public void EventStart(int x, int y)
    {
        Debug.Log("Evento Nuevo");
        if (Spawned == false)
        {
            if (EventChosen == -2)
                EventSpawn(UniqueHandler, x, y, UniquePos);

            if (EventChosen >= 0)
                EventSpawn(EventList[EventChosen].EventHandler, x, y, EventList[EventChosen].pos);
            Spawned = true;
        }
    }

    public void EventDone(int x, int y)
    {
        Debug.Log("Evento Hecho");
        if (Spawned == false)
        {
            if (EventChosen == -2)
                EventDone(UniqueHandler, x, y, UniquePos);

            if (EventChosen >= 0)
                EventDone(EventList[EventChosen].EventHandler, x, y, EventList[EventChosen].pos);
            Spawned = true;
        }
    }

    void EventSpawn(string scp_event, int x, int y, Transform pos)
    {
        GameObject res_event = Resources.Load<GameObject>(string.Concat("Events/", scp_event));
        if (pos == null)
        {
            res_event = Instantiate(res_event, this.transform.position, this.transform.rotation, this.transform);
        }
        else
            res_event = Instantiate(res_event, pos.position, pos.rotation, this.transform);

        res_event.GetComponent<Event_Parent>().x = x;
        res_event.GetComponent<Event_Parent>().y = y;
        res_event.GetComponent<Event_Parent>().EventStart();
    }

    void EventDone(string scp_event, int x, int y, Transform pos)
    {
        GameObject res_event = Resources.Load<GameObject>(string.Concat("Events/", scp_event));
        if (pos == null)
        {
            res_event = Instantiate(res_event, this.transform.position, this.transform.rotation, this.transform);
        }
        else
            res_event = Instantiate(res_event, pos.position, pos.rotation, this.transform);
        res_event.GetComponent<Event_Parent>().x = x;
        res_event.GetComponent<Event_Parent>().y = y;
        res_event.GetComponent<Event_Parent>().EventFinished();
    }

}
