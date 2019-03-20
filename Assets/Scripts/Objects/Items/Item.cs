using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public string itemName;
    public string itemDispName;
    public Sprite icon;

    public virtual void Use()
    {

    }


}
