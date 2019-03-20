using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Equipable", menuName = "Items/Equipable")]
public class Equipable_Wear : Item
{
    public Sprite Overlay;
    public bool protectGas;
    public bodyPart part;


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
