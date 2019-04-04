using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public enum bodyPart { Head, Body, Hand, Any };
public enum Ailment { Eyes, Sprint};

[System.Serializable]
public class effects
{
    public bool permanent;
    public float time;
    public float max;
    public float min;
    public float multiplier;
}

[System.Serializable]
public class efecttable
{
    public Ailment Affected;
    public effects effect;
}
public class Timers
{
    public float Timer;
    public bool Activated;
        public Timers()
        {
        Timer = 0;
        Activated = false;
        }
}


public class Player_Control : MonoBehaviour
{
    float InputX, InputY, BlinkingTimer, BlinkMult = 1, RunMult = 1, CloseTimer, AsfixTimer, Health = 100, speed, headBob, amplitude, lastBob=0, RunningTimer;
    public GameObject Camera, InterHold, DeathCol, handPos, CameraContainer;
    private GameObject hand;
    private Transform _groundChecker;
    public Transform DefHead, CrouchHead;
    public LayerMask Ground, InteractiveLayer;
    RaycastHit WallCheck;
    Vector3 holdCam, fallSpeed, movement, HoldPos, OriPos, totalmove, headPos;
    private CharacterController _controller;
    public float GroundDistance = 0.2f, baseAmplitude, bobSpeed, Gravity = -9.81f, maxfallspeed, Basespeed = 3, crouchspeed = 2, runSpeed = 4, BlinkingTimerBase, ClosedEyes, AsfixiaTimer, RunningTimerBase;
    bool Grounded = true, isGameplay = true, isSmoke = false, Crouch = false, fakeBlink, isRunning, isTired = false;
    Camera PlayerCam;
    Image eyes, blinkbar, runbar, overlay, handEquip;
    RectTransform hand_rect, hud_rect;

    public AudioClip[] Conch, NormalStep, Deaths, Breath;
    public AudioSource sfx, va;
    public AudioReverbZone Reverb;

    Collider[] Interact;

    //Iteeemssss
    public Equipable_Wear[] equipment = new Equipable_Wear[4];
    public effects[] playerEffects = new effects[2];
    public Timers[] effecTimers = new Timers[2];
    int headSlot = 0;
    int bodySlot = 0;
    int anySlot = 0;
    int handSlot = 0;

    float eyesMin, sprintMin;

    bool protectSmoke;


    // Start is called before the first frame update
    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        Reverb = GetComponent<AudioReverbZone>();
        _groundChecker = transform.GetChild(0);
        PlayerCam = Camera.GetComponent<Camera>();
        speed = Basespeed;
        headPos = DefHead.transform.position;
        eyes = SCP_UI.instance.eyes;
        blinkbar = SCP_UI.instance.blinkBar;
        runbar = SCP_UI.instance.runBar;
        hand = SCP_UI.instance.hand;
        overlay = SCP_UI.instance.Overlay;
        handEquip = SCP_UI.instance.handEquip;

        hand_rect = hand.GetComponent<RectTransform>();
        hud_rect = SCP_UI.instance.HUD.GetComponent<RectTransform>();

        playerEffects[0] = null;
        playerEffects[1] = null;
        effecTimers[0] = new Timers();
        effecTimers[1] = new Timers();


    }

    // Update is called once per frame
    void Update()
    {
        if (isGameplay && Time.timeScale == 1f)
        {
            ACT_Effects();
            ACT_Move();
            ACT_Gravity();
            ACT_Blinking();
            ACT_Running();
            ACT_Buttons();


            _controller.Move(movement * speed * Time.deltaTime);
        }
        if (Health <= 0)
            Death(0);
    }

    private void LateUpdate()
    {
        if (isGameplay && Time.timeScale == 1f)
        {
            ACT_HUD();
            ACT_Camera();
            ACT_Walk();
            if (isTired && !va.isPlaying)
            {
                va.clip = Breath[Random.Range(0, Breath.Length)];
                va.Play();
            }
        }
    }

    void ACT_Move()
    {
        Grounded = _controller.isGrounded;//Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);

        InputX = Input.GetAxis("Horizontal");
        InputY = Input.GetAxis("Vertical");

        movement = ((transform.right * InputX) + (transform.forward * InputY));
        Vector3.Normalize(movement);

        if (Input.GetButtonDown("Crouch") && !isRunning)
            Crouch = !Crouch;

        isRunning = (Input.GetButton("Run") && !Crouch && RunningTimer > 0.2f);

        speed = Basespeed;
        if (Crouch)
            speed = crouchspeed;

        if (isRunning)
            speed = runSpeed;

    }

    void ACT_Walk()
    {
        if(lastBob > 0 && headBob < 0)
        {
            sfx.PlayOneShot(NormalStep[Random.Range(0, NormalStep.Length)]);
        }
        lastBob = headBob;
    }

    void ACT_HUD()
    {
        int blinkPercent = ((int)Mathf.Ceil((BlinkingTimer / (BlinkingTimerBase / 100)) / 5));

        blinkbar.rectTransform.sizeDelta = new Vector2(blinkPercent * 8, 14);

        int runPercent = ((int)Mathf.Floor((RunningTimer / (RunningTimerBase / 100)) / 5));

        runbar.rectTransform.sizeDelta = new Vector2(runPercent * 8, 14);

        if (InterHold != null)
        {
            hand.SetActive(true);
            Vector3 screen = PlayerCam.WorldToScreenPoint(InterHold.transform.position);

            Vector3 heading = InterHold.transform.position - Camera.transform.position;
            if (Vector3.Dot(Camera.transform.forward, heading) < 0)
            {
                screen.y = 0f;
            }

            hand.transform.position = screen;
        }
        else
            hand.SetActive(false);

        Vector3 pos = hand_rect.localPosition;

        Vector3 minPosition = hud_rect.rect.min - hand_rect.rect.min;
        Vector3 maxPosition = hud_rect.rect.max - hand_rect.rect.max;

        pos.x = Mathf.Clamp(hand_rect.localPosition.x, minPosition.x, maxPosition.x);
        pos.y = Mathf.Clamp(hand_rect.localPosition.y, minPosition.y, maxPosition.y);

        hand_rect.localPosition = pos;

        if (Input.GetButtonDown("Unequip"))
            ACT_UnEquip(bodyPart.Hand);


    }

    void ACT_Camera()
    {
        holdCam = Camera.transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0.0f, holdCam.y, 0.0f);

        if (Crouch)
        {
            headPos.x = CrouchHead.transform.position.x;
            headPos.z = CrouchHead.transform.position.z;

            if (Vector3.Distance(headPos, CrouchHead.transform.position) > 0.005f)
                headPos.y = Mathf.Lerp(headPos.y, CrouchHead.transform.position.y, 15.0f * Time.deltaTime);
            else
                headPos.y = CrouchHead.transform.position.y;
        }
        else
        {
            headPos.x = DefHead.transform.position.x;
            headPos.z = DefHead.transform.position.z;

            if (Vector3.Distance(headPos, DefHead.transform.position) > 0.005f)
                headPos.y = Mathf.Lerp(headPos.y, DefHead.transform.position.y, 15.0f * Time.deltaTime);
            else
                headPos.y = DefHead.transform.position.y;
        }



        if ((InputX != 0 || InputY != 0)&& RunningTimer > 0.3f)
        {
            amplitude = baseAmplitude;
            HoldPos = headPos;
            headBob = (amplitude * Mathf.Sin((bobSpeed * (speed) / 3) * Time.time));
            HoldPos.y += headBob * Time.deltaTime;
            Camera.transform.position = HoldPos;
        }
        else
        {
            amplitude = 0;
            if (Vector3.Distance(Camera.transform.position, headPos) > 0.005f)
                Camera.transform.position = new Vector3(headPos.x, Mathf.Lerp(Camera.transform.position.y, headPos.y, 15.0f * Time.deltaTime), headPos.z);//headPos;
            else
                Camera.transform.position = headPos;
        }

    }

    void ACT_Gravity()
    {
        fallSpeed.y -= Gravity * Time.deltaTime;
        if (fallSpeed.y < maxfallspeed)
            fallSpeed.y = maxfallspeed;

        if (Grounded && fallSpeed.y < 0)
        {
            fallSpeed.y = 0f;
        }

        movement.y = fallSpeed.y;
    }

    void ACT_Buttons()
    {
        float lastdistance = 100f;
        Interact = Physics.OverlapSphere(handPos.transform.position, 2.0f, InteractiveLayer);
        if (Interact.Length != 0)
        {
            InterHold = Interact[0].gameObject;
            float currdistance;
            for (int i = 0; i < Interact.Length; i++)
            {
                currdistance = Vector3.Distance(handPos.transform.position, Interact[i].transform.position);
                Debug.DrawRay(Interact[i].transform.position, (headPos - new Vector3(0.0f, 0.4f, 0.0f)) - Interact[i].transform.position, new Color(255, 255, 255, 1.0f), 5);
                if (Physics.Raycast(Interact[i].transform.position, (headPos - new Vector3(0.0f, 0.4f, 0.0f)) - Interact[i].transform.position, currdistance - 0.2f, Ground, QueryTriggerInteraction.Ignore))
                {
                    InterHold = null;
                }
                else
                if (currdistance < lastdistance)
                {
                    lastdistance = currdistance;
                    InterHold = Interact[i].gameObject;
                }
            }
        }
        else
            InterHold = null;

        if (InterHold != null && Input.GetButtonDown("Interact"))
        {
            InterHold.GetComponent<Object_Interact>().Pressed();
        }

        if (InterHold != null && Input.GetButton("Interact"))
        {
            InterHold.GetComponent<Object_Interact>().Hold();
        }
    }

    void ACT_Running()
    {
        if (!Input.GetButton("Run") && RunningTimer < RunningTimerBase)
        RunningTimer += (Time.deltaTime) * RunMult;
        

        if (isRunning && (InputX != 0 || InputY != 0) && RunningTimer > sprintMin)
        {
            RunningTimer -= (Time.deltaTime) * RunMult;
        }

        if ((RunningTimer < ((RunningTimerBase / 100) * 20)))
        {
            isTired = true;
        }
        if ((RunningTimer > ((RunningTimerBase / 100) * 35)))
        {
            isTired = false;
        }


    }   







    void ACT_Blinking()
    {
        if (IsBlinking() || fakeBlink == true)
            eyes.color = Color.black;
        else
            eyes.color = Color.clear;

        if (Input.GetButton("Blink"))
        {
            CloseTimer = ClosedEyes;
            BlinkingTimer = -1f;
        }

        if (isSmoke && !protectSmoke)
        {
            BlinkMult = 4;
            AsfixTimer -= (Time.deltaTime);
            {
                if (AsfixTimer <= 0.0f)
                {
                    Health -= (Time.deltaTime) * 20;
                }
            }

        }
        else
        {
            AsfixTimer = AsfixiaTimer;
            BlinkMult = 1;
        }


        BlinkingTimer -= (Time.deltaTime) * BlinkMult;
        if (BlinkingTimer <= 0.0f)
        {
            CloseTimer -= Time.deltaTime;
            if (CloseTimer <= 0.0f)
            {
                BlinkingTimer = BlinkingTimerBase;
                CloseTimer = ClosedEyes;
                fakeBlink = false;
            }
        }
    }

    public void Death(int cause)
    {
        if (isGameplay)
        {
            _controller.enabled = false;
            DeathCol.SetActive(true);
            DeathCol.transform.parent = null;
            Camera.transform.parent = DeathCol.transform;
            Camera.GetComponent<Player_MouseLook>().enabled = false;
            isGameplay = false;
            eyes.enabled = false;

            switch (cause)
            {
                case 0:
                    {
                        sfx.PlayOneShot(Deaths[0]);
                        break;
                    }

                case 1:
                    {
                        sfx.PlayOneShot(Conch[Random.Range(0, Conch.Length)]);
                        break;
                    }
                case 2:
                    {
                        sfx.PlayOneShot(Deaths[1]);
                        break;
                    }
            }
        }
    }

    public void FakeBlink(float time)
    {
        BlinkingTimer = -1f;
        CloseTimer = time;
        fakeBlink = true;
    }



    public bool IsBlinking()
    {
        if (BlinkingTimer <= 0.0f && fakeBlink != true)
            return (true);
        else
            return (false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Smoke"))
            isSmoke = true;
        else
            isSmoke = false;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body != null && !body.isKinematic)
            body.velocity += hit.controller.velocity;
    }


    public void ACT_Equip(Equipable_Wear item)
    {
        switch (item.part)
        {
            case bodyPart.Head:
                {
                    equipment[(int)item.part] = item;

                    ItemController.instance.slots[headSlot].isEquip = false;
                    ItemController.instance.slots[headSlot].updateInfo();
                    headSlot = ItemController.instance.currhover;
                    ItemController.instance.slots[headSlot].isEquip = true;
                    break;
                }
            case bodyPart.Body:
                {
                    equipment[(int)item.part] = item;

                    ItemController.instance.slots[bodySlot].isEquip = false;
                    ItemController.instance.slots[bodySlot].updateInfo();
                    bodySlot = ItemController.instance.currhover;
                    ItemController.instance.slots[bodySlot].isEquip = true;
                    break;
                }
            case bodyPart.Any:
                {
                    equipment[(int)item.part] = item;

                    ItemController.instance.slots[anySlot].isEquip = false;
                    ItemController.instance.slots[anySlot].updateInfo();
                    anySlot = ItemController.instance.currhover;
                    ItemController.instance.slots[anySlot].isEquip = true;

                    break;
                }
            case bodyPart.Hand:
                {
                    equipment[(int)item.part] = item;

                    ItemController.instance.slots[handSlot].isEquip = false;
                    ItemController.instance.slots[handSlot].updateInfo();
                    handSlot = ItemController.instance.currhover;
                    ItemController.instance.slots[handSlot].isEquip = true;

                    break;
                }
        }
        if (item.hasEffect)
            SetEffect(item);
        ReloadEquipment();
    }

    public void SetEffect(Item item)
    {
        playerEffects[(int)item.Effects.Affected] = item.Effects.effect;
    }

    void ACT_Effects()
    {
        for(int i = 0; i < playerEffects.Length; i++)
        {
            if (playerEffects[i] != null)
            {
                if (effecTimers[i].Activated == false)
                {
                    effecTimers[i].Timer = playerEffects[i].time;
                    switch ((Ailment)i)
                    {
                        case Ailment.Eyes:
                            {
                                BlinkMult = playerEffects[i].multiplier;
                                break;
                            }
                        case Ailment.Sprint:
                            {
                                sprintMin = playerEffects[i].min;
                                break;
                            }
                    }
                    effecTimers[i].Activated = true;
                }

                if (!playerEffects[i].permanent)
                {
                    effecTimers[i].Timer -= Time.deltaTime;
                    if (effecTimers[i].Timer <= 0)
                        StopEffects((Ailment)i);
                }
            }
        }
    }

    void StopEffects(Ailment what)
    {
        switch (what)
        {
            case Ailment.Eyes:
                {
                    BlinkMult = 1;
                    break;
                }
            case Ailment.Sprint:
                {
                    sprintMin = 0;
                    break;
                }
        }
        effecTimers[(int)what].Activated = false;
        playerEffects[(int)what] = null;
    }



    public void ACT_UnEquip(bodyPart where)
    {
        if (equipment[(int)where].hasEffect)
        {
            StopEffects(equipment[(int)where].Effects.Affected);
        }

        switch (where)
        {
            case bodyPart.Head:
                {
                    ItemController.instance.slots[headSlot].isEquip = false;
                    ItemController.instance.slots[headSlot].updateInfo();
                    break;
                }
            case bodyPart.Body:
                {
                    ItemController.instance.slots[bodySlot].isEquip = false;
                    ItemController.instance.slots[bodySlot].updateInfo();
                    break;
                }
            case bodyPart.Hand:
                {
                    ItemController.instance.slots[handSlot].isEquip = false;
                    ItemController.instance.slots[bodySlot].updateInfo();
                    break;
                }
            case bodyPart.Any:
                {
                    ItemController.instance.slots[anySlot].isEquip = false;
                    ItemController.instance.slots[bodySlot].updateInfo();
                    break;
                }
        }
        equipment[(int)where] = null;
        ReloadEquipment();

    }

    void ReloadEquipment()
    {
        if (equipment[(int)bodyPart.Head] != null)
        {
            protectSmoke = equipment[(int)bodyPart.Head].protectGas;
            Reverb.enabled = equipment[(int)bodyPart.Head].protectGas;
            overlay.sprite = equipment[(int)bodyPart.Head].Overlay;
        }
        else
        {
            Reverb.enabled = false;
            protectSmoke = false;
            overlay.sprite = null;
        }

        if (equipment[(int)bodyPart.Hand] != null)
        {
            handEquip.sprite = equipment[(int)bodyPart.Hand].Overlay;
            handEquip.color = Color.white;
            handEquip.SetNativeSize();
        }
        else
        {
            handEquip.sprite = null;
            handEquip.SetNativeSize();
            handEquip.color = Color.clear;
        }
    }

    public void DropItem(Item item)
    {
        GameObject newObject;
        newObject = Instantiate(GameController.instance.itemSpawner, handPos.transform.position, Quaternion.identity, GameController.instance.itemParent);
        newObject.GetComponent<Object_Item>().item = item;
        newObject.GetComponent<Object_Item>().id = GameController.instance.AddItem(handPos.transform.position, item);
        newObject.GetComponent<Object_Item>().Spawn();
    }

    public void playerWarp(Vector3 here)
    {
        _controller.enabled = false;
        transform.position = here;
        _controller.enabled = true;
    }

}


