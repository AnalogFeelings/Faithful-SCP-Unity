using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum Menu { None, Pause, Inv , Death, Screen, Debug};

public class SCP_UI : MonoBehaviour
{

    public static SCP_UI instance = null;
    public Image eyes;
    public Canvas PauseM;
    public GameObject canvas, SNav;
    public Canvas Inventory, Death, Screen;
    public Image ScreenText;
    public Canvas HUD;
    public EventSystem menu;
    public AudioClip[] inventory;
    Menu currMenu = Menu.None;

    public Image blinkBar, Overlay, handEquip, runBar, navBar;

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

    public void ItemSFX(int sfx)
    {
        GameController.instance.GlobalSFX.PlayOneShot(inventory[sfx]);
    }

    public void TogglePauseMenu()
    {
        if (currMenu == Menu.Pause)
        {
            Debug.Log("Quitando pausa");
            PauseM.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1.0f;
            currMenu = Menu.None;
            AudioListener.pause = false;
            return;
        }
        if (currMenu == Menu.None)
        {
            PauseM.enabled = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            currMenu = Menu.Pause;
            AudioListener.pause = true;
            return;
        }
    }

    public void ToggleInventory()
    {
        if (currMenu == Menu.Inv)
        {
            Inventory.enabled = false;
            Time.timeScale = 1.0f;
            ItemController.instance.CloseInv();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            currMenu = Menu.None;
            AudioListener.pause = false;
            return;
        }
        if (currMenu == Menu.None)
        {
            ItemController.instance.OpenInv();
            Inventory.enabled = true;
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            currMenu = Menu.Inv;
            menu.SetSelectedGameObject(defInv);
            AudioListener.pause = true;

            return;
        }
    }

    public void ToggleDeath()
    {
        if (currMenu == Menu.Death)
        {
            Death.enabled = false;
            Time.timeScale = 1.0f;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            currMenu = Menu.None;
            return;
        }
        else
        {
            Death.enabled = true;
            Time.timeScale = 1.0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            currMenu = Menu.Death;
            return;
        }
    }

    public void ToggleScreen()
    {
        if (currMenu == Menu.Screen)
        {
            Screen.enabled = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            currMenu = Menu.None;
            AudioListener.pause = false;
            return;
        }
        if (currMenu == Menu.None)
        {
            Screen.enabled = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            currMenu = Menu.Screen;
            AudioListener.pause = true;
            return;
        }
    }

    public bool ToggleConsole()
    {
        if (currMenu == Menu.Debug)
        {
            Time.timeScale = 1.0f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            currMenu = Menu.None;
            return(false);
        }
        if (currMenu == Menu.None)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            currMenu = Menu.Debug;
            return(true);
        }
        return (false);
    }
}
