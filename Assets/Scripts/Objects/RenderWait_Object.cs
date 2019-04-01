using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderWait_Object : MonoBehaviour
{
    public GameObject contains;
    float Timer = 30;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            contains.SetActive(false);
            Destroy(this);
        }
    }
}
