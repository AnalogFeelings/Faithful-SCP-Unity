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
        if (id != -1)
        {
            transform.position = Input.mousePosition;
            dragging = true;
            ItemController.instance.currdrag = id;
            GetComponent<Image>().raycastTarget = false;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = orpos;
        GetComponent<Image>().raycastTarget = true;
        dragging = false;

        if (ItemController.instance.currhover == -1 && isEquip == false && item != null)
        {
            GameController.instance.player.GetComponent<Player_Control>().DropItem(item);
            SCP_UI.instance.ItemSFX(item.SFX);
            item = null;
            updateInfo();
            SCP_UI.instance.ToggleInventory();
        }

        if (ItemController.instance.currhover != -1 && ItemController.instance.slots[ItemController.instance.currhover].item == null)
            slotMove();

        
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
            displayText.text = "";
            displayImage.sprite = null;
            displayImage.color = Color.black;
        }
    }


    public void Use()
    {
        if (item && !dragging)
        {
            SCP_UI.instance.ItemSFX(item.SFX);
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
    }
    public void slotMove()
    {
        ItemController.instance.slots[ItemController.instance.currhover].item = item;
        ItemController.instance.slots[ItemController.instance.currhover].updateInfo();
        item = null;
        updateInfo();
    }


}
