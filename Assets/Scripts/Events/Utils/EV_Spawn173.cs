using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EV_Spawn173 : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        GameController.instance.Warp173(true, transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
