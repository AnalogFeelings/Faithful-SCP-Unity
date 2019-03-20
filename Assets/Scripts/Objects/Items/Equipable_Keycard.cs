using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Keycard", menuName = "Items/Keycard")]
public class Equipable_Key : Equipable_Wear
{
    public int level;
    part = bodyPart.hand;


    public override void Use()
    {
        Player_Control player = GameController.instance.player.GetComponent<Player_Control>();
        switch (part)
        {
            case bodyPart.Head:
                { 
                    if (player.equippedHead == null || player.equippedHead != this.itemName)
                        player.ACT_Equip(this);
                    else
                        player.ACT_UnEquip(part);
                    break;
                }
            case bodyPart.Body:
                {
                    if (player.equippedBody == null || player.equippedBody != this.itemName)
                        player.ACT_Equip(this);
                    else
                        player.ACT_UnEquip(part);
                    break;
                }
            case bodyPart.Hand:
                {
                    if (player.equippedHand == null || player.equippedHand != this.itemName)
                        player.ACT_Equip(this);
                    else
                        player.ACT_UnEquip(part);
                    break;
                }
            case bodyPart.Any:
                {
                    if (player.equippedAny == null || player.equippedAny != this.itemName)
                        player.ACT_Equip(this);
                    else
                        player.ACT_UnEquip(part);
                    break;
                }
        }

    }

}
