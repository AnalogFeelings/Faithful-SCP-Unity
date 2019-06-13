using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipable_Elec : Equipable_Wear
{
    public float Battery = 100;

    public override void Use()
    {
        Player_Control player = GameController.instance.player.GetComponent<Player_Control>();
        if (player.equipment[(int)this.part] == null || player.equipment[(int)this.part].itemName != this.itemName)
            player.ACT_Equip(this);
        else
            player.ACT_UnEquip(part);

    }
}
