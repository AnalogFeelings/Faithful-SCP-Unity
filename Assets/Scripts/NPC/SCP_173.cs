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
    float DoorTimer, DoorCool;
    Object_Door DoorObj;
    public GameObject Player;
    Camera mainCamera;
    Plane[] frustum;
    Collider col_173;
    Collider[] Interact;
    bool canSee = true, canMove = true, hasDoor = false, hasPatrol = false, DidOpen;
    public bool canAttack;
    AudioSource sfx;
    public Transform DoorSpot;
    Vector3 Destination;
    int MoveAttempts;

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
                    SetDestination();
                    _navMeshagent.speed = 15;
                    _navMeshagent.isStopped = false;
                    sfx.UnPause();

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


    private void SetDestination()
    {
        if (Vector3.Distance(Player.transform.position, transform.position) < 30f)
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
    }


}
