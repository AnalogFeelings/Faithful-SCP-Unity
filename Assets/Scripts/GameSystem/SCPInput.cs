using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SCPInput : MonoBehaviour
{
    public DefaultActions playerInput;
    public static SCPInput instance;
    public Text inputText;
    public GameObject rebindScreen;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            playerInput = new DefaultActions();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            DestroyImmediate(this.gameObject);
        }
    }

    private void Update()
    {
        
        //if (playerInput.UI.enabled && playerInput.UI.PointerMove.triggered)
            //Mouse.current.WarpCursorPosition(Mouse.current.position.ReadValue() + playerInput.UI.PointerMove.ReadValue<Vector2>());
    }

    public void ToGameplay()
    {
        playerInput.Gameplay.Enable();
    }
    public void ToUI()
    {
        /*playerInput.UI.Enable();*/
        playerInput.Gameplay.Disable();
    }

    public void TestRebind()
    {
        RemapButtonClicked(playerInput.Gameplay.Blink);
    }

    void RemapButtonClicked(InputAction actionToRebind)
    {
        actionToRebind.Disable();
        inputText.text = string.Format(Localization.GetString("uiStrings", "ui_input_rebind"), actionToRebind.name);
        rebindScreen.SetActive(true);
        var rebindOperation = actionToRebind.PerformInteractiveRebinding()
                    // To avoid accidental input from mouse motion
                    //.WithControlsExcluding("Mouse")
                    .WithTargetBinding(0)
                    .WithExpectedControlType("Button")
                    .OnMatchWaitForAnother(0.1f)
                    .OnComplete(
                   operation =>
                   {
                       Debug.Log($"Rebound '{actionToRebind}' to '{operation.selectedControl}'");
                       operation.Dispose();
                       StopRebind(actionToRebind);
                   })
                    .Start();
    }

    public void StopRebind(InputAction actionToRebind)
    {
        actionToRebind.Enable();
        rebindScreen.SetActive(false);
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
}
