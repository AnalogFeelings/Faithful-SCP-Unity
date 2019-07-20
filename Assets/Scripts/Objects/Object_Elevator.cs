using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Elevator : MonoBehaviour
{
    public Transform Floor1, Floor2;
    public Object_Button_Trigger Out1, Out2, Switch1, Switch2;
    public Object_Door Door1, Door2;
    public bool FloorUp;
    public float MovingTime;
    bool Ignoreinputs = false;
    bool insideElev;
    bool calledFromUp;
    public AudioClip elev, ding;
    bool soundPlayed = true;

    float Timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Out1.GetComponent<Object_Button_Trigger>().activated == true && !Ignoreinputs && Timer <= 0)
        {
            if (!FloorUp)
            {
                CloseDoors();
                Timer = MovingTime;
                insideElev = false;
                Ignoreinputs = true;
                soundPlayed = false;
            }
            else
                Door1.DoorSwitch();
        }

        if (Out2.GetComponent<Object_Button_Trigger>().activated == true && !Ignoreinputs && Timer <= 0)
        {
            if (FloorUp)
            {
                CloseDoors();
                Timer = MovingTime;
                insideElev = false;
                Ignoreinputs = true;
                soundPlayed = false;
            }
            else
                Door2.DoorSwitch();
        }

        if ((Switch1.GetComponent<Object_Button_Trigger>().activated == true || Switch2.GetComponent<Object_Button_Trigger>().activated == true) && !Ignoreinputs && Timer <= 0)
        {
            //SwitchFloor(Floor1.transform, Floor2.transform);
            CloseDoors();
            Timer = MovingTime;
            insideElev = true;
            Ignoreinputs = true;
            soundPlayed = false;
        }


        Timer -= Time.deltaTime;

        if (Timer <= (MovingTime - 2) && !soundPlayed)
        {
            GameController.instance.GlobalSFX.PlayOneShot(elev);
            soundPlayed = true;
        }

        if (Timer <= 3 && Ignoreinputs)
        {
            FloorUp = !FloorUp;
            if (insideElev)
            {
                if (FloorUp)
                    SwitchFloor(Floor2, Floor1);
                else
                    SwitchFloor(Floor1, Floor2);
            }

            if (FloorUp)
                Door1.DoorSwitch();
            else
                Door2.DoorSwitch();

            GameController.instance.GlobalSFX.PlayOneShot(ding);
            Ignoreinputs = false;


        }



        

    }

    void SwitchFloor(Transform start, Transform end)
    {
        GameObject objPlayer = GameController.instance.player;
        objPlayer.GetComponent<Player_Control>().playerWarp((end.transform.position + ((end.transform.rotation * Quaternion.Inverse(start.transform.rotation)) * (objPlayer.transform.position - start.position))), end.transform.eulerAngles.y - start.transform.eulerAngles.y);
        Debug.Log("Diferencia de Rotacion: " + (end.transform.eulerAngles.y - start.transform.eulerAngles.y));

    }

    void CloseDoors()
    {
        if (Door1.switchOpen)
            Door1.DoorSwitch();
        if (Door2.switchOpen)
            Door2.DoorSwitch();
    }
}
