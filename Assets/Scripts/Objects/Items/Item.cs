using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public bool deleteUse, hasEffect, isFem = false;
    public efecttable Effects;
    public GameObject ItemModel;
    public float valueFloat;
    public int valueInt;
    public int SFX;

    public virtual void Use()
    {

    }

    public virtual bool Mix(Item toMix)
    {
        return false;
    }


}
