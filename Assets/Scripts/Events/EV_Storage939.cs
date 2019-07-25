using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EV_Storage939 : Event_Parent
{
    public Object_Elevator ele1, ele2;
    public Object_LeverV lever1, lever2;
    public Object_Door door;
    public GameObject audio;

    bool active_lev1, active_lev2, up_ele1=true, up_ele2=true;

    // Start is called before the first frame update
    void Start()
    {
        GameController.instance.setValue(x, y, 0, ele1.FloorUp ? 1 : 0);
        GameController.instance.setValue(x, y, 1, ele2.FloorUp ? 1 : 0);
        EventFinished();
    }

    // Update is called once per frame
    void Update()
    {
        if (up_ele1 != ele1.FloorUp)
        {
            GameController.instance.setValue(x, y, 0, ele1.FloorUp ? 1 : 0);
            up_ele1 = ele1.FloorUp;

            if (up_ele1 == false)
            {
                audio.SetActive(true);
                GameController.instance.holdRoom = true;
                if (GameController.instance.getValue(x, y, 4) == 0)
                {
                    SCP_UI.instance.ShowTutorial("tutohide2");
                    GameController.instance.setValue(x, y, 4, 1);
                }
            }
            else
            {
                audio.SetActive(false);
                GameController.instance.holdRoom = false;
            }


        }
        if (up_ele2 != ele2.FloorUp)
        {
            GameController.instance.setValue(x, y, 1, ele2.FloorUp ? 1 : 0);
            up_ele2 = ele2.FloorUp;

            if (up_ele2 == false)
            {
                audio.SetActive(true);
                GameController.instance.holdRoom = true;
                if (GameController.instance.getValue(x, y, 4) == 0)
                {
                    SCP_UI.instance.ShowTutorial("tutohide2");
                    GameController.instance.setValue(x, y, 4, 1);
                }
            }
            else
            {
                audio.SetActive(false);
                GameController.instance.holdRoom = false;
            }


        }

        if (active_lev1 != lever1.On)
        {
            GameController.instance.setValue(x, y, 2, lever1.On ? 1 : 0);
            if (lever1.On || lever2.On)
            {
                if (!door.switchOpen)
                    door.DoorSwitch();
            }
            else if (door.switchOpen)
                door.DoorSwitch();

            active_lev1 = lever1.On;
        }

        if (active_lev2 != lever2.On)
        {
            GameController.instance.setValue(x, y, 3, lever2.On ? 1 : 0);
            if (lever1.On || lever2.On)
            {
                if (!door.switchOpen)
                    door.DoorSwitch();
            }
            else if (door.switchOpen)
                door.DoorSwitch();

            active_lev2 = lever2.On;
        }
    }

    public override void EventFinished()
    {
        base.EventFinished();
        up_ele1 = (GameController.instance.getValue(x, y, 0) == 1);
        up_ele2 = (GameController.instance.getValue(x, y, 1) == 1);
        active_lev1 = (GameController.instance.getValue(x, y, 3) == 1);
        active_lev2 = (GameController.instance.getValue(x, y, 4) == 1);

        ele1.FloorUp = up_ele1;
        ele2.FloorUp = up_ele2;

        lever1.On = active_lev1;
        lever2.On = active_lev2;

        if (lever1.On || lever2.On)
        {
            if (!door.switchOpen)
                door.DoorSwitch();
        }


    }
}
