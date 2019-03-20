using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Item : Object_Interact
{
    public Item item;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(item.ItemModel, this.transform);
    }

    // Update is called once per frame
    public override void Pressed()
    {
        ItemController.instance.AddItem(item);
        DestroyImmediate(this.gameObject);
    }

    public override void Hold()
    {
    }
}
