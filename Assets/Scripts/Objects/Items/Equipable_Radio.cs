using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Radio", menuName = "Items/Radio")]
public class Equipable_Radio : Equipable_Elec
{
    public override void Use(ref gameItem currItem)
    {
        Player_Control player = GameController.instance.player.GetComponent<Player_Control>();

        this.part = bodyPart.Hand;

        if (player.equipment[(int)this.part] == null || player.equipment[(int)this.part].itemName != this.itemName)
        {
            player.ACT_Equip(currItem);
            SCP_UI.instance.radio.StartRadio();
        }
        else
        {
            player.ACT_UnEquip(part);
        }

    }

    public override bool Mix(ref gameItem currItem, ref gameItem toMix)
    {
        if (ItemController.instance.items[toMix.itemName].itemName.Equals("bat_nor"))
        {
            currItem.valFloat = 100;
            return (true);
        }
        else
            return (false);
    }

}
