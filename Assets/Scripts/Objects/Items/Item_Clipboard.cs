using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Clipboard", menuName = "Items/Clipboard")]
public class Item_Clipboard : Item
{
    public Sprite doc, nodoc;
    // Start is called before the first frame update
    public override void Use(ref gameItem currItem)
    {
        if (currItem.valInt == -1)
        {
            currItem.valInt = ItemController.instance.invs.Count;
            ItemController.instance.NewInv();
        }
        ItemController.instance.ChangeInv(valueInt);
    }
    public override bool Mix(ref gameItem currItem, ref gameItem toMix)
    {
        if (ItemController.instance.items[toMix.itemName] is Document_Equipable || ItemController.instance.items[toMix.itemName] is Equipable_Key)
        {
            if (currItem.valInt == -1)
            {
                currItem.valInt = ItemController.instance.invs.Count;
                ItemController.instance.NewInv();
            }
            if (ItemController.instance.AddItem(toMix, currItem.valInt))
            {
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }



}
