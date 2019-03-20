using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventChance
{
    public int Chance;
    public GameObject EventHandler;
}


public class EventHandler : MonoBehaviour
{
    public EventChance[] EventList;

    public GameObject UniqueHandler;
    public string SpecialName;
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

    public void EventStart()
    {
        if (EventChosen == -2)
            UniqueHandler.SetActive(true);

        if (EventChosen >= 0)
            EventList[EventChosen].EventHandler.SetActive(true);
    }

}
