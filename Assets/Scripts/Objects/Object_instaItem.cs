using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_instaItem : Object_Interact
{
    // Start is called before the first frame update
    public Item item;

    // Start is called before the first frame updat

    public void Start()
    {
        Spawn();
    }

    public void Spawn()
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
