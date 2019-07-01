using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SCP_173 : Roam_NPC
{
    NavMeshAgent _navMeshagent;
    public LayerMask DoorLay;
    public LayerMask CanSeePlayer;
    public float DoorFiddle, DoorCoolDown, TeleCoolDown;
    float DoorTimer, DoorCool, PlayerDistance= 20, DoorDistance;
    Object_Door DoorObj;
    public GameObject Player;
    Camera mainCamera;
    Plane[] frustum;
    Collider col_173;
    Collider[] Interact;
    bool canSee = true, canMove = true, hasDoor = false, hasPatrol = false, DidOpen, playedHorror, playedNear, TeleWait, blinkFrame, closeDoor = false;
    AudioSource sfx;
    public Transform DoorSpot;
    Vector3 Destination;
    int MoveAttempts, TeleAttempts, frameInterval=15;
    public AudioClip[] farHorror, closeHorror;
    public AudioClip teleport;

    // Use this for initialization
    void Awake()
    {
        mainCamera = Camera.main;
        col_173 = GetComponent<Collider>();
        Player = GameController.instance.player;
        Destination = transform.position;


        _navMeshagent = this.GetComponent<NavMeshAgent>();
        sfx = GetComponent<AudioSource>();
        _navMeshagent.updateRotation = false;
    }

    void Start()
    {
        _navMeshagent.Warp(transform.position);
        sfx.Pause();
    }

    void Update()
    {
        if (Time.frameCount % frameInterval == 0)
            PlayerDistance = (Vector3.Distance(Player.transform.position, transform.position));

        if (PlayerDistance > 20)
        {
            playedHorror = false;
            playedNear = false;
            }

        canSee = IsSeen();

        CheckDoor();

        DoorCool -= Time.deltaTime;

        if (isActive)
        {
            HorrorFar();

            if (TeleWait)
                TeleCoolDown -= Time.deltaTime;

            if (!canSee)
            {
                HorrorNear();

                if (_navMeshagent.velocity.sqrMagnitude > Mathf.Epsilon)
                {
                    transform.rotation = Quaternion.LookRotation(_navMeshagent.velocity.normalized);
                }


                DoorTimer -= Time.deltaTime;
                if (canMove)
                {
                    if (PlayerDistance < 20f && !closeDoor)
                        _navMeshagent.speed = 18;
                    if (PlayerDistance >= 20f && !closeDoor)
                        _navMeshagent.speed = 10;

                    if (closeDoor)
                    {
                        _navMeshagent.speed = 5;
                    }

                    

                    _navMeshagent.isStopped = false;
                    sfx.UnPause();
                    if (Time.frameCount % frameInterval == 0 || blinkFrame == false)
                    {
                        SetDestination();
                        blinkFrame = true;
                    }

                }
                else
                {
                    _navMeshagent.speed = 0;
                    _navMeshagent.isStopped = true;
                    sfx.Pause();
                }
            }
            else
            {
                blinkFrame = false;
                _navMeshagent.speed = 0;
                _navMeshagent.isStopped = true;
                sfx.Pause();
            }
        }
        else
            sfx.Pause();
    }


    void HorrorFar()
    {
        if (PlayerDistance < 16 && CheckPlayer())
        {
                
                if (playedHorror == false)
                {
                    GameController.instance.PlayHorror(farHorror[Random.Range(0, farHorror.Length)],transform, npc.scp173);
                    playedHorror = true;
                    playedNear = false;
                }
        }
    }
    
    void HorrorNear()
    {
        if (PlayerDistance < 4 && CheckPlayer())
        {
                if (playedNear == false)
                {
                    GameController.instance.PlayHorror(closeHorror[Random.Range(0, closeHorror.Length)],transform, npc.scp173);
                    playedNear = true;
                }
        }
    }



    bool CheckPlayer()
    {
        Debug.DrawRay(Player.transform.position, (transform.position + new Vector3(0, 0.4f, 0)) - Player.transform.position);
        
        if (Time.frameCount % frameInterval == 0)
        {
            if (!Physics.Raycast(Player.transform.position, (transform.position + new Vector3(0, 0.4f,0))- Player.transform.position, PlayerDistance, CanSeePlayer))
            {
                    return true;
            }
        }
        return false;
    }







    private void SetDestination()
    {
        if (PlayerDistance < 30f)
        {
            Destination = Player.transform.position;
                _navMeshagent.SetDestination(Destination);
        }
        else
            {
            if (hasPatrol == false && agroLevel != 0)
            {
                Destination = GameController.instance.GetPatrol(transform.position, 4, 0);
                    _navMeshagent.SetDestination(Destination);
                hasPatrol = true;
            }
            if (Vector3.Distance(Destination, transform.position) < 3f)
            {
                hasPatrol = false;
            }
            if (_navMeshagent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                hasPatrol = false;
            }

            if (PlayerDistance > 35f )
            {
                if (TeleWait != true)
                {
                    if (agroLevel == 1)
                    TeleCoolDown = 6f;
                    if (agroLevel == 0)
                        TeleCoolDown = 15f;
                    TeleWait = true;
                }

                if (TeleCoolDown <= 0)
                {
                    TeleWait = false;
                    hasPatrol = false;

                    if (agroLevel == 1)
                    {
                        Vector3 here;
                        do
                        {
                            here = GameController.instance.GetPatrol(Player.transform.position, 4, 2);

                        }
                        while (!GameController.instance.PlayerNotHere(here));
                        Spawn(true, here);
                    }
                    if (agroLevel == 0)
                    {
                        Vector3 here;
                        do
                        {
                            here = GameController.instance.GetPatrol(Player.transform.position, 6, 2);

                        }
                        while (!GameController.instance.PlayerNotHere(here));
                        Spawn(true, here);
                    }
                }
                
            }

        }
    }

    private bool IsSeen()
    {
        frustum = GeometryUtility.CalculateFrustumPlanes(mainCamera);

        if (Player.GetComponent<Player_Control>().IsBlinking()==true)
            return (false);
        else
            return (GeometryUtility.TestPlanesAABB(frustum, col_173.bounds));

    }

    void CheckDoor()
    {
        if (hasDoor == false)
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.forward);
            if (Physics.Raycast(transform.position, transform.forward, out hit, 6f, DoorLay, QueryTriggerInteraction.Collide))
            {
                if (PlayerDistance < hit.distance)
                    closeDoor = false;
                else
                {
                    DoorObj = hit.transform.gameObject.GetComponent<Object_Door>();
                    closeDoor = !DoorObj.GetState();
                }
            }
            else
                closeDoor = false;

            if (Physics.Raycast(transform.position, transform.forward, out hit, 1.5f, DoorLay, QueryTriggerInteraction.Collide))
            {
                DoorObj = hit.transform.gameObject.GetComponent<Object_Door>();
                DoorTimer = DoorFiddle;
                hasDoor = true;
            }
            else
            {
                canMove = true;
                hasDoor = false;
            }
        }
        else
        {
            if (DoorObj.GetState())
            {
                canMove = true;
                hasDoor = false;
            }
            else
            {
                canMove = false;
                if (DoorTimer <= 0)
                {
                    DidOpen = DoorObj.Door173();
                    DoorCool = DoorCoolDown;
                    Debug.Log(MoveAttempts);
                    if (MoveAttempts >= 5)
                    {
                        hasDoor = false;
                        canMove = true;
                        Vector3 here;
                        do
                        {
                            here = GameController.instance.GetPatrol(Player.transform.position, 2, 1);

                        }
                        while (!GameController.instance.PlayerNotHere(here));
                        Spawn(true, here);
                        MoveAttempts = 0;
                    }
                    if (DidOpen == false)
                    {
                        SetDestination();
                        MoveAttempts += 1;
                        hasDoor = false;
                    }
                }
            }
        }


    }

    public override void Spawn(bool beActive, Vector3 warppoint)
    {
        NavMeshHit here;

        if (NavMesh.SamplePosition(warppoint, out here, 0.2f, NavMesh.AllAreas))
        {
            _navMeshagent.Warp(warppoint);
            isActive = beActive;
            playedNear = false;
            playedHorror = false;
            hasDoor = false;
            hasPatrol = false;
        }
        else if (NavMesh.SamplePosition(warppoint, out here, 10f, NavMesh.AllAreas))
        {
            _navMeshagent.Warp(here.position);
            isActive = beActive;
            playedNear = false;
            playedHorror = false;
            hasDoor = false;
            hasPatrol = false;
        }

        if (isActive)
        {
            GameController.instance.GlobalSFX.PlayOneShot(teleport);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if ((!IsSeen())&&(other.gameObject.CompareTag("Player")))
        {
            other.gameObject.GetComponent<Player_Control>().Death(1);
            isActive = false;
        }
    }


}
