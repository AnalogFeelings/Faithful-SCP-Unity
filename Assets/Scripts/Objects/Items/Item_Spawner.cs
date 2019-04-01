using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Spawner : MonoBehaviour
{
    public Item[] items;
    public Transform[] positions;
    // Start is called before the first frame update
    public void Spawn()
    {
        for (int i = 0; i < items.Length; i++)
        {
            GameObject newObject;
            newObject = Instantiate(GameController.instance.itemSpawner, positions[i].position, Quaternion.identity);
            Helper(newObject, items[i], positions[i].position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Helper(GameObject newObject, Item item, Vector3 position)
    {
        newObject.GetComponent<Object_Item>().item = item;
        newObject.GetComponent<Object_Item>().id = GameController.instance.AddItem(position, item);
        newObject.GetComponent<Object_Item>().Spawn();
    }
}
