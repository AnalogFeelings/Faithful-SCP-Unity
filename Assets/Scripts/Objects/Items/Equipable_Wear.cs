using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Equipable", menuName = "Items/Equipable")]
public class Equipable_Wear : Item
{
    public Sprite Overlay;
    public bool protectGas;
    public bodyPart part;

    public override void Use(ref gameItem currItem)
    {
        Player_Control player = GameController.instance.player.GetComponent<Player_Control>();
        if (player.equipment[(int)this.part] == null || player.equipment[(int)this.part].itemName != this.itemName)
            player.ACT_Equip(currItem);
        else
            player.ACT_UnEquip(part);

    }

}
