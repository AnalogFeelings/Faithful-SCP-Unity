using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : ScriptableObject
{
    public string itemName;
    public string itemDispName;
    public Sprite icon;
    public bool deleteUse, hasEffect;
    public efecttable Effects;
    public GameObject ItemModel;
    public int SFX;

    public virtual void Use()
    {

    }


}
