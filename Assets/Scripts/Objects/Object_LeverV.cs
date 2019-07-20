using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using Pixelplacement.TweenSystem;

public class Object_LeverV : Object_Interact
{
    public bool On = false;
    public bool OnUp = false;
    bool isUp = false;
    public GameObject Handle;
    public AudioSource sfx;
    public AudioClip Switch;

    float changing = 0;
    bool start, change, cooldown;
    // Start is called before the first frame update
    void Update()
    {
    }

    public void HandleUpdate(float value)
    {
        Handle.transform.localRotation = Quaternion.Euler(value, 0, 0);
    }

    private void Start()
    {
        if ((!On && !OnUp) || (On && OnUp))
        {
            Tween.Value(76.98701f, -80.399f, HandleUpdate, 0.6f, 0f, Tween.EaseInStrong, Tween.LoopType.None, null);
            isUp = true;
        }
    }


    // Update is called once per frame
    public override void Hold()
    {
        if (Input.GetAxis("Mouse Y") != 0)
        {
            changing -= Time.deltaTime;
            if (changing <= 0)
            {
                if (!cooldown)
                {
                    change = true;
                    start = false;
                }
                else
                {
                    change = false;
                    cooldown = false;
                }
            }

            if (change == true && !cooldown)
            {

                if (Input.GetAxis("Mouse Y") > 0)
                {
                    if (!isUp)
                    {
                        sfx.PlayOneShot(Switch);
                        if (OnUp == true)
                            On = true;
                        else
                            On = false;

                        Tween.Value(76.98701f, -80.399f, HandleUpdate, 0.6f, 0f, Tween.EaseInStrong, Tween.LoopType.None, null);
                        Debug.Log("GoingUp");
                        cooldown = true;
                        changing = 0.4f;
                        isUp = true;
                    }
                }
                if (Input.GetAxis("Mouse Y") < 0)
                {
                    if (isUp)
                    {
                        sfx.PlayOneShot(Switch);
                        Debug.Log("GoingDown");
                        if (OnUp == true)
                            On = false;
                        else
                            On = true;

                        Tween.Value(-80.399f, 76.98701f, HandleUpdate, 0.6f, 0, Tween.EaseInStrong, Tween.LoopType.None, null);
                        cooldown = true;
                        changing = 0.4f;
                        isUp = false;
                    }
                }
            }
        }

        if (start == false)
        {
            changing = 0.3f;
            start = true;
        }


    }
}