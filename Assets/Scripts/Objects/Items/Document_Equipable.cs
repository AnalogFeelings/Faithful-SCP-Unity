using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Document", menuName = "Items/Document")]
public class Document_Equipable : Equipable_Wear
{
    public string filename;

    public override void Use()
    {
        Player_Control player = GameController.instance.player.GetComponent<Player_Control>();

        if (player.equipment[(int)this.part] == null || player.equipment[(int)this.part].itemName != this.itemName)
        {
            this.Overlay = Resources.Load<Sprite>(string.Concat("Items/Docs/", filename));
            this.part = bodyPart.Hand;
            player.ACT_Equip(this);
        }
        else
        {
            Resources.UnloadAsset(this.Overlay);
            player.ACT_UnEquip(part);
        }

    }

}
