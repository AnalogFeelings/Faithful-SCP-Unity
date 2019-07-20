using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ItemController : MonoBehaviour
{
    public static ItemController instance = null;
    public Item [] currentItem;
    public bool[] currentEquip;
    public List<Item[]> invs;
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

        invs = new List<Item[]>();
        equip = new List<bool[]>();
        currentItem = new Item[10];
        currentEquip = new bool[10];

        invs.Add(currentItem);
        equip.Add(currentEquip);


        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].id = i;
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

    public bool AddItem(Item newitem, int inv)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (invs[inv][i] == null)
            {
                invs[inv][i] = newitem;
                Debug.Log(newitem.name);
                SCP_UI.instance.ItemSFX(newitem.SFX);
                return (true);
            }
        }

        return (false);
    }

    public List<svItem[]> GetItems()
    {
        
        List<svItem[]> temp_list = new List<svItem[]>();

        for (int j = 0; j < invs.Count; j++)
        {
            svItem[] temp_items = new svItem[10];
            for (int i = 0; i < 10; i++)
            {
                if (invs[j][i] != null)
                {
                    svItem temp = new svItem();
                    temp.item = invs[j][i].name;
                    temp.vlFloat = invs[j][i].valueFloat;
                    temp.vlInt = invs[j][i].valueInt;
                    temp_items[i] = temp;
                    Debug.Log("Objeto " + invs[j][i].name + " inv " + j + " slot " + i);
                }
                else
                {
                    temp_items[i] = null;
                    Debug.Log("Sin objeto inv " + j + " slot " + i);
                }
            }
            temp_list.Add(temp_items);
        }
        return (temp_list);
    }

    public void LoadItems(List<svItem[]> List)
    {
        Debug.Log("inventario vacio? " + invs.Count);
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
        }
    }

    public void EmptyItems()
    {
        invs = new List<Item[]>();
    }
}
