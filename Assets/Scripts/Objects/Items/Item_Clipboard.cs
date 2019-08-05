using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Clipboard", menuName = "Items/Clipboard")]
public class Item_Clipboard : Item
{
    public Sprite doc, nodoc;
    // Start is called before the first frame update
    public override void Use()
    {
        if (valueInt == -1)
        {
            valueInt = ItemController.instance.invs.Count;
            ItemController.instance.NewInv();
        }
        ItemController.instance.ChangeInv(valueInt);
    }
    public override bool Mix(Item toMix)
    {
        if (toMix is Document_Equipable || toMix is Equipable_Key)
        {
            if (valueInt == -1)
            {
                valueInt = ItemController.instance.invs.Count;
                ItemController.instance.NewInv();
            }
            if (ItemController.instance.AddItem(toMix, valueInt))
            {
                icon = doc;
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }



}
