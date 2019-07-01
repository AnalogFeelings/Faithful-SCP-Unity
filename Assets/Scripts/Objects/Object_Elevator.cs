using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Elevator : MonoBehaviour
{
    public GameObject Floor1, Floor2;
    public Object_Button_Trigger Out1, Out2, Switch1, Switch2;
    public Object_Door Door1, Door2;
    bool Ignore1, Ignore2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Switch1.GetComponent<Object_Button_Trigger>().activated == true && Ignore1 == false)
        {
            GameController.instance.holdRoom = true;
            SwitchFloor(Floor1.transform, Floor2.transform);
            Ignore1 = true;
            Ignore2 = false;
        }
        if (Switch2.GetComponent<Object_Button_Trigger>().activated == true && Ignore2 == false)
        {
            SwitchFloor(Floor2.transform, Floor1.transform);
            GameController.instance.holdRoom = false;
            Ignore2 = true;
            Ignore1 = false;
        }
    }

    void SwitchFloor(Transform start, Transform end)
    {
        GameObject objPlayer = GameController.instance.player;
        objPlayer.GetComponent<Player_Control>().playerWarp((end.transform.position + ((end.transform.rotation) * (objPlayer.transform.position - start.position))), end.transform.eulerAngles.y - start.transform.eulerAngles.y);
        Debug.Log("Diferencia de Rotacion: " + (end.transform.eulerAngles.y - start.transform.eulerAngles.y));

    }
}
