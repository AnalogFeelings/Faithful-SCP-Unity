using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SCP_173 : MonoBehaviour
{
    NavMeshAgent _navMeshagent;
    public LayerMask DoorLay;
    public LayerMask CanSeePlayer;
    public float DoorFiddle, DoorCoolDown;
    float DoorTimer, DoorCool, PlayerDistance= 20;
    Object_Door DoorObj;
    public GameObject Player;
    Camera mainCamera;
    Plane[] frustum;
    Collider col_173;
    Collider[] Interact;
    bool canSee = true, canMove = true, hasDoor = false, hasPatrol = false, DidOpen, playedHorror, playedNear;
    public bool canAttack;
    AudioSource sfx;
    public Transform DoorSpot;
    Vector3 Destination;
    int MoveAttempts, frameInterval=5;
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
            if (!canSee)
            {
                DoorTimer -= Time.deltaTime;
                if (canMove)
                {
                    if (Time.frameCount % frameInterval == 0)
                        SetDestination();

                    if (PlayerDistance < 20f)
                        _navMeshagent.speed = 25;
                    else
                        _navMeshagent.speed = 10;
                    _navMeshagent.isStopped = false;
                    sfx.UnPause();
                    HorrorNear();

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
                HorrorFar();
                
                
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
                    GameController.instance.PlayHorror(farHorror[Random.Range(0, farHorror.Length)]);
                    playedHorror = true;
                }
        }
    }
    
    void HorrorNear()
    {
        if (PlayerDistance < 4 && CheckPlayer())
        {
                if (playedNear == false)
                {
                    GameController.instance.PlayHorror(closeHorror[Random.Range(0, closeHorror.Length)]);
                    playedNear = true;
                }
        }
    }



    bool CheckPlayer()
    {
        RaycastHit WallCheck;
        Debug.DrawRay(Player.transform.position, (transform.position + new Vector3(0, 0.4f, 0)) - Player.transform.position);
        
        if (Time.frameCount % frameInterval == 0)
        {
            if (Physics.Raycast(Player.transform.position, (transform.position + new Vector3(0, 0.4f,0))- Player.transform.position, out WallCheck, 40.0f))
            {
                if (WallCheck.transform == this.transform)
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
           Interact = Interact = Physics.OverlapSphere(DoorSpot.position, 1.0f, DoorLay);
            if (Interact.Length != 0)
            {
                Debug.DrawRay(transform.position, Interact[0].transform.position - transform.position);
                DoorObj = Interact[0].transform.gameObject.GetComponent<Object_Door>();
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
    }

    private void OnTriggerStay(Collider other)
    {
        if ((!IsSeen())&&(other.gameObject.CompareTag("Player")))
        {
            other.gameObject.GetComponent<Player_Control>().Death(0);
            Debug.Log("You are ded ded ded");
        }

    }


}
