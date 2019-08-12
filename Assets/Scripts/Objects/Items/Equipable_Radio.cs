using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Radio", menuName = "Items/Radio")]
public class Equipable_Radio : Equipable_Elec
{
    public override void Use()
    {
        Player_Control player = GameController.instance.player.GetComponent<Player_Control>();

        this.part = bodyPart.Hand;

        if (player.equipment[(int)this.part] == null || player.equipment[(int)this.part].itemName != this.itemName)
        {
            player.ACT_Equip(this);
            SCP_UI.instance.radio.StartRadio();
        }
        else
        {
            player.ACT_UnEquip(part);
        }

    }

    public override bool Mix(Item toMix)
    {
        if (toMix != null && toMix.itemName.Equals("bat_nor"))
        {
            valueFloat = 100;
            return (true);
        }
        else
            return (false);
    }

}
