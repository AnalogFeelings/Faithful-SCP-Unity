using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnchor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameController.instance.WorldAnchor = transform.position;
    }

}
