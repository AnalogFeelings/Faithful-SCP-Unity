using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName = "new Consumable", menuName = "Items/Consumable")]
public class Consumable : Item
{
    public override void Use()
    {
        GameController.instance.playercache.SetEffect(this);
    }

}
