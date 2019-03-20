using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public string itemName;
    public string itemDispName;
    public Sprite icon;
    public bool deleteUse;
    public GameObject ItemModel;

    public virtual void Use()
    {

    }


}
