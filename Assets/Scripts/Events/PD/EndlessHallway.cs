using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessHallway : MonoBehaviour
{
    public Transform Pos1, Pos2;
    public BoxTrigger Box1, Box2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Box1.GetState())
        {
            Switch(Pos1, Pos2);
        }

        if (Box2.GetState())
        {
            Switch(Pos2, Pos1);
        }
    }


    void Switch(Transform start, Transform end)
    {
        GameObject objPlayer = GameController.instance.player;
        objPlayer.GetComponent<Player_Control>().playerWarp((end.transform.position + ((end.transform.rotation * Quaternion.Inverse(start.transform.rotation)) * (objPlayer.transform.position - start.position))), end.transform.eulerAngles.y - start.transform.eulerAngles.y);
        Debug.Log("Diferencia de Rotacion: " + (end.transform.eulerAngles.y - start.transform.eulerAngles.y));

    }
}
