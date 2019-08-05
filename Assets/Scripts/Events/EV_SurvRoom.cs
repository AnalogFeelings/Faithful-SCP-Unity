using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EV_SurvRoom : Event_Parent
{
    public ScreenRenderer blockscreen;
    public Object_LeverV lever;
    public BoxTrigger endtrigger;
    public Transform spawn1;
    public Transform[] points;
    int status = 0;
    float Timer = 10;
    public int HowLong;
    
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if (isStarted)
        {
            EventUpdate();
        }
    }

    public override void EventUpdate()
    {
        if (status == 0 && lever.On == false)
        {
            GameController.instance.globalBools[0] = true;
            status = 1;
            Timer = 5;
            blockscreen.animate = false;
            blockscreen.SetFrame(0, 0);
        }

        if (status != 0)
            Timer -= Time.deltaTime;

        if (Timer <= 0)
        {
            switch (status)
            {
                case 1:
                    {
                        GameController.instance.npcTable[(int)npc.scp106].Event_Spawn(true, spawn1.position);
                        ((SCP_106)GameController.instance.npcTable[(int)npc.scp106]).SetPath(points);
                        status = 2;

                        Timer = 0.1f;
                        
                        break;
                    }
                case 2:
                    {
                        Timer = HowLong;
                        status = 3;
                        break;
                    }
                case 3:
                    {
                        GameController.instance.npcTable[(int)npc.scp106].StopEvent();
                        EventFinished();
                        status = 5;
                        break;
                    }



            }

        }

        if (endtrigger.GetState() && status != 0)
        {
            GameController.instance.npcTable[(int)npc.scp106].StopEvent();
            EventFinished();
        }

    }

    public override void EventFinished()
    {
        base.EventFinished();
        isStarted = false;
    }
}
