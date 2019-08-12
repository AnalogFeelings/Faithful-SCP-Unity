using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public enum bodyPart { Head, Body, Hand, Any };
public enum Ailment { Eyes, Sprint, Health};

[System.Serializable]
public class effects
{
    public bool permanent;
    public float time = -1;
    public float max = -1;
    public float min = -1;
    public float value = -1;
    public float multiplier = -1;
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
    float InputX, InputY, BlinkingTimer, BlinkMult = 1, currentBlinkMult = 1, RunMult = 1, CloseTimer, AsfixTimer, speed, lastBob=0, headBob, RunningTimer, OpenTimer=1, lookingForce = 3f, InternalTimer;

    private GameObject hand, CinemaLoaded;
    private Transform _groundChecker;
    
    RaycastHit WallCheck;
    Vector3 holdCam, fallSpeed, movement, HoldPos, OriPos, totalmove, headPos, forceLook;
    Quaternion toAngle;
    private CharacterController _controller;
    bool Grounded = true, isSmoke = false, fakeBlink, isRunning, isTired = false, isLooking=false, cognitoEffect, onBlink, cameraNextFrame;
    Camera PlayerCam;
    Image eyes, blinkbar, runbar, batbar, overlay, handEquip, eyeIcon;
    RectTransform hand_rect, hud_rect;
    public bool Freeze = false, isGameplay = false, Crouch = false, onCam = false, godmode = false;

    [Header("Movement")]
    public float GroundDistance = 0.2f;
    public float Gravity = -9.81f, maxfallspeed, Basespeed = 3, crouchspeed = 2, runSpeed = 4;
    [Header("Camera")]
    public float HurtDivisor = 3;
    public float baseAmplitude, bobSpeed, headBobmult = 20, Camplitude, Cspeed, hamplitude;
    [Header("Gimmicks")]
    public float BlinkingTimerBase;
    public float ClosedEyes, BaseBlinkMult = 0.75f, AsfixiaTimer, RunningTimerBase, OpenMulti, CollisionSphere, Health = 100, bloodloss = 0;
    public LayerMask Ground, InteractiveLayer;
    [Header("Object References")]
    public Transform DefHead;
    public Transform CrouchHead;
    public GameObject CameraObj, InterHold, DeathCol, handPos, CameraContainer, CinemaEffect, SoundPrefab;

    [Header("Audio")]
    public AudioReverbZone Reverb;
    public AudioClip[] Conch, CurrentStep, Deaths, Breath, Concrete, Metal, PD, Forest;
    public AudioSource sfx, va;
    

    Collider[] Interact;

    //Iteeemssss
    [System.NonSerialized]
    public Equipable_Wear[] equipment = new Equipable_Wear[4];
    [System.NonSerialized]
    public effects[] playerEffects = new effects[4];
    [System.NonSerialized]
    public Timers[] effecTimers = new Timers[4];
    int headSlot = 0;
    int bodySlot = 0;
    int anySlot = 0;
    int handSlot = 0;

    int headInv = 0;
    int bodyInv = 0;
    int anyInv = 0;
    int handInv = 0;


    float eyesMin, sprintMin;

    bool protectSmoke;

    //ForcePath Values
    Transform[] Path;
    bool isPath, walkAnim;
    int currentNode;
    Quaternion PathAngle;
    public float NodeDistance;



    //Debug Values
    bool IsNoClip = false;
    bool movementMode = false;


    // Start is called before the first frame update

    void Awake()
    {
        CameraObj = Camera.main.gameObject;
        CameraObj.transform.position = CameraContainer.transform.position;

        handPos.transform.parent = CameraObj.transform;

        CameraObj.GetComponent<Player_MouseLook>().enabled = true;
        _controller = GetComponent<CharacterController>();
        Reverb = GetComponent<AudioReverbZone>();
        _groundChecker = transform.GetChild(0);
        PlayerCam = CameraObj.GetComponent<Camera>();
        speed = Basespeed;
        headPos = DefHead.transform.position;
        eyes = SCP_UI.instance.eyes;
        blinkbar = SCP_UI.instance.blinkBar;
        runbar = SCP_UI.instance.runBar;
        batbar = SCP_UI.instance.navBar;
        hand = SCP_UI.instance.hand;
        overlay = SCP_UI.instance.Overlay;
        handEquip = SCP_UI.instance.handEquip;
        eyeIcon = SCP_UI.instance.eyegraphics;

        hand_rect = hand.GetComponent<RectTransform>();
        hud_rect = SCP_UI.instance.HUD.GetComponent<RectTransform>();

        playerEffects[0] = null;
        playerEffects[1] = null;
        playerEffects[2] = null;
        effecTimers[0] = new Timers();
        effecTimers[1] = new Timers();
        effecTimers[2] = new Timers();
    }

    private void Start()
    {
        CameraObj.transform.rotation = Quaternion.identity;
        if (GlobalValues.isNew == false)
        {
            CameraObj.GetComponent<Player_MouseLook>().rotation = new Vector3(0, SaveSystem.instance.playData.angle, 0);
        }
        handPos.transform.position = CameraObj.transform.position + (CameraObj.transform.forward * 0.5f);

        currentBlinkMult = BaseBlinkMult;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameplay && Time.timeScale == 1f)
        {
            if (IsNoClip == false)
            {
                ACT_Effects();
                ACT_Blinking();
                ACT_Buttons();
                if (!Freeze)
                {
                    ACT_Move();
                    ACT_Gravity();
                    ACT_Running();
                    ACT_Walk();

                    CollisionDetection();
                    if (isPath)
                        ACT_ForceWalk();
                    _controller.Move(movement * Time.deltaTime);
                }
                ACT_Camera();

                if (Health <= 0)
                    Death(0);
            }
            else
            {
                ACT_SimpleMove();
                ACT_NoClipCamera();
                transform.position += movement * Time.deltaTime;
                if (Input.GetKeyDown(KeyCode.Space))
                    movementMode = !movementMode;
            }
        }
    }

    private void LateUpdate()
    {
        if (isGameplay && Time.timeScale == 1f)
        {
            if (!IsNoClip)
            {
                ACT_HUD();
                if (isLooking)
                    ACT_ForceLook();


                if (isTired && !va.isPlaying)
                {
                    va.clip = Breath[Random.Range(0, Breath.Length)];
                    va.Play();
                }
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

        if (Health <= 30)
            Crouch = true;

        isRunning = (Input.GetButton("Run") && !Crouch && RunningTimer > 0.2f && !GameController.instance.isPocket);

        speed = Basespeed;
        if (Crouch)
            speed = crouchspeed;

        if (isRunning)
            speed = runSpeed;

        if (GameController.instance.isPocket)
        {
            speed = crouchspeed+0.5f;
        }

        movement *= speed;
    }

    void ACT_SimpleMove()
    {
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
            speed = runSpeed+2;

        movement *= speed;

    }

    void ACT_Walk()
    {
        if(lastBob > 0 && headBob < 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 2, Ground, QueryTriggerInteraction.Ignore))
            {
                switch (hit.collider.gameObject.tag)
                {
                    case "Metal":
                        {
                            CurrentStep = Metal;
                            break;
                        }
                    case "PD":
                        {
                            CurrentStep = PD;
                            break;
                        }
                    case "Forest":
                        {
                            CurrentStep = Forest;
                            break;
                        }
                    default:
                        {
                            CurrentStep = Concrete;
                            break;
                        }
                }
                sfx.PlayOneShot(CurrentStep[Random.Range(0, CurrentStep.Length)]);

                GameObject soundSpawn = Instantiate(SoundPrefab, transform.position, Quaternion.identity);
                int sound = 1;
                int dur = 2;
                if (Crouch)
                {
                    sound = 0;
                    dur = 4;
                }
                if (isRunning)
                    sound = 2;

                soundSpawn.GetComponent<WorldSound>().SoundLevel = sound;
                soundSpawn.GetComponent<WorldSound>().Timer = dur;


            }
        }
        lastBob = headBob;
    }

    void ACT_HUD()
    {
        if (!cameraNextFrame)
        {
            int blinkPercent = ((int)Mathf.Ceil((BlinkingTimer / (BlinkingTimerBase / 100)) / 5));

            blinkbar.rectTransform.sizeDelta = new Vector2(blinkPercent * 8, 14);

            int runPercent = ((int)Mathf.Floor((RunningTimer / (RunningTimerBase / 100)) / 5));

            runbar.rectTransform.sizeDelta = new Vector2(runPercent * 8, 14);

            if (InterHold != null)
            {
                hand.SetActive(true);
                Vector3 screen = PlayerCam.WorldToScreenPoint(InterHold.transform.position);

                Vector3 heading = InterHold.transform.position - CameraObj.transform.position;
                if (Vector3.Dot(CameraObj.transform.forward, heading) < 0)
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
            {
                if (equipment[(int)bodyPart.Hand] != null)
                {
                    ACT_UnEquip(bodyPart.Hand);
                    return;
                }
                if (equipment[(int)bodyPart.Head] != null)
                {
                    ACT_UnEquip(bodyPart.Head);
                    return;
                }

            }
        }


    }

    public void CognitoHazard(bool state)
    {
        cognitoEffect = state;
        if (cognitoEffect == false)
        {
            PlayerCam.fieldOfView = 60;
        }
    }

    void ACT_Camera()
    {
        holdCam = CameraObj.transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0.0f, holdCam.y, 0.0f);

        if (!cameraNextFrame)
        {
            if (Crouch)
            {
                headPos.x = CrouchHead.transform.position.x;
                headPos.z = CrouchHead.transform.position.z;

                if (Vector3.Distance(headPos, CrouchHead.transform.position) > 0.005f)
                    headPos.y = Mathf.Lerp(headPos.y, CrouchHead.transform.position.y, 15.0f * Time.deltaTime);
                if (Vector3.Distance(headPos, CrouchHead.transform.position) > 2)
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

                if (Vector3.Distance(headPos, DefHead.transform.position) > 2)
                    headPos.y = DefHead.transform.position.y;
            }

            if (cognitoEffect)
            {
                PlayerCam.fieldOfView = 60 + (Camplitude * Mathf.Sin(Cspeed * Time.time));
            }

            if ((((InputX != 0 || InputY != 0)) || walkAnim) && !Freeze)
            {
                InternalTimer += Time.deltaTime;

                headBob = baseAmplitude * Mathf.Sin((speed * bobSpeed) * InternalTimer);
                HoldPos = headPos;
                HoldPos.y = Mathf.Lerp(HoldPos.y, HoldPos.y + headBob, headBobmult);
                if (Vector3.Distance(CameraObj.transform.position, headPos) > 2)
                    CameraObj.transform.position = headPos;
                else
                    CameraObj.transform.position = HoldPos;
                float z = CameraObj.transform.eulerAngles.z;
                z = (z > 180) ? z - 360 : z;

                if (Health < 80)
                {
                    if (!GameController.instance.isPocket)
                        CameraObj.transform.rotation = Quaternion.Euler(CameraObj.transform.eulerAngles.x, CameraObj.transform.eulerAngles.y, Mathf.Lerp(z, 0 + (hamplitude * Mathf.Sin((((101 - Health) / HurtDivisor) * (speed / 2)) * InternalTimer)), 10 * Time.deltaTime));
                    else
                        CameraObj.transform.rotation = Quaternion.Euler(CameraObj.transform.eulerAngles.x, CameraObj.transform.eulerAngles.y, Mathf.Lerp(z, 0 + (hamplitude * Mathf.Sin((((101 - 50) / HurtDivisor) * 0.5f) * InternalTimer)), 10 * Time.deltaTime));
                }
                else
                    if (CameraObj.transform.eulerAngles.z > 0.0001f)
                    CameraObj.transform.rotation = Quaternion.Euler(CameraObj.transform.eulerAngles.x, CameraObj.transform.eulerAngles.y, Mathf.Lerp(z, 0, 10 * Time.deltaTime));


            }
            else
            {
                {
                    if (Vector3.Distance(CameraObj.transform.position, headPos) > 0.005f)
                        CameraObj.transform.position = new Vector3(headPos.x, Mathf.Lerp(CameraObj.transform.position.y, headPos.y, 15.0f * Time.deltaTime), headPos.z);
                    else
                        CameraObj.transform.position = headPos;

                    if (Vector3.Distance(CameraObj.transform.position, headPos) > 2)
                        CameraObj.transform.position = headPos;
                }
            }
        }
        else
        {
            cameraNextFrame = false;

        }

    }

    void ACT_NoClipCamera()
    {
        holdCam = CameraObj.transform.rotation.eulerAngles;
        if (!movementMode)
            transform.rotation = Quaternion.Euler(holdCam.x, holdCam.y, 0.0f);
        else
            transform.rotation = Quaternion.Euler(0, holdCam.y, 0.0f);
        CameraObj.transform.position = transform.position;
    }


    void ACT_Gravity()
    {
        fallSpeed.y -= Gravity * Time.deltaTime;
        if (fallSpeed.y < maxfallspeed)
            fallSpeed.y = maxfallspeed;

        if (Grounded)
        {
            fallSpeed.y = -_controller.stepOffset / Time.deltaTime;
        }

        movement.y = fallSpeed.y;
    }


    public void ForceLook(Vector3 point, float Force)
    {
        forceLook = point;
        lookingForce = Force;
        isLooking = true;

    }

    public void StopLook()
    {
        isLooking = false;
        CameraObj.GetComponent<Player_MouseLook>().addedRota = Quaternion.identity;
    }

    void ACT_ForceLook()
    {
            CameraObj.GetComponent<Player_MouseLook>().addedRota = Quaternion.Slerp(CameraObj.transform.rotation, toAngle, lookingForce * Time.deltaTime);
            Vector3 Point = new Vector3(forceLook.x, forceLook.y, forceLook.z) - CameraObj.transform.position;

            toAngle = Quaternion.LookRotation(Point);
    }

   

    void ACT_ForceWalk()
    {
        
        if (currentNode > 1 && Vector3.Distance(new Vector3(Path[currentNode-2].position.x, transform.position.y, Path[currentNode-2].position.z), transform.position) < NodeDistance)
        {
                currentNode -= 1;
        }

        Vector3 Point = new Vector3(Path[currentNode].position.x, transform.position.y, Path[currentNode].position.z) - transform.position;
        movement += (Point.normalized * 1.6f);

        if (Vector3.Distance(new Vector3(Path[currentNode].position.x, transform.position.y, Path[currentNode].position.z), transform.position) < NodeDistance)
        {
            if (currentNode != Path.Length - 1)
            {
                currentNode += 1;
                walkAnim = true;
            }
            else
            {
                walkAnim = false;
                movement -= (Point.normalized * 1.6f);
            }

        }
    }

    public void StopWalk()
    {
        isPath = false;
        walkAnim = false;
    }

    public void ForceWalk(Transform [] newPath)
    {
        currentNode = 0;
        Path = newPath;
        isPath = true;
    }

    void ACT_Buttons()
    {
        if (!Freeze)
        {
            float lastdistance = 100f;
            Interact = Physics.OverlapSphere(handPos.transform.position, 2.0f, InteractiveLayer);
            if (Interact.Length != 0)
            {
                InterHold = null;
                float currdistance;
                for (int i = 0; i < Interact.Length; i++)
                {
                    currdistance = Vector3.Distance(handPos.transform.position, Interact[i].transform.position);
                    Debug.DrawRay(Interact[i].transform.position, (headPos - new Vector3(0.0f, 0.4f, 0.0f)) - Interact[i].transform.position, new Color(255, 255, 255, 1.0f), 5);
                    if (currdistance < lastdistance)
                    {
                        if (!Physics.Raycast(Interact[i].transform.position, (headPos - new Vector3(0.0f, 0.4f, 0.0f)) - Interact[i].transform.position, currdistance, Ground, QueryTriggerInteraction.Ignore))
                        {
                            lastdistance = currdistance;
                            InterHold = Interact[i].gameObject;
                        }
                    }
                }
            }
            else
                InterHold = null;
        }

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
        if (onBlink == false)
            eyes.color = new Color(255, 255, 255, Mathf.Clamp(-((BlinkingTimer-0.25f)*4), 0.0f, 1.0f));
        else
        {
            OpenTimer -= Time.deltaTime * OpenMulti;
            if (OpenTimer < 0)
                onBlink = false;
            eyes.color = new Color(255, 255, 255, OpenTimer);
        }

        if (Input.GetButton("Blink"))
        {
            Debug.Log("blinking");
            CloseTimer = ClosedEyes;
            BlinkingTimer = -2f;
        }

        if (isSmoke && !protectSmoke)
        {
            eyeIcon.color = Color.red;
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
            eyeIcon.color = Color.white;
            AsfixTimer = AsfixiaTimer;
            BlinkMult = currentBlinkMult;
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
                OpenTimer = 1;
                onBlink = true;
            }
        }
    }

    public void Death(int cause)
    {
        if (isGameplay && !godmode)
        {
            GameController.instance.PlayerDeath();
            _controller.enabled = false;
            DeathCol.SetActive(true);
            CameraObj.transform.parent = DeathCol.transform;
            CameraObj.GetComponent<Player_MouseLook>().enabled = false;
            isGameplay = false;
            eyes.color = Color.clear;
            Destroy(handPos);

            overlay.sprite = null;
            handEquip.color = Color.clear;
            SCP_UI.instance.SNav.SetActive(false);
            SCP_UI.instance.radio.StopRadio();

            switch (cause)
            {
                case 3:
                    {
                        sfx.PlayOneShot(Deaths[cause]);
                        GameController.instance.deathmsg = GlobalValues.deathStrings["death_106_stone"];
                        break;
                    }
                case 1:
                    {
                        sfx.PlayOneShot(Conch[Random.Range(0, Conch.Length)]);
                        break;
                    }
                default:
                    {
                        sfx.PlayOneShot(Deaths[cause]);
                        break;
                    }
            }
        }
    }

    public void FakeDeath(int cause)
    {
        if (isGameplay && !godmode)
        {

            GameController.instance.FakeDeath();
            _controller.enabled = false;
            DeathCol.SetActive(true);
            CameraObj.transform.parent = DeathCol.transform;
            CameraObj.GetComponent<Player_MouseLook>().enabled = false;
            isGameplay = false;
            eyes.color = Color.clear;
            Destroy(handPos);

            overlay.sprite = null;
            handEquip.color = Color.clear;
            SCP_UI.instance.SNav.SetActive(false);

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
        if (other.gameObject.CompareTag("Camera"))
            onCam = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Smoke"))
            isSmoke = false;
        if (other.gameObject.CompareTag("Camera"))
            onCam = false;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Death"))
            Death(0);
        if (hit.gameObject.CompareTag("DeathFall"))
            Death(3);

        Rigidbody body = hit.collider.attachedRigidbody;
        if (body != null && !body.isKinematic)
            body.velocity += hit.controller.velocity;
    }

    void CollisionDetection()
    {
        Collider[] collisions = Physics.OverlapSphere(transform.position, CollisionSphere, Ground, QueryTriggerInteraction.Ignore);

        if (collisions.Length != 0)
        {
            for (int i = 0; i < collisions.Length; i++)
            {
                if (collisions[i].gameObject.CompareTag("Death"))
                    Death(0);
                if (collisions[i].gameObject.CompareTag("DeathFall"))
                    Death(3);


            }
        }
    }


    public void ACT_Equip(Equipable_Wear item)
    {
        if (equipment[(int)item.part] is Equipable_Nav)
        {
            SCP_UI.instance.SNav.SetActive(false);
        }

        if (equipment[(int)item.part] is Equipable_Radio)
        {
            SCP_UI.instance.radio.StopRadio();
        }

        switch (item.part)
        {
            case bodyPart.Head:
                {
                    equipment[(int)item.part] = item;

                    if (item.isFem)
                        SubtitleEngine.instance.playSub(string.Format(GlobalValues.playStrings["play_equip_fem"], GlobalValues.itemStrings[item.itemName]));
                    else
                        SubtitleEngine.instance.playSub(string.Format(GlobalValues.playStrings["play_equip_male"], GlobalValues.itemStrings[item.itemName]));

                    ItemController.instance.equip[headInv][headSlot] = false;
                    headSlot = ItemController.instance.currhover;
                    headInv = ItemController.instance.currInv;
                    ItemController.instance.equip[headInv][headSlot] = true;
                    break;
                }
            case bodyPart.Body:
                {
                    equipment[(int)item.part] = item;

                    if (item.isFem)
                        SubtitleEngine.instance.playSub(string.Format(GlobalValues.playStrings["play_equip_fem"], GlobalValues.itemStrings[item.itemName]));
                    else
                        SubtitleEngine.instance.playSub(string.Format(GlobalValues.playStrings["play_equip_male"], GlobalValues.itemStrings[item.itemName]));


                    ItemController.instance.equip[bodyInv][bodySlot] = false;
                    bodySlot = ItemController.instance.currhover;
                    bodyInv = ItemController.instance.currInv;
                    ItemController.instance.equip[bodyInv][bodySlot] = true;



                    break;
                }
            case bodyPart.Any:
                {
                    equipment[(int)item.part] = item;

                    if (item.isFem)
                        SubtitleEngine.instance.playSub(string.Format(GlobalValues.playStrings["play_equip_fem"], GlobalValues.itemStrings[item.itemName]));
                    else
                        SubtitleEngine.instance.playSub(string.Format(GlobalValues.playStrings["play_equip_male"], GlobalValues.itemStrings[item.itemName]));

                    ItemController.instance.equip[anyInv][anySlot] = false;
                    anySlot = ItemController.instance.currhover;
                    anyInv = ItemController.instance.currInv;
                    ItemController.instance.equip[anyInv][anySlot] = true;
                    break;
                }
            case bodyPart.Hand:
                {
                    equipment[(int)item.part] = item;

                    ItemController.instance.equip[handInv][handSlot] = false;
                    handSlot = ItemController.instance.currhover;
                    handInv = ItemController.instance.currInv;
                    ItemController.instance.equip[handInv][handSlot] = true;

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
        Health -= (bloodloss * 0.125f) * Time.deltaTime;
        if (Health >= 100)
            Health = 100;



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
                                if (playerEffects[i].multiplier != -1)
                                    currentBlinkMult = playerEffects[i].multiplier;
                                Debug.Log("Blink mult is now " + BlinkMult);
                                if (playerEffects[i].value != -1)
                                    BlinkingTimer = playerEffects[i].value;
                                break;
                            }
                        case Ailment.Sprint:
                            {
                                if (playerEffects[i].min != -1)
                                    sprintMin = playerEffects[i].min;
                                break;
                            }
                        case Ailment.Health:
                            {
                                if (playerEffects[i].value != -1)
                                    Health += playerEffects[i].value;
                                if (playerEffects[i].multiplier != -1)
                                {
                                    if (bloodloss == 1)
                                        SubtitleEngine.instance.playSub(GlobalValues.playStrings["play_cureblood"]);
                                    if (bloodloss > 1)
                                        SubtitleEngine.instance.playSub(GlobalValues.playStrings["play_cureblood2"]);
                                    bloodloss -= playerEffects[i].multiplier;
                                    
                                }
                                else
                                    SubtitleEngine.instance.playSub(GlobalValues.playStrings["play_cure"]);


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

        if (equipment[(int)bodyPart.Hand] is Equipable_Elec && equipment[(int)bodyPart.Hand].valueFloat >= 0)
        {
            (equipment[(int)bodyPart.Hand]).valueFloat -= ((Equipable_Elec)equipment[(int)bodyPart.Hand]).SpendFactor * Time.deltaTime;
        }


    }

    void StopEffects(Ailment what)
    {
        switch (what)
        {
            case Ailment.Eyes:
                {
                    currentBlinkMult = BaseBlinkMult;
                    Debug.Log("Stopping effects");
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
        SCP_UI.instance.ItemSFX(equipment[(int)where].SFX);

        if (equipment[(int)where].hasEffect)
        {
            StopEffects(equipment[(int)where].Effects.Affected);
        }

        if (equipment[(int)where] is Equipable_Nav)
        {
            SCP_UI.instance.SNav.SetActive(false);
        }

        if (equipment[(int)where] is Equipable_Radio)
        {
            SCP_UI.instance.radio.StopRadio();
        }

        if (where != bodyPart.Hand)
        {
            if (equipment[(int)where].isFem)
                SubtitleEngine.instance.playSub(string.Format(GlobalValues.playStrings["play_dequip_fem"], GlobalValues.itemStrings[equipment[(int)where].itemName]));
            else
                SubtitleEngine.instance.playSub(string.Format(GlobalValues.playStrings["play_dequip_male"], GlobalValues.itemStrings[equipment[(int)where].itemName]));
        }
        switch (where)
        {
            case bodyPart.Head:
                {
                    ItemController.instance.equip[headInv][headSlot] = false;
                    break;
                }
            case bodyPart.Body:
                {
                    ItemController.instance.equip[bodyInv][bodySlot] = false;
                    break;
                }
            case bodyPart.Hand:
                {
                    ItemController.instance.equip[handInv][handSlot] = false;
                    break;
                }
            case bodyPart.Any:
                {
                    ItemController.instance.equip[anyInv][anySlot] = false;
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
            overlay.color = new Color(255, 255, 255, 0.75f);
        }
        else
        {
            Reverb.enabled = false;
            protectSmoke = false;
            overlay.sprite = null;
        }

        if (equipment[(int)bodyPart.Hand] != null && !((equipment[(int)bodyPart.Hand] is Equipable_Radio) || (equipment[(int)bodyPart.Hand] is Equipable_Nav)))
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
        newObject = Instantiate(GameController.instance.itemSpawner, handPos.transform.position, Quaternion.identity, GameController.instance.itemParent.transform);
        newObject.GetComponent<Object_Item>().item = item;
        newObject.GetComponent<Object_Item>().isNew = false;
        newObject.GetComponent<Object_Item>().id = GameController.instance.AddItem(handPos.transform.position, item);
        Debug.Log("dROPPED " + newObject.GetComponent<Object_Item>().id);
        newObject.GetComponent<Object_Item>().Spawn();
        newObject.GetComponent<Rigidbody>().AddForce(transform.forward * 5f);
    }

    public void playerWarp(Vector3 here, float rotation)
    {
        _controller.enabled = false;
        transform.position = here;
        Debug.Log("body pos " + transform.position + " head pos " + CrouchHead.transform.position);
        if (Crouch)
            CameraObj.transform.position = CrouchHead.transform.position;
        else
            CameraObj.transform.position = DefHead.transform.position;
        cameraNextFrame = true;
        Vector3 rota = CameraObj.GetComponent<Player_MouseLook>().rotation;
        CameraObj.GetComponent<Player_MouseLook>().rotation = new Vector3(rota.x, rota.y + rotation, 0);
        
        
        _controller.enabled = true;
    }


    public void SwitchNoClip()
    {
        if (IsNoClip == false)
        {
            IsNoClip = true;
            _controller.enabled = false;
            SCP_UI.instance.HUD.enabled = false;
            CinemaLoaded = Instantiate(CinemaEffect);
        }
        else
        {
            IsNoClip = false;
            _controller.enabled = true;
            SCP_UI.instance.HUD.enabled = true;
            DestroyImmediate(CinemaLoaded);
        }
    }


}


