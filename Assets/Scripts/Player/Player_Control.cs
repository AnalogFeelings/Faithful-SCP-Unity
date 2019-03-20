using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public enum bodyPart { Head, Body, Hand, Any };

public class Player_Control : MonoBehaviour
{
    float InputX, InputY, BlinkingTimer, BlinkMult = 1, CloseTimer, AsfixTimer, Health = 100, speed, amplitude;
    public GameObject Camera, InterHold, DeathCol;
    private GameObject hand;
    private PostProcessingBehaviour currPost;
    public PostProcessingProfile NormalAmbient;
    private Transform _groundChecker;
    public Transform DefHead, CrouchHead;
    public LayerMask Ground, InteractiveLayer;
    RaycastHit WallCheck;
    Vector3 holdCam, fallSpeed, movement, HoldPos, OriPos, totalmove, headPos;
    private CharacterController _controller;
    public float GroundDistance = 0.2f, bobSpeed, Gravity = -9.81f, maxfallspeed, Basespeed = 3, crouchspeed = 2, runSpeed = 4, BlinkingTimerBase, ClosedEyes, AsfixiaTimer;
    bool Grounded = true, isGameplay = true, isSmoke = false, Crouch = false, fakeBlink, isRunning;
    Camera PlayerCam;
    Image eyes, blinkbar, overlay;
    RectTransform hand_rect, hud_rect;

    public AudioClip[] Conch;
    public AudioSource sfx;

    Collider[] Interact;

    //Iteeemssss
    public string equippedHead = null;
    public string equippedBody = null;
    public string equippedHand = null;
    public string equippedAny = null;

    int headSlot = 0;
    int bodySlot = 0;
    int anySlot = 0;
    int handSlot = 0;

    bool protectSmoke;


    // Start is called before the first frame update
    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _groundChecker = transform.GetChild(0);
        PlayerCam = Camera.GetComponent<Camera>();
        sfx.GetComponent<AudioSource>();
        speed = Basespeed;
        headPos = DefHead.transform.position;
        currPost = Camera.GetComponent<PostProcessingBehaviour>();
        eyes = SCP_UI.instance.eyes;
        blinkbar = SCP_UI.instance.blinkBar;
        hand = SCP_UI.instance.hand;
        overlay = SCP_UI.instance.Overlay;

        hand_rect = hand.GetComponent<RectTransform>();
        hud_rect = SCP_UI.instance.HUD.GetComponent<RectTransform>();



    }

    // Update is called once per frame
    void Update()
    {
        if (isGameplay)
        {
            ACT_Move();
            ACT_Camera();
            ACT_Gravity();
            ACT_Blinking();
            ACT_Buttons();
            ACT_HUD();

            _controller.Move(movement * speed * Time.deltaTime);
        }
        if (Health <= 0)
            Death(0);
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

        isRunning = (Input.GetButton("Run") && !Crouch);



        speed = Basespeed;
        if (Crouch)
            speed = crouchspeed;
        if (isRunning)
            speed = runSpeed;

    }

    void ACT_HUD()
    {
        int Percent = ((int)Mathf.Ceil((BlinkingTimer / (BlinkingTimerBase / 100)) / 5));

        blinkbar.rectTransform.sizeDelta = new Vector2(Percent * 8, 14);

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


    }

    void ACT_Camera()
    {
        holdCam = Camera.transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0.0f, holdCam.y, 0.0f);

        if (Crouch)
        {
            headPos.x = CrouchHead.transform.position.x;
            headPos.z = CrouchHead.transform.position.z;

            if (Vector3.Distance(headPos, CrouchHead.transform.position) > 0.02f)
                headPos.y = Mathf.Lerp(headPos.y, CrouchHead.transform.position.y, 0.2f);
            else
                headPos.y = CrouchHead.transform.position.y;
        }
        else
        {
            headPos.x = DefHead.transform.position.x;
            headPos.z = DefHead.transform.position.z;

            if (Vector3.Distance(headPos, DefHead.transform.position) > 0.02f)
                headPos.y = Mathf.Lerp(headPos.y, DefHead.transform.position.y, 0.2f);
            else
                headPos.y = DefHead.transform.position.y;
        }



        if ((InputX != 0 || InputY != 0))
        {
            amplitude = 0.03f;
            HoldPos = headPos;
            HoldPos.y += amplitude * Mathf.Sin((bobSpeed * (speed) / 3) * Time.time);
            Camera.transform.position = HoldPos;
        }
        else
        {
            amplitude = 0;
            if (Vector3.Distance(Camera.transform.position, headPos) > 0.02f)
                Camera.transform.position = Vector3.Lerp(Camera.transform.position, headPos, 0.1f);
            else
                Camera.transform.position = headPos;
        }

    }

    public void ChangePost(PostProcessingProfile post)
    {
        currPost.profile = post;
    }

    public void DefPost()
    {
        currPost.profile = NormalAmbient;
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
        Interact = Physics.OverlapSphere(transform.position, 2.0f, InteractiveLayer);
        if (Interact.Length != 0)
        {
            InterHold = Interact[0].gameObject;
            float currdistance;
            for (int i = 0; i < Interact.Length; i++)
            {
                currdistance = Vector3.Distance(this.transform.position, Interact[i].transform.position);
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
    }

    void ACT_Blinking()
    {

        eyes.enabled = (IsBlinking() || fakeBlink == true);

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
                        sfx.PlayOneShot(Conch[Random.Range(0, Conch.Length)]);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Smoke"))
            isSmoke = true;
        else
            isSmoke = false;
    }

    public void ACT_Equip(Equipable_Wear item)
    {
        switch (item.part)
        {
            case bodyPart.Head:
                {
                    ItemController.instance.slots[headSlot].isEquip = false;
                    ItemController.instance.slots[headSlot].updateInfo();
                    headSlot = ItemController.instance.currhover;
                    ItemController.instance.slots[headSlot].isEquip = true;

                    protectSmoke = item.protectGas;
                    equippedHead = item.itemName;
                    overlay.sprite = item.Overlay;
                    break;
                }
            case bodyPart.Body:
                {
                    ItemController.instance.slots[bodySlot].isEquip = false;
                    ItemController.instance.slots[bodySlot].updateInfo();
                    bodySlot = ItemController.instance.currhover;
                    ItemController.instance.slots[bodySlot].isEquip = true;

                    equippedBody = item.itemName;
                    break;
                }
            case bodyPart.Any:
                {
                    ItemController.instance.slots[anySlot].isEquip = false;
                    ItemController.instance.slots[anySlot].updateInfo();
                    anySlot = ItemController.instance.currhover;
                    ItemController.instance.slots[anySlot].isEquip = true;

                    equippedAny = item.itemName;
                    break;
                }
            case bodyPart.Hand:
                {
                    ItemController.instance.slots[handSlot].isEquip = false;
                    ItemController.instance.slots[handSlot].updateInfo();
                    handSlot = ItemController.instance.currhover;
                    ItemController.instance.slots[handSlot].isEquip = true;

                    equippedHand = item.itemName;
                    break;
                }
        }

    }

    public void ACT_UnEquip(bodyPart where)
    {

        switch (where)
        {
            case bodyPart.Head:
                {
                    ItemController.instance.slots[headSlot].isEquip = false;
                    ItemController.instance.slots[headSlot].updateInfo();
                    overlay.sprite = null;
                    protectSmoke = false;
                    equippedHead = null;
                    break;
                }
            case bodyPart.Body:
                {
                    ItemController.instance.slots[bodySlot].isEquip = false;
                    ItemController.instance.slots[bodySlot].updateInfo();
                    equippedBody = null;
                    break;
                }
            case bodyPart.Hand:
                {
                    ItemController.instance.slots[handSlot].isEquip = false;
                    ItemController.instance.slots[bodySlot].updateInfo();
                    equippedHand = null;
                    break;
                }
            case bodyPart.Any:
                {
                    ItemController.instance.slots[anySlot].isEquip = false;
                    ItemController.instance.slots[bodySlot].updateInfo();
                    equippedAny = null;
                    break;
                }
        }

    }
}


