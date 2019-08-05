using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Item : Object_Interact
{
    public Item item;
    public int id;
    // Start is called before the first frame updat

    public void Spawn()
    {
        Instantiate(item.ItemModel, this.transform);
    }

    // Update is called once per frame
    public override void Pressed()
    {
        Item newitem = Object.Instantiate(item);
        newitem.name = item.name;

        if (ItemController.instance.AddItem(newitem, 0))
        {
            GameController.instance.DeleteItem(id);
            DestroyImmediate(this.gameObject);

            if (item.isUnique)
                SubtitleEngine.instance.playSub(string.Format(GlobalValues.playStrings["play_picked_uni"], GlobalValues.itemStrings[item.itemName]));
            else
            {
                if (item.isFem)
                    SubtitleEngine.instance.playSub(string.Format(GlobalValues.playStrings["play_picked_fem"], GlobalValues.itemStrings[item.itemName]));
                else
                    SubtitleEngine.instance.playSub(string.Format(GlobalValues.playStrings["play_picked_male"], GlobalValues.itemStrings[item.itemName]));

            }
        }
        else
            SubtitleEngine.instance.playSub(GlobalValues.playStrings["play_fullinv"]);

    }

    public override void Hold()
    {
    }

    public void Delete()
    {
        GameController.instance.DeleteItem(id);
        DestroyImmediate(this.gameObject);
    }



}
