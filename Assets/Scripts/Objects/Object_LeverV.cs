using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using Pixelplacement.TweenSystem;

public class Object_LeverV : Object_Interact
{
    public bool On = false;
    public bool OnUp = false;
    public GameObject Handle;

    float changing = 0;
    bool start, change;
    // Start is called before the first frame update
    void Update()
    {
    }

    public void HandleUpdate(float value)
    {
        Handle.transform.localRotation = Quaternion.Euler(value, 0, 0);
    }


    // Update is called once per frame
    public override void Hold()
    {
        if (Input.GetAxis("Mouse Y") != 0)
        {
            changing -= Time.deltaTime;
            if (changing <= 0)
            {
                change = true;
                start = false;
            }

            if (change == true)
            {

                if (Input.GetAxis("Mouse Y") > 0)
                {
                    if (OnUp == false)
                        On = true;
                    else
                        On = false;

                    Tween.Value(76.98701f, -80.399f, HandleUpdate, 0.6f, 0f, Tween.EaseInStrong, Tween.LoopType.None, null);
                }
                if (Input.GetAxis("Mouse Y") < 0)
                {
                    if (OnUp == false)
                        On = false;
                    else
                        On = true;

                    Tween.Value(-80.399f, 76.98701f, HandleUpdate, 0.6f, 0, Tween.EaseInStrong, Tween.LoopType.None, null);
                }
                change = false;
            }
        }

        if (start == false)
        {
            changing = 0.4f;
            start = true;
        }


    }
}