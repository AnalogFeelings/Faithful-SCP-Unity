using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class slotController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    Vector3 orpos;
    bool dragging;
    public Item item;
    public int id;
    public bool isEquip;

    private void Start()
    {
        updateInfo();
        orpos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        dragging = true;
        ItemController.instance.currdrag = id;
        GetComponent<Image>().raycastTarget = false;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
 
        if (ItemController.instance.slots[ItemController.instance.currhover].item == null)
            slotMove();

        transform.position = orpos;
        GetComponent<Image>().raycastTarget = true;
        dragging = false;
    }



    public void updateInfo()
    {
        Text displayText = transform.Find("Text").GetComponent<Text>();
        Image displayImage = transform.Find("Image").GetComponent<Image>();

        if (item)
        {
            if (!isEquip)
                displayText.text = item.itemDispName;
            else
                displayText.text = "Using " + item.itemDispName;
            displayImage.sprite = item.icon;
            displayImage.color = Color.white;
        }
        else
        {
            displayText.text = " " + id;
            displayImage.sprite = null;
            displayImage.color = Color.black;
        }
    }


    public void Use()
    {
        if (item && !dragging)
        {
            item.Use();
            if (item.deleteUse == true)
            {
                item = null;
            }

        }
        updateInfo();
    }

    public void Hover()
    {
        ItemController.instance.currhover = id;
        Debug.Log(id);
    }
    public void slotMove()
    {
        ItemController.instance.slots[ItemController.instance.currhover].item = item;
        ItemController.instance.slots[ItemController.instance.currhover].updateInfo();
        item = null;
        updateInfo();
    }


}
