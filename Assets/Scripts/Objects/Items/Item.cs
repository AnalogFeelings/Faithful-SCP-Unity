using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public bool deleteUse, hasEffect, isFem = false, isUnique=false, keepInv = false;
    public efecttable Effects;
    public GameObject ItemModel;
    public float valueFloat;
    public int valueInt;
    public int SFX;

    public virtual void Use(ref gameItem currItem)
    {

    }

    public virtual bool Mix(ref gameItem currItem, ref gameItem toMix)
    {
        return false;
    }

    public virtual string getName()
    {
        return itemName;
    }


}
