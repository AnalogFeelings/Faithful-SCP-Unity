using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class gameItem
{
    public string itemName;
    public int valInt;
    public float valFloat;

    public gameItem(string _itemName, bool isNew = true, int _int = 0, float _float = 0)
    {
        itemName = _itemName;
        if (!isNew)
        {
            valInt = _int;
            valFloat = _float;
        }
        else
        {
            valInt = ItemController.instance.items[_itemName].valueInt;
            valFloat = ItemController.instance.items[_itemName].valueFloat;
        }
    }

}

public class ItemController : MonoBehaviour
{
    public static ItemController instance = null;
    public Dictionary<string, Item> items;
    public gameItem [] currentItem;
    public bool[] currentEquip;
    public List<gameItem[]> invs;
    public List<bool []> equip;
    public slotController [] slots;

    public int currdrag;
    public int currhover;
    public int currInv=0;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        invs = new List<gameItem[]>();
        equip = new List<bool[]>();
        currentItem = new gameItem[10];
        currentEquip = new bool[10];

        invs.Add(currentItem);
        equip.Add(currentEquip);

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].id = i;
        }

        items = new Dictionary<string, Item>();
        Object[] templatesArray;
        templatesArray = Resources.LoadAll("Items/", typeof(Item));
        foreach (Item template in templatesArray)
        {
            Debug.Log("Template name: " + template.name);
            if (!items.ContainsKey(template.name))
                items.Add(template.name, template);
            /*else
                Debug.Log("Duplicate KEY!");*/
        }
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenInv()
    {
        currentItem = invs[0];
        currentEquip = equip[0];
        UpdateInv();
    }

    public void CloseInv()
    {
        currInv = 0;
    }


    
    public void UpdateInv()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].updateInfo();
        }
    }
    public void playerDeath()
    {

    }

    public bool AddItem(gameItem item, int inv)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (invs[inv][i] == null)
            {
                invs[inv][i] = item;
                Debug.Log(item.itemName);
                SCP_UI.instance.ItemSFX(items[item.itemName].SFX);
                return (true);
            }
        }

        return (false);
    }

    public bool IsEmpty(int inv)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (invs[inv][i] != null)
                return false;
        }

        return (true);
    }

    public List<gameItem[]> GetItems()
    {
        return (invs);
    }

    public void LoadItems(List<gameItem[]> List)
    {
        /*Debug.Log("inventario vacio? " + invs.Count);
        equip = new List<bool[]>();

        for (int j = 0; j < List.Count; j++)
        {
            Debug.Log("Entrando al loop j" + j);
            Item[] temp_items = new Item[10];
            for (int i = 0; i < 10; i++)
            {
                Debug.Log("Entrando al loop i" + i);
                if (List[j][i] != null)
                {
                    Item newitem = Object.Instantiate(Resources.Load<Item>(string.Concat("Items/", List[j][i].item)));
                    newitem.name = List[j][i].item;
                    newitem.valueFloat = List[j][i].vlFloat;
                    newitem.valueInt = List[j][i].vlInt;
                    temp_items[i] = newitem;
                    Debug.Log("Cargando Item " + string.Concat("Items/", List[j][i].item));
                }
                else
                    temp_items[i] = null;
            }
            
            invs.Add(temp_items);
            equip.Add(new bool[10]);
        }*/
        invs = List;
    }

    public void EmptyItems()
    {
        invs = new List<gameItem[]>();
    }

    public void NewInv()
    {
        invs.Add(new gameItem[10]);
        equip.Add(new bool[10]);
    }

    public void ChangeInv(int inv)
    {
        currentItem = invs[inv];
        currentEquip = equip[inv];
        currInv = inv;
        UpdateInv();
    }
}
