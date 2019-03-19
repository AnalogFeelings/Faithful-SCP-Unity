using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class slotController : MonoBehaviour
{
    public Item item;
    public Text displayText;
    public Image displayImage;

    private void Start()
    {
        updateInfo();
    }



    public void updateInfo()
    {
        if (item)
        {
            displayText.text = item.itemDispName;
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
        if (item)
        {
            Debug.Log("You clicked: " + item.itemName);
        }
    }

}
