using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cb_Mode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GlobalValues.isNew)
        {
            GameController.instance.globalBools.Add(false); //HEAVYZONE LOCK
            GameController.instance.globalBools.Add(false); //LIGHTZONE LOCK
            GameController.instance.globalBools.Add(true); //DEMO LOCK
            GameController.instance.globalBools.Add(false); //LIGHTS ENABLED
            GameController.instance.globalBools.Add(false); //106 ENABLED
            GameController.instance.globalBools.Add(false); //DEMO MTF Espawned

            GameController.instance.globalFloats.Add(Random.Range(840, 1050)); //LARRY TIMER
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.instance.doGameplay)
        {
            GameController.instance.globalFloats[0] -= Time.deltaTime;
            if (GameController.instance.globalFloats[0] < 0)
            {
                GameController.instance.globalFloats[0] = Random.Range(840, 1050); //LARRY TIMER
                Vector3 newpos = GameController.instance.player.transform.position;
                newpos.y -= 1f;
                GameController.instance.npcTable[(int)npc.scp106].Spawn(true, newpos);
            }
        }
    }
}
