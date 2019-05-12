using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ItemController : MonoBehaviour
{
    public static ItemController instance = null;
    public slotController [] slots;
    public int currdrag;
    public int currhover;



    

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    private void Start()
    {
        for (int i = 0; i < slots.Length; i++)
        {
                slots[i].id = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool AddItem(Item newitem)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].item = newitem;
                slots[i].updateInfo();
                Debug.Log(newitem.name);
                SCP_UI.instance.ItemSFX(newitem.SFX);
                return (true);
            }
        }

        return (false);
    }

    public string [] GetItems()
    {
        string [] temp_items = new string [10];
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
                temp_items[i] = slots[i].item.name;
            else
                temp_items[i] = "null";
        }
        return (temp_items);
    }

    public void LoadItems(string[] temp_items)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (temp_items[i]!="null")
                slots[i].item = Resources.Load<Item>(string.Concat("Items/", temp_items[i]));
            slots[i].updateInfo();
        }
    }

    public void EmptyItems()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].item = null;
        }
    }
}
