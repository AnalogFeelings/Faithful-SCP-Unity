using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Object_Item : Object_Interact
{
    public gameItem item;
    public int id;

    public Mesh itemMesh;
    public Material[] itemMats;
    // Start is called before the first frame updat
    public void Start()
    {
        this.transform.parent = GameController.instance.itemParent.transform;
    }

    public void Spawn()
    {
        GameObject model = ItemController.instance.items[item.itemName].ItemModel;
        MeshFilter mesh = model.GetComponentInChildren<MeshFilter>(true);
        MeshRenderer renderer = model.GetComponentInChildren<MeshRenderer>(true);
        itemMesh = mesh.sharedMesh;
        itemMats = renderer.sharedMaterials;
    }

    // Update is called once per frame
    public override void Pressed()
    {
        if (ItemController.instance.AddItem(item, 0))
        {
            GameController.instance.DeleteItem(id);
            DestroyImmediate(this.gameObject);

            if (ItemController.instance.items[item.itemName].isUnique)
                SubtitleEngine.instance.playFormatted("playStrings", "play_picked_uni", "itemStrings", ItemController.instance.items[item.itemName].getName());
            else
            {
                if (ItemController.instance.items[item.itemName].isFem)
                    SubtitleEngine.instance.playFormatted("playStrings", "play_picked_fem", "itemStrings", ItemController.instance.items[item.itemName].getName());
                else
                    SubtitleEngine.instance.playFormatted("playStrings", "play_picked_male", "itemStrings", ItemController.instance.items[item.itemName].getName());
            }
        }
        else
            SubtitleEngine.instance.playSub("playStrings", "play_fullinv");

    }

    public void OnRenderObject()
    {
        if (itemMesh != null)
        {
            //Debug.Log("Mats " + currMat.Length);
            for (int i = 0; i < itemMats.Length; i++)
            {
                Graphics.DrawMesh(itemMesh, gameObject.transform.position, gameObject.transform.rotation, itemMats[i], 0, null, i);
            }
        }
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
