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
        Debug.Log(item.itemName);
        Instantiate(item.ItemModel, this.transform);
    }

    // Update is called once per frame
    public override void Pressed()
    {
        ItemController.instance.AddItem(item);
        GameController.instance.DeleteItem(id);
        DestroyImmediate(this.gameObject);
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
