using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new SNAV", menuName = "Items/SNAV")]
public class Equipable_Nav : Equipable_Elec
{
    public bool isOnline;

    public override void Use()
    {
        Player_Control player = GameController.instance.player.GetComponent<Player_Control>();

        this.part = bodyPart.Hand;

        if (player.equipment[(int)this.part] == null || player.equipment[(int)this.part].itemName != this.itemName)
        {
            SCP_UI.instance.SNav.enabled = true;
            player.ACT_Equip(this);
        }
        else
        {
            player.ACT_UnEquip(part);
        }

    }

}
