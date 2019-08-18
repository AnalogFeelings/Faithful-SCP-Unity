using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Button_Trigger : Object_Interact
{
    float deactivate;
    public bool activated;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        deactivate -= Time.deltaTime;
        if (deactivate <= 0)
            activated = false;
    }

    public override void Pressed()
    {
        deactivate = 1.0f;
        activated = true;
        SubtitleEngine.instance.playSub("playStrings", "play_button");
    }

    public override void Hold()
    {
    }
}
