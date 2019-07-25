using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EV_SurvRoom : Event_Parent
{
    public ScreenRenderer blockscreen;
    public GameObject parentMTF;
    public Object_LeverV lever;
    public BoxTrigger endtrigger;
    testpath_ mtf1_, mtf2_;
    public Transform point1, point2, spawn1, spawn2;
    public Transform[] points;
    public AudioClip aviso1, aviso2, respuesta;
    bool normalfinish = false;
    int status = 0;
    float Timer = 10;
    public int HowLong;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isStarted)
        {
            EventUpdate();
        }
    }

    /*public override void EventStart()
    {
        base.EventStart();
    }*/

    public override void EventUpdate()
    {
        if (status == 0 && lever.On == false)
        {
            GameController.instance.globalBools[0] = true;
            status = 1;
            Timer = 7;
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
                        GameController.instance.globalBools[5] = true;
                        GameObject spawned1 = Instantiate(parentMTF, spawn1.transform.position, Quaternion.identity, GameController.instance.npcParent.transform);
                        mtf1_ = spawned1.GetComponent<testpath_>();
                        GameObject spawned2 = Instantiate(parentMTF, spawn2.transform.position, Quaternion.identity, GameController.instance.npcParent.transform);
                        mtf2_ = spawned2.GetComponent<testpath_>();

                        status = 2;
                        GameController.instance.GlobalSFX.PlayOneShot(aviso1);
                        mtf1_.patrol = points;
                        mtf2_.patrol = points;
                        mtf1_.GoHere(point1.transform.position);
                        mtf2_.GoHere(point2.transform.position);
                        Timer = 0.1f;
                        
                        break;
                    }
                case 2:
                    {
                        SCP_UI.instance.ShowTutorial("tutohide1");
                        Timer = HowLong;
                        status = 3;
                        break;
                    }
                case 3:
                    {
                        GameController.instance.GlobalSFX.PlayOneShot(aviso2);
                        Timer = 10;
                        status = 4;
                        break;
                    }
                case 4:
                    {
                        mtf1_.Audio.PlayOneShot(respuesta);
                        Timer = 8;
                        status = 5;
                        break;
                    }
                case 5:
                    {
                        status = 6;
                        break;
                    }

            }

        }

        if (endtrigger.GetState() && status != 0)
        {
            if (status != 6)
            {
                normalfinish = true;
                mtf1_.StopThis();
                mtf2_.StopThis();
                mtf1_.WorldSearch = true;
                mtf2_.WorldSearch = true;
                EventFinished();
            }
            else
            {
                normalfinish = true;
                mtf1_.WorldSearch = true;
                mtf2_.WorldSearch = true;
                EventFinished();
            }
        }

    }

    public override void EventFinished()
    {
        base.EventFinished();
        isStarted = false;

        if (GameController.instance.globalBools[5] == false)
        {
            GameObject spawned1 = Instantiate(parentMTF, spawn1.transform.position, Quaternion.identity);
            mtf1_ = spawned1.GetComponent<testpath_>();
            spawned1 = Instantiate(parentMTF, spawn2.transform.position, Quaternion.identity);
            mtf2_ = spawned1.GetComponent<testpath_>();
            mtf1_.patrol = points;
            mtf2_.patrol = points;
            mtf1_.WorldSearch = true;
            mtf2_.WorldSearch = true;
        }
    }
}
