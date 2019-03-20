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
                return (true);
            }
        }

        return (false);
    }
}
