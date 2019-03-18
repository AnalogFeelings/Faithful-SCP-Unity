using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public class Player_Control : MonoBehaviour
{
    float InputX, InputY, BlinkingTimer, BlinkMult = 1, CloseTimer, AsfixTimer, Health = 100, speed, amplitude;
    public GameObject Camera, InterHold, DeathCol;
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
    public Canvas HUD;
    public Image eyes;

    public AudioClip [] Conch;
    public AudioSource sfx;

    Collider[] Interact;


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

        if (Input.GetButtonDown("Crouch")&& !isRunning)
            Crouch = !Crouch;

        isRunning = (Input.GetButton("Run") && !Crouch);



        speed = Basespeed;
        if (Crouch)
            speed = crouchspeed;
        if (isRunning)
            speed = runSpeed;

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







        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
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
            for (int i = 0; i < Interact.Length; i++)
            {
                Debug.DrawRay(Interact[i].transform.position, (headPos - new Vector3(0.0f, 0.4f, 0.0f)) - Interact[i].transform.position, new Color(255, 255, 255, 1.0f), 5);
                if (Physics.Raycast(Interact[i].transform.position, (headPos - new Vector3(0.0f, 0.4f, 0.0f)) - Interact[i].transform.position, out WallCheck, 4.0f, Ground, QueryTriggerInteraction.Ignore))
                {
                    if (WallCheck.transform == this.transform && (Vector3.Distance(this.transform.position, Interact[i].transform.position) < lastdistance))
                    {
                        lastdistance = Vector3.Distance(this.transform.position, Interact[i].transform.position);
                        InterHold = Interact[i].gameObject;
                    }
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

        eyes.enabled = (IsBlinking()||fakeBlink==true);

        if (Input.GetButton("Blink"))
        {
            CloseTimer = ClosedEyes;
            BlinkingTimer = -1f;
        }

        if (isSmoke)
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


        BlinkingTimer -= (Time.deltaTime)*BlinkMult;
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
        if (BlinkingTimer <= 0.0f && fakeBlink!=true)
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








    /* void ButtonOverlay()
     {
         Vector3 offsetPos = InterHold.transform.position;
         // Calculate *screen* position (note, not a canvas/recttransform position)
         Vector2 canvasPos;
         Vector2 screenPoint = PlayerCam.WorldToScreenPoint(offsetPos);
         // Convert screen position to Canvas / RectTransform space <- leave camera null if Screen Space Overlay
         RectTransformUtility.ScreenPointToLocalPointInRectangle(HUD.pixelRect, screenPoint, null, out canvasPos);

     }*/

}


