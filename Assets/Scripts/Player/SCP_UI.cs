using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum Menu { None, Pause, Inv };

public class SCP_UI : MonoBehaviour
{
    public static SCP_UI instance = null;
    public Image eyes;
    public Canvas PauseM;
    public Canvas Inventory;
    public Canvas HUD;
    public EventSystem menu;
    Menu currMenu = Menu.None;

    public Image blinkBar, Overlay;

    public GameObject defInv, defPause, hand;
    // Start is called before the first frame update

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TogglePauseMenu()
    {
        if (currMenu == Menu.Pause)
        {
            Time.timeScale = 1.0f;
            PauseM.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            currMenu = Menu.None;
            return;
        }
        if (currMenu == Menu.None)
        {
            PauseM.enabled = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            currMenu = Menu.Pause;
            return;
        }
    }

    public void ToggleInventory()
    {
        if (currMenu == Menu.Inv)
        {
            Inventory.enabled = false;
            Time.timeScale = 1.0f;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            currMenu = Menu.None;
            return;
        }
        if (currMenu == Menu.None)
        {
            Inventory.enabled = true;
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            currMenu = Menu.Inv;
            menu.SetSelectedGameObject(defInv);

            return;

            //
        }
    }

}
