using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSound : MonoBehaviour
{
<<<<<<< HEAD
    public int SoundLevel;
=======
    public float SoundLevel;
>>>>>>> b51288c654010cd38746c4fc80b787219a127ef4
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
