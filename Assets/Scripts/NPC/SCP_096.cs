using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class SCP_096 : Roam_NPC
{
    enum scp096State { idle, sitting, patrol, panic, run, attack }

    public AudioClip idle, panic, horror, chaseClip, scream;
    Camera mainCamera;
    Plane[] frustum;
    NavMeshAgent agent;
    AudioSource screamSource;
    public Animator animator;
    public BoxCollider faceCol;
    public float viewLimit, cryTimer, teleportTimer, normalSpeed, panicSpeed, accel, maxFallSpeed, gravity, normalTurn=1f, runTurn;
    public LayerMask ground, doors, playerMask;
    public bool debugIsTargeting;
    scp096State state, currAnim;
    float Timer, fallSpeed, framerate = 15, framerate2 = 60, speed, turnSpeed;
    int currNode;
    bool hasPath, isRota;
    Vector3 currTarget, movement, tempMovement;
    NavMeshPath currPath;
    Quaternion toRota,fromAngle;
    static int valIsPanic = 0, valIsChase = 1, valTimer = 2;

    // Start is called before the first frame update
    void Start()
    {
        state = scp096State.idle;
        currAnim = scp096State.idle;
        mainCamera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        screamSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (data.isActive)
        {
            if (isEvent)
                NPCEvent();
            else
                NPCUpdate();
        }
    }

    public override void setData(NPC_Data state)
    {
        agent.Warp(state.Pos.toVector3());
        base.setData(state);
    }

    private void LateUpdate()
    {
        if (currAnim != state)
        {
            if (state != scp096State.panic && state != scp096State.attack && state != scp096State.run && (currAnim == scp096State.attack || currAnim == scp096State.panic || currAnim == scp096State.run))
            {
                Debug.Log("currANim = " + currAnim + " new state = " + state);
                animator.SetBool("panic", false);
            }
                

            switch (state)
            {
                case scp096State.idle:
                    {
                        animator.SetTrigger("toIdle");
                        break;
                    }
                case scp096State.sitting:
                    {
                        if (currAnim != scp096State.attack)
                        animator.SetTrigger("toSit");
                        break;
                    }
                case scp096State.patrol:
                    {
                        animator.SetTrigger("toWalk");
                        break;
                    }
                case scp096State.panic:
                    {
                        animator.SetBool("panic", true);
                        break;
                    }
                case scp096State.attack:
                    {
                        Debug.Log("Doing to attack");
                        animator.SetTrigger("toAttack");
                        break;
                    }
                case scp096State.run:
                    {
                        animator.SetTrigger("toWalk");
                        break;
                    }
            }
            currAnim = state;
        }
    }

    private void OnDrawGizmos()
    {
        if (currTarget != null)
        Gizmos.DrawSphere(currTarget, 0.3f);
    }

    void NPCUpdate()
    {
        //Current State
        switch(state)
        {
            case scp096State.panic:
                {
                    agent.ResetPath();
                    break;
                }
            case scp096State.idle:
                {
                    agent.ResetPath();
                    break;
                }
            case scp096State.patrol:
                {
                    agent.speed = normalSpeed;
                    if (!agent.hasPath||agent.pathStatus==NavMeshPathStatus.PathInvalid)
                        agent.SetDestination(getRandomPoint());

                    break;
                }
            case scp096State.run:
                {
                    agent.speed = panicSpeed;

                    if (Time.frameCount % framerate == 0)
                    {

                        agent.SetDestination(GameController.instance.playercache.transform.position);
                    }

                    RaycastHit hit;
                    if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 1f, doors))
                    {
                        hit.transform.gameObject.GetComponent<Object_Door>().ForceOpen(5);
                    }

                    if (Physics.OverlapSphere(transform.position + transform.forward, 0.5f, playerMask).Length > 0)
                    {
                        GameController.instance.playercache.Death(4);
                        state = scp096State.attack;
                    }

                    break;
                }
        }

        if (debugIsTargeting && Time.frameCount % framerate == 0)
        {
            if ((state != scp096State.attack && state != scp096State.panic && state != scp096State.run) && OnView())
            {
                data.npcvalue[valIsPanic] = 1;
                state = scp096State.panic;
                screamSource.Stop();
                GameController.instance.ChangeMusic(panic);
                GameController.instance.PlayHorror(horror, this.transform, npc.none);
                GameController.instance.playercache.CognitoHazard(true);
                Timer = cryTimer;
            }
        }

        Timer -= Time.deltaTime;

        //Next State
        if (Timer < 0)
        {
            switch(state)
            {
                case scp096State.idle:
                    {
                        int chance = Random.Range(0, 100);
                        if (chance < 25)
                        {
                            state = scp096State.sitting;
                            Timer = Random.Range(10, 30);
                        }
                        else
                        {
                            state = scp096State.patrol;
                            Timer = Random.Range(10, 15);
                            getRandomPoint();
                        }
                        break;
                    }
                case scp096State.patrol:
                case scp096State.sitting:
                    {
                        state = scp096State.idle;
                        Timer = Random.Range(5, 10);
                        break;
                    }
                case scp096State.panic:
                    {
                        state = scp096State.run;
                        MusicPlayer.instance.ChangeMusic(chaseClip);
                        screamSource.Stop();
                        screamSource.clip = scream;
                        screamSource.Play();
                        break;
                    }
                case scp096State.attack:
                    {
                        state = scp096State.sitting;
                        Timer = 7;
                        break;
                    }
            }
        }
    }

    void ACT_PathDebugging()
    {
        for (int i = 0; i < currPath.corners.Length - 1; i++)
            Debug.DrawLine(currPath.corners[i], currPath.corners[i + 1], Color.red);
    }

    void SetNavPath(Vector3 pos)
    {
        currNode = 0;
        hasPath = NavMesh.CalculatePath(transform.position, pos, NavMesh.AllAreas, currPath);
        if (!hasPath)
            Debug.Log("Path 096 failed!");
    }

    void DoNavPath()
    {

        movement = agent.desiredVelocity;
    }

    Vector3 getRandomPoint()
    {
        currTarget = transform.position + Random.insideUnitSphere * 5;
        currTarget.y = transform.position.y;
        return currTarget;
    }

    void NPCEvent()
    {
        switch (state)
        {
            case scp096State.panic:
                {
                    agent.ResetPath();
                    break;
                }
            case scp096State.idle:
                {
                    agent.ResetPath();
                    break;
                }
            case scp096State.patrol:
                {
                    agent.speed = normalSpeed;
                    if (agent.remainingDistance < 0.5)
                        state = scp096State.idle;

                    break;
                }
            case scp096State.run:
                {
                    agent.speed = panicSpeed;

                    if (agent.remainingDistance < 0.5)
                        state = scp096State.attack;

                    break;
                }
        }

        if (isRota)
            ACT_Rotation();
    }

    public override void Event_Spawn(bool instant, Vector3 warppoint)
    {
        base.Event_Spawn(instant, warppoint);
        agent.Warp(warppoint);
        animator.Rebind();
        state = scp096State.idle;
        currAnim = state;
        agent.Warp(warppoint);
        isEvent = true;
    }

    public override void Spawn(bool beActive, Vector3 warppoint)
    {
        base.Spawn(beActive, warppoint);
        data.isActive = beActive;
        animator.Rebind();
        state = scp096State.idle;
        currAnim = state;
        agent.Warp(warppoint);
    }

    public override void StopEvent()
    {
        Debug.Log("Finishing event");
        base.StopEvent();
        Timer = 0;
        isEvent = false;
    }

    public void evWalkTo(Vector3 to)
    {
        if (state == scp096State.idle)
            state = scp096State.patrol;
        else if (state == scp096State.panic)
            state = scp096State.run;

        isEvent = true;
        agent.SetDestination(to);
    }
    /// <summary>
    /// Change 096 fake state
    /// </summary>
    /// <param name="newState">state (0 = idle, sitting, patrol, panic, run, attack)</param>
    public void evChangeState(int newState)
    {
        isEvent = true;
        state = (scp096State)newState;
    }


    /// <summary>
    /// Rotates 096 in a event
    /// </summary>
    /// <param name="rotateTo">Point to rotate towards</param>
    public void RotateTo(Vector3 rotateTo)
    {
        toRota = Quaternion.LookRotation((new Vector3(rotateTo.x, transform.position.y, rotateTo.z) - transform.position));
        fromAngle = transform.rotation;
        Timer = 0;
        isRota = true;
    }

    void ACT_Rotation()
    {
        //Debug.Log("I'm rotating");
        Timer += Time.deltaTime;
        if (Timer > 1f)
        {
            Timer = 1f;
            isRota = false;
        }

        //lerp!
        float perc = Timer / 1f;
        transform.rotation = Quaternion.Lerp(fromAngle, toRota, perc);
    }




    /// <summary>
    /// Check if SCP 096 face is on the camera view
    /// </summary>
    /// <returns></returns>
    bool OnView()
    {
        frustum = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        Vector3 playerDir = GameController.instance.playercache.transform.position - faceCol.transform.position;
        float playerDis = Vector3.Distance(faceCol.transform.position, GameController.instance.playercache.transform.position);
        return GeometryUtility.TestPlanesAABB(frustum, faceCol.bounds) && (Vector3.Dot(playerDir.normalized, faceCol.transform.forward) > viewLimit) && !Physics.Raycast(faceCol.transform.position, playerDir, playerDis, ground) && !GameController.instance.playercache.IsBlinking() && playerDis < 15f;
    }
}
