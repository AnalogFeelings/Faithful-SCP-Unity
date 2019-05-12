using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Screen : Object_Interact
{
    // Start is called before the first frame update
    public Sprite screen;
    bool Active = false;

    // Update is called once per frame
    public override void Pressed()
    {
        if (Active == false)
        {
            SCP_UI.instance.ScreenText.sprite = screen;
            SCP_UI.instance.ToggleScreen();
            GameController.instance.player.GetComponent<Player_Control>().Freeze = true;
            GameController.instance.player.GetComponent<Player_Control>().ForceLook(transform.position, 4f);
            Active = true;

        }
        else
        {
            SCP_UI.instance.ToggleScreen();
            GameController.instance.player.GetComponent<Player_Control>().Freeze = false;
            GameController.instance.player.GetComponent<Player_Control>().StopLook();
            Active = false;
        }

    }
}
