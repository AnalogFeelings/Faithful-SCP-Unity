using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class slotController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    Vector3 orpos;
    bool dragging;
    public int id;

    private void Start()
    {
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

        if (ItemController.instance.currhover == -1)
        {
            if (ItemController.instance.currentEquip[id] == false && ItemController.instance.currentItem[id] != null)
            {
                GameController.instance.player.GetComponent<Player_Control>().DropItem(ItemController.instance.currentItem[id]);
                SCP_UI.instance.ItemSFX(ItemController.instance.currentItem[id].SFX);
                ItemController.instance.currentItem[id] = null;
                SCP_UI.instance.ToggleInventory();
            }
        }
        else
        {
            if (ItemController.instance.currhover != -1 && ItemController.instance.currentItem[ItemController.instance.currhover] == null && ItemController.instance.currentEquip[ItemController.instance.currhover] != true && ItemController.instance.currentEquip[id] != true)
            {
                slotMove();
                return;
            }
            if (ItemController.instance.currentItem[ItemController.instance.currhover] != null && ItemController.instance.currentEquip[ItemController.instance.currhover] != true && ItemController.instance.currentEquip[id] != true)
            {
                if (ItemController.instance.currentItem[ItemController.instance.currhover].Mix(ItemController.instance.currentItem[id]))
                {
                    ItemController.instance.currentItem[id] = null;
                }
            }
        }

        ItemController.instance.UpdateInv();
    }



    public void updateInfo()
    {
        Text displayText = transform.Find("Text").GetComponent<Text>();
        Image displayImage = transform.Find("Image").GetComponent<Image>();

        if (ItemController.instance.currentItem[id])
        {
            if (!ItemController.instance.currentEquip[id])
                displayText.text = GlobalValues.itemStrings[ItemController.instance.currentItem[id].itemName];
            else
                displayText.text = string.Format(GlobalValues.playStrings["play_equiped"], GlobalValues.itemStrings[ItemController.instance.currentItem[id].itemName]);
            displayImage.sprite = ItemController.instance.currentItem[id].icon;
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
        if (ItemController.instance.currentItem[id] && !dragging)
        {
            SCP_UI.instance.ItemSFX(ItemController.instance.currentItem[id].SFX);
            ItemController.instance.currentItem[id].Use();
            if (ItemController.instance.currentItem[id].deleteUse == true)
            {
                if (ItemController.instance.currentItem[id].isFem)
                    SubtitleEngine.instance.playSub(string.Format(GlobalValues.playStrings["play_used_fem"], GlobalValues.itemStrings[ItemController.instance.currentItem[id].itemName]));
                else
                    SubtitleEngine.instance.playSub(string.Format(GlobalValues.playStrings["play_used_male"], GlobalValues.itemStrings[ItemController.instance.currentItem[id].itemName]));

                ItemController.instance.currentItem[id] = null;
                
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
        ItemController.instance.currentItem[ItemController.instance.currhover] = ItemController.instance.currentItem[id];
        ItemController.instance.currentItem[id] = null;
    }
}
