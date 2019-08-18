using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Item : Object_Interact
{
    public Item item;
    public int id;
    public int vlInt;
    public float vlFloat;
    public bool isNew = true, preValues = false;
    // Start is called before the first frame updat
    public void Start()
    {
        this.transform.parent = GameController.instance.itemParent.transform;
    }

    public void Spawn()
    {
        Instantiate(item.ItemModel, this.transform);
    }

    // Update is called once per frame
    public override void Pressed()
    {
        Item newitem;
        if (isNew)
        {
            newitem = Object.Instantiate(item);
            newitem.name = item.name;

            if (preValues)
            {
                newitem.valueFloat = vlFloat;
                newitem.valueInt = vlInt;
            }
        }
        else
        {
            newitem = item;
        }

        if (ItemController.instance.AddItem(newitem, 0))
        {
            GameController.instance.DeleteItem(id);
            DestroyImmediate(this.gameObject);

            if (item.isUnique)
                SubtitleEngine.instance.playFormatted("playStrings", "play_picked_uni", "itemStrings", item.itemName);
            else
            {
                if (item.isFem)
                    SubtitleEngine.instance.playFormatted("playStrings", "play_picked_fem", "itemStrings", item.itemName);
                else
                    SubtitleEngine.instance.playFormatted("playStrings", "play_picked_male", "itemStrings", item.itemName);
            }
        }
        else
            SubtitleEngine.instance.playSub("playStrings", "play_fullinv");

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
