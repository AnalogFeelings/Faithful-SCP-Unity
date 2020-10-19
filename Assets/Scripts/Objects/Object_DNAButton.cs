using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_DNAButton : Object_Interact
{
    public GameObject Door01, Door02;
    public int handID;
    public AudioClip Accepted, Rejected;
    public AudioSource soundsource;
    public bool WaitForBool;
    public int ThisValue;
    public override void Pressed()
    {
        Player_Control player = GameController.instance.player.GetComponent<Player_Control>();

        if (!WaitForBool || (WaitForBool && GameController.instance.globalBools[ThisValue]))
        {
            if (player.equipment[(int)bodyPart.Hand] != null && ItemController.instance.items[player.equipment[(int)bodyPart.Hand].itemName] is Equipable_Key)
            {
                Equipable_Hand hand;
                hand = (Equipable_Hand)ItemController.instance.items[player.equipment[(int)bodyPart.Hand].itemName];
                if (hand.handID == handID)
                {
                    Door01.GetComponent<Object_Door>().DoorSwitch();
                    if (Door02 != null)
                        Door02.GetComponent<Object_Door>().DoorSwitch();
                    soundsource.PlayOneShot(Accepted);
                    SubtitleEngine.instance.playSub("playStrings", "play_button_card");
                }
                else
                {
                    SubtitleEngine.instance.playSub("playStrings", "play_button_lowcard");
                    soundsource.PlayOneShot(Rejected);
                }

            }
            else
                SubtitleEngine.instance.playSub("playStrings", "play_button_nocard");
        }
        else
        {
            SubtitleEngine.instance.playSub("playStrings", "play_button_failcard");
            soundsource.PlayOneShot(Rejected);
        }
    }

    public override void Hold()
    {
    }
}

