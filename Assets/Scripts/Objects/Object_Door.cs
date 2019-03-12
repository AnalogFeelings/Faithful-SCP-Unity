using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Object_Door : MonoBehaviour
{
    public GameObject Door01, Door02;
    public float OpenSpeed, DoorEndPos, DoorTime;

    public AudioClip[] Open_AUD;
    public AudioClip[] SCP_AUD;
    public AudioClip[] Close_AUD;
    AudioSource AUD;

    float LastPos1;

    Vector3 Pos1, Pos2;
    public bool switchOpen = false, scp173 = true;
    bool IsOpen = false, isForcing = false;
    
    // Start is called before the first frame update
    void Start()
    {
        Pos1 = Door01.transform.position;
        Pos2 = Door02.transform.position;
        LastPos1 = 10f;
        AUD = GetComponent<AudioSource>();
        //DoorOpen();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOpen == false && switchOpen == true)
            DoorOpen();

        if (switchOpen == false && IsOpen == true)
            DoorClose();

        if (isForcing == true)
            Hodor();
    }

    void DoorOpen()
    {
        float tempdis = Vector3.Distance(Door01.transform.position, Pos1 + (Door01.transform.right * DoorEndPos));
        if (tempdis >= 0.02)
        {
            Door01.transform.position += Door01.transform.right * OpenSpeed * Time.deltaTime;
            if (tempdis > LastPos1)
                IsOpen = true;
            else
                LastPos1 = tempdis;
            
        }
        else IsOpen = true;

        tempdis = Vector3.Distance(Door02.transform.position, Pos2 + (Door02.transform.right * DoorEndPos));
        if (tempdis >= 0.02)
        {
            Door02.transform.position += Door02.transform.right * OpenSpeed * Time.deltaTime;
        }




    }

    void DoorClose()
    {
        float tempdis = Vector3.Distance(Door01.transform.position, Pos1);
        if (tempdis >= 0.02)
        {
            Door01.transform.position += Door01.transform.right * -OpenSpeed * Time.deltaTime;
            if (tempdis > LastPos1)
                IsOpen = false;
            else
                LastPos1 = tempdis;
        }
        else IsOpen = false;

        tempdis = Vector3.Distance(Door02.transform.position, Pos2);
        if (tempdis >= 0.02)
            Door02.transform.position += Door02.transform.right * -OpenSpeed * Time.deltaTime;
    }

    public void DoorSwitch()
    {

        if (switchOpen == true && IsOpen == true)
        {
            switchOpen = false;
            PlayClose();
            LastPos1 = 10f;
        }
        if (switchOpen == false && IsOpen == false)
        {
            switchOpen = true;
            PlayOpen();
            LastPos1 = 10f;
        }
    }

    public void ForceOpen(float time)
    {
        isForcing = true;
        DoorTime = time;
        Debug.Log("HoldTheDooor");
    }

    void Hodor()
    {
        if (switchOpen != true)
            DoorSwitch();
        DoorTime -= (Time.deltaTime);
            if (DoorTime <= 0.0f)
            {
            isForcing = false;
            switchOpen = false;
            }
    }



    public bool Door173()
    {
        if (switchOpen == false && IsOpen == false && scp173 == true)
        {
            switchOpen = true;
            PlaySCP(0);
            LastPos1 = 10f;
            return (true);
        }
        else
        {
            return (false);
        }
    }

    void PlayOpen()
    {
        AUD.clip = Open_AUD[Random.Range(0, Open_AUD.Length)];
        AUD.Play();
    }
    void PlaySCP(int i)
    {
        AUD.clip = SCP_AUD[i];
        AUD.Play();
    }
    void PlayClose()
    {
        AUD.clip = Close_AUD[Random.Range(0, Close_AUD.Length)];
        AUD.Play();
    }

    public bool GetState()
    {
        return (IsOpen);
    }

}
