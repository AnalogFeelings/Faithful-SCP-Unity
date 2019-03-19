using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ItemController : MonoBehaviour
{
    public static ItemController instance = null;
    public slotController [] slots;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
