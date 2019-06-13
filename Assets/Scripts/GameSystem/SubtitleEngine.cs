using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubtitleEngine : MonoBehaviour
{
    public static SubtitleEngine instance = null;
    float subtitle_add;
    float[] subtitle_hold = new float [3];
    public float subtitle_hold_timer, subtitle_add_timer;

    Text [] line = new Text [3];
    List<string> pending = new List<string>();
    string [] current = new string [3];

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void moveSubtitles()
    {
        for (int i = 1; i < 2; i++)
        {
            if (current[i-1]==null)
            {
                current[i - 1] = current[i];
                current[i] = null;
            }
        }
    }


}
