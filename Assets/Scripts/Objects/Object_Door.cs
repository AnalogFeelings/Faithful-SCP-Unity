using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Object_Door : MonoBehaviour
{
    public GameObject Door01, Door02;
    public float OpenSpeed, DoorEndPos, DoorTime;
    public int id;

    public AudioClip[] Open_AUD;
    public AudioClip[] SCP_AUD;
    public AudioClip[] Close_AUD;
    public AudioSource AUD;

    public bool UseParticle = false;

    float LastPos1;

    Vector3 Pos1, Pos2;
    public bool switchOpen = false, scp173 = true, ignoreSave = false;
    bool IsOpen = false, isForcing = false;

    // Start is called before the first frame update

    private void Awake()
    {
        AUD = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (!ignoreSave)
        {
            id = GameController.instance.GetDoorID();
            transform.parent = GameController.instance.doorParent.transform;
            resetState();
        }

        Pos1 = Door01.transform.position;
        Pos2 = Door02.transform.position;
        LastPos1 = 10f;
    }

    public void resetState()
    {
        int doorState = GameController.instance.GetDoorState(id);
        if (doorState != -1)
        {
            LastPos1 = 10f;

            if (doorState == 0)
            {
                IsOpen = true;
                switchOpen = false;
            }
            if (doorState == 1)
            {
                IsOpen = false;
                switchOpen = true;
            }
        }
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
            {
                Door01.transform.position = Pos1 + (Door01.transform.right * DoorEndPos);
                Door02.transform.position = Pos2 + (Door02.transform.right * DoorEndPos);
                IsOpen = true;
                if (!ignoreSave)
                    GameController.instance.SetDoorState(true, id);
            }
            else
                LastPos1 = tempdis;

        }
        else
        {
            IsOpen = true;
            if (!ignoreSave)
                GameController.instance.SetDoorState(true, id);
        }

        tempdis = Vector3.Distance(Door02.transform.position, Pos2 + (Door02.transform.right * DoorEndPos));
        if (tempdis >= 0.02)
        {
            Door02.transform.position += Door02.transform.right * OpenSpeed * Time.deltaTime;
        }




    }

    void DoorClose()
    {
        float tempdis = Vector3.Distance(Door01.transform.position, Pos1);
        if (tempdis >= 0.00002)
        {
            Door01.transform.position += Door01.transform.right * -OpenSpeed * Time.deltaTime;
            if (tempdis > LastPos1)
            {
                Door01.transform.position = Pos1;
                Door02.transform.position = Pos2;

                if (UseParticle)
                    GameController.instance.particleController.StartParticle(1, transform.position, transform.rotation);

                IsOpen = false;
                if (!ignoreSave)
                    GameController.instance.SetDoorState(false, id);
            }
            else
                LastPos1 = tempdis;
        }
        else
        {
            IsOpen = false;
            if (!ignoreSave)
                GameController.instance.SetDoorState(false, id);
        }

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
    }

    void Hodor()
    {
        if (switchOpen != true)
            DoorSwitch();
        DoorTime -= (Time.deltaTime);
        if (DoorTime <= 0.0f)
        {
            isForcing = false;
            DoorSwitch();
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
