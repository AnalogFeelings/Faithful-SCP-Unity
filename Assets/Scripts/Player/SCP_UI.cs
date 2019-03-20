using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCP_UI : MonoBehaviour
{
    public static SCP_UI instance = null;
    public Image eyes;
    public Canvas PauseM;
    public Canvas Inventory;
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
        Debug.Log("Tumama");
        // not the optimal way but for the sake of readability
        if (PauseM.enabled)
        {
            Time.timeScale = 1.0f;
            PauseM.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }
        else
        {
            PauseM.enabled = true;
            Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
            Time.timeScale = 0f;
        }

    }

    public void ToggleInventory()
    {
        Debug.Log("Tumama2");
        // not the optimal way but for the sake of readability
        if (Inventory.enabled)
        {
            Inventory.enabled = false;
            Time.timeScale = 1.0f;

            Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }
        else
        {
            Inventory.enabled = true;
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;
        }

    }
}
