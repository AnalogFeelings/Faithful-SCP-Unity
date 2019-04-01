using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SCP_173 : MonoBehaviour
{
    NavMeshAgent _navMeshagent;
    public LayerMask DoorLay;
    public LayerMask CanSeePlayer;
    public float DoorFiddle, DoorCoolDown, TeleCoolDown;
    float DoorTimer, DoorCool, PlayerDistance= 20;
    Object_Door DoorObj;
    public GameObject Player;
    Camera mainCamera;
    Plane[] frustum;
    Collider col_173;
    Collider[] Interact;
    bool canSee = true, canMove = true, hasDoor = false, hasPatrol = false, DidOpen, playedHorror, playedNear, TeleWait;
    public bool canAttack;
    AudioSource sfx;
    public Transform DoorSpot;
    Vector3 Destination;
    int MoveAttempts, TeleAttempts, frameInterval=5;
    public AudioClip[] farHorror, closeHorror;

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

        if (canAttack)
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
                    if (PlayerDistance < 20f)
                        _navMeshagent.speed = 25;
                    else
                        _navMeshagent.speed = 10;
                    _navMeshagent.isStopped = false;
                    sfx.UnPause();
                    if (Time.frameCount % frameInterval == 0)
                        SetDestination();

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
                playedNear = false;
                if (playedHorror == false)
                {
                    GameController.instance.PlayHorror(farHorror[Random.Range(0, farHorror.Length)],transform);
                    playedHorror = true;
                }
        }
    }
    
    void HorrorNear()
    {
        if (PlayerDistance < 6 && CheckPlayer())
        {
                if (playedNear == false)
                {
                    GameController.instance.PlayHorror(closeHorror[Random.Range(0, closeHorror.Length)],transform);
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
            if (hasPatrol == false)
            {
                Debug.Log(Vector3.Distance(transform.position, Destination));
                Destination = GameController.instance.GetPatrol(transform.position);
                    _navMeshagent.SetDestination(Destination);
                hasPatrol = true;
            }
            if (Vector3.Distance(Destination, transform.position) < 5f)
            {
                hasPatrol = false;
            }
            if (_navMeshagent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                hasPatrol = false;
            }

            if (PlayerDistance > 40f)
            {
                if (TeleWait != true)
                {
                    TeleCoolDown = 15f;
                    TeleWait = true;
                }

                if (TeleCoolDown <= 0)
                {
                    TeleWait = false;
                    hasPatrol = false;
                    Vector3 here = GameController.instance.Get173Point();
                    if (here != Vector3.zero && GameController.instance.PlayerNotHere(here))
                        WarpMe(true, here);
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
            if (Physics.Raycast(transform.position, transform.forward, out hit, 5f, DoorLay, QueryTriggerInteraction.Collide))
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
                        WarpMe(true, GameController.instance.GetPatrol(transform.position));
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

    public void WarpMe(bool beActive, Vector3 warppoint)
    {
        _navMeshagent.Warp(warppoint);
        canAttack = beActive;
        playedNear = false;
        playedHorror = false;
        hasDoor = false;
        hasPatrol = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if ((!IsSeen())&&(other.gameObject.CompareTag("Player")))
        {
            other.gameObject.GetComponent<Player_Control>().Death(1);
            canAttack = false;
        }

    }


}
