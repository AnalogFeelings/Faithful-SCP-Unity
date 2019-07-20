using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavController : MonoBehaviour
{
    public GameObject Display, Offline, Battery, mapFull, mapFill;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Equipable_Nav Nav = ((Equipable_Nav)GameController.instance.player.GetComponent<Player_Control>().equipment[(int)bodyPart.Hand]);
        if (Nav.isOnline)
        {
            Offline.SetActive(false);
            mapFull.SetActive(true);
            mapFill.SetActive(false);
        }
        else
        {
            Offline.SetActive(true);
            mapFull.SetActive(false);
            mapFill.SetActive(true);
        }

        if (Nav.valueFloat < 0)
            Display.SetActive(false);
        else
            Display.SetActive(true);
    }
}
