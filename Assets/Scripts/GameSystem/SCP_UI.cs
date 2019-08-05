using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum Menu { None, Pause, Inv , Death, Screen, Debug, Options};

public class SCP_UI : MonoBehaviour
{

    public static SCP_UI instance = null;
    public Image eyes, eyegraphics;
    public Canvas PauseM;
    public GameObject canvas, SNav, notifprefab;
    public Canvas Inventory, Death, Screen, Options;
    public Image ScreenText;
    public Canvas HUD;
    public EventSystem menu;
    public AudioClip[] inventory;
    public AudioClip menublip;
    public Text Info1, Info2, DeathMSG;
    public Button save;
    Menu currMenu = Menu.None;

    bool canConsole, canTuto;

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
        GameController.instance.MenuSFX.PlayOneShot(inventory[sfx]);
    }

    public void TogglePauseMenu()
    {
        if (currMenu == Menu.Pause)
        {
            
            GameController.instance.MenuSFX.PlayOneShot(menublip);
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
            Info1.text = string.Format(GlobalValues.uiStrings["ui_in_info"], GlobalValues.design, GlobalValues.playername, GlobalValues.mapname, GlobalValues.mapseed);


            PauseM.enabled = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            currMenu = Menu.Pause;
            AudioListener.pause = true;
            return;
        }
    }

    public void ToggleOptionsMenu()
    {
        if (currMenu == Menu.Options)
        {
            GameController.instance.MenuSFX.PlayOneShot(menublip);
            Options.enabled = false;
            PauseM.enabled = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            currMenu = Menu.Pause;
            AudioListener.pause = true;
            GameController.instance.LoadUserValues();
            return;
        }
        if (currMenu == Menu.None || currMenu == Menu.Pause)
        {
            GameController.instance.MenuSFX.PlayOneShot(menublip);
            PauseM.enabled = false;
            Options.enabled = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            currMenu = Menu.Options;
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
            DeathMSG.text = GameController.instance.deathmsg;
            Death.enabled = false;
            Time.timeScale = 1.0f;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            currMenu = Menu.None;
            return;
        }
        else
        {
            if (!GlobalValues.hasSaved)
                save.interactable = false;
            else
                save.interactable = true;
            Info2.text = string.Format(GlobalValues.uiStrings["ui_in_info"], GlobalValues.design, GlobalValues.playername, GlobalValues.mapname, GlobalValues.mapseed);
            DeathMSG.text = GameController.instance.deathmsg;
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

            GameController.instance.MenuSFX.PlayOneShot(menublip);
            /*Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;*/
            currMenu = Menu.None;
            return;
        }
        if (currMenu == Menu.None)
        {
            Screen.enabled = true;
            /*Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;*/
            currMenu = Menu.Screen;
            return;
        }
    }

    public bool ToggleConsole()
    {
        if (canConsole)
        {
            if (currMenu == Menu.Debug)
            {
                Time.timeScale = 1.0f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                currMenu = Menu.None;
                return (false);
            }
            if (currMenu == Menu.None)
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                currMenu = Menu.Debug;
                return (true);
            }
        }
        return (false);
    }


    public void LoadValues()
    {
        canConsole = (PlayerPrefs.GetInt("Debug", 0) == 1);
        canTuto = (PlayerPrefs.GetInt("Tutorials", 0) == 1);
    }



    public void ShowTutorial(string tuto)
    {
        if (canTuto)
        {
            GameObject notif = Instantiate(notifprefab, canvas.transform);
            NotifSystem notifval = notif.GetComponent<NotifSystem>();
            notifval.image.sprite = Resources.Load<Sprite>("Tutorials/" + tuto);
            notifval.body.text = GlobalValues.tutoStrings[tuto];
        }
    }
}
