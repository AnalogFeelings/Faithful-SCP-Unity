using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSound : MonoBehaviour
{
    public float SoundLevel;
    float Timer = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer < 0)
        {
            Destroy(this.gameObject);
        }
    }


}
