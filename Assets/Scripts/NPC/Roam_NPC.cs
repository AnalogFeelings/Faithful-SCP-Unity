using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Roam_NPC : MonoBehaviour
{
    public int agroLevel = 1;
    public bool isEvent;
    public bool isActive;

    public virtual void Spawn(bool beActive, Vector3 warppoint)
    {
    }

    public virtual void Event_Spawn(bool instant, Vector3 warppoint)
    {
    }

    public virtual void StopEvent()
    {
    }

    public virtual void SetAgroLevel(int level)
    {
        agroLevel = level;
        Debug.Log("Nivel Agro =" + level);
    }

    public virtual void UnSpawn()
    {
    }
}
