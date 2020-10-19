using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NPC_049 : Roam_NPC
{
    enum scp049State { idle, patrol, chase, trail, kill, hearing, soundChase, trailIdle}

    //public AudioClip idle, panic, horror, chaseClip, scream;
    Camera mainCamera;
    Plane[] frustum;
    Collider[] closeSounds;
    NavMeshAgent agent;
    AudioSource voiceSource;
    public AudioClip [] Chase, Search;
    public Animator animator;
    public float viewLimit, normalSpeed, chaseSpeed, listeningRange, closeRange;
    public LayerMask ground, doors, playerMask, soundLayer;
    public bool debugIsTargeting, allowMapPath=true;
    scp049State state, currAnim;
    float audTimer, Timer, trailTimer, fallSpeed, framerate = 10, framerate2 = 60, distanceFromPlayer=Mathf.Infinity;
    int currNode, currSoundLevel;
    bool onPath, isRota, seePlayer, foundSound, hasPath,pathIsMap=false;
    Vector3 currTarget;
    NavMeshPath currPath;
    Quaternion toRota, fromAngle;
    static int valIsPanic = 0, valIsChase = 1, valTimer = 2;

    // Start is called before the first frame update
    void Start()
    {
        state = scp049State.idle;
        currAnim = scp049State.idle;
        mainCamera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        voiceSource = GetComponent<AudioSource>();
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

    private void LateUpdate()
    {
        animator.SetFloat("moveSpeed", agent.isOnOffMeshLink ? 1 : agent.velocity.magnitude);
        animator.SetBool("reach", distanceFromPlayer < 5 && state != scp049State.kill);
        if (currAnim != state)
        {
            switch (state)
            {
                case scp049State.trailIdle:
                case scp049State.hearing:
                    {
                        animator.SetTrigger("toHearing");
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
        seePlayer = false;
        if (debugIsTargeting && Time.frameCount % framerate == 0)
        {
            seePlayer = CanSee();
            if(!seePlayer)
            {
                distanceFromPlayer = Mathf.Infinity;
            }
            else
            {
                currTarget = GameController.instance.playercache.transform.position;
            }
            
            if (state != scp049State.chase && state != scp049State.kill && seePlayer)
            {
                data.npcvalue[valIsPanic] = 1;
                state = scp049State.chase;
                audTimer = 0;
            }
        }

        if (Time.frameCount % framerate == 0 && !foundSound)
        {
            CheckSounds();
        }


        //Current State
        switch (state)
        {
            case scp049State.idle:
                {
                    onPath = false;
                    agent.ResetPath();
                    if (foundSound)
                    {
                        if (currSoundLevel < 2 )
                        {
                            audTimer = 0;
                            foundSound = false;
                            state = scp049State.hearing;
                            Timer = Random.Range(5, 10);
                        }
                        else
                        {
                            audTimer = 0;
                            state = scp049State.soundChase;
                            onPath = false;
                        }
                        foundSound = false;
                    }
                    break;
                }
            case scp049State.trailIdle:
            case scp049State.hearing:
                {
                    agent.ResetPath();
                    if (foundSound)
                    {
                        if (currSoundLevel > 0)
                        {
                            audTimer = 0;
                            state = scp049State.soundChase;
                            onPath = false;
                        }
                    }
                    break;
                }
            case scp049State.trail:
                {
                    agent.speed = normalSpeed;
                    if (!agent.hasPath || agent.pathStatus == NavMeshPathStatus.PathInvalid)
                        agent.SetDestination(getRandomPoint());

                    if (foundSound)
                    {
                        if (currSoundLevel < 2)
                        {
                            audTimer = 0;
                            state = scp049State.hearing;
                            Timer = Random.Range(3, 10);
                            foundSound = false;
                        }
                        else
                        {
                            audTimer = 0;
                            state = scp049State.soundChase;
                            onPath = false;
                        }
                    }

                    break;
                }
            case scp049State.patrol:
                {
                    agent.speed = normalSpeed;
                    if (!onPath)
                    {
                        agent.SetDestination(getPatrol());
                        onPath = true;
                    }

                    if (foundSound)
                    {
                        if (currSoundLevel < 2)
                        {
                            audTimer = 0;
                            state = scp049State.hearing;
                            Timer = Random.Range(5, 10);
                            foundSound = false;
                        }
                        else
                        {
                            audTimer = 0;
                            state = scp049State.soundChase;
                        }
                    }

                    if (agent.hasPath && agent.remainingDistance < (pathIsMap ? 5 : 0.5f))
                    {
                        state = scp049State.idle;
                        onPath = false;
                        Timer = Random.Range(5, 10);
                    }

                    break;
                }
            case scp049State.soundChase:
                {
                    agent.speed = normalSpeed;
                    if (!onPath)
                    {
                        agent.SetDestination(currTarget);
                        onPath = true;
                    }


                    if (agent.hasPath && agent.remainingDistance < 0.5f)
                    {
                        if (trailTimer > 0)
                            state = scp049State.trail;
                        else
                            state = scp049State.idle;
                        onPath = false;
                        currSoundLevel = 0;
                        Timer = Random.Range(5, 10);
                    }

                    RaycastHit hit;
                    if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 1f, doors))
                    {
                        hit.transform.gameObject.GetComponent<Object_Door>().ForceOpen(5);
                    }
                    break;
                }
            case scp049State.chase:
                {
                    currSoundLevel = 0;
                    agent.speed = chaseSpeed;
                    if (Time.frameCount % framerate == 0)
                    {
                        if (seePlayer)
                        agent.SetDestination(currTarget);
                    }

                    if (agent.hasPath && agent.remainingDistance < 0.5f && distanceFromPlayer > 1f)
                    {
                        state = scp049State.trail;
                        Timer = Random.Range(5, 10);
                        trailTimer = Random.Range(20, 30);
                    }


                    RaycastHit hit;
                    if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 1f, doors))
                    {
                        hit.transform.gameObject.GetComponent<Object_Door>().ForceOpen(5);
                    }

                    if (Physics.OverlapSphere(transform.position + transform.forward, 0.5f, playerMask).Length > 0)
                    {
                        GameController.instance.playercache.Death(0);
                        Debug.Log("Kill");
                        state = scp049State.kill;
                    }

                    break;
                }
        }

        Timer -= Time.deltaTime;
        trailTimer -= Time.deltaTime;
        audTimer -= Time.deltaTime;

        if (trailTimer < 0 && (state == scp049State.trailIdle || state == scp049State.trail))
        {
            state = scp049State.idle;
            Timer = Random.Range(5, 10);
        }

        //Next State
        if (Timer < 0)
        {
            switch (state)
            {
                case scp049State.trailIdle:
                    {


                        state = scp049State.trail;
                        Timer = Random.Range(3, 10);
                        getRandomPoint();

                        break;
                    }
                case scp049State.trail:
                    {
                        state = scp049State.trailIdle;
                        Timer = Random.Range(3, 10);
                        break;
                    }
                case scp049State.idle:
                    {
                        state = scp049State.patrol;
                        Timer = Random.Range(10, 15);
                        getRandomPoint();

                        break;
                    }
                case scp049State.hearing:
                case scp049State.patrol:
                    {
                        if (trailTimer > 0)
                            state = scp049State.trail;
                        else
                            state = scp049State.idle;
                        Timer = Random.Range(5, 10);
                        break;
                    }
            }
        }

        //Audio state
        if (audTimer < 0)
        {
            switch (state)
            {
                case scp049State.hearing:
                case scp049State.trail:
                case scp049State.trailIdle:
                    {

                        PlayVoice(1);
                        audTimer = Random.Range(8, 15);

                        break;
                    }
                case scp049State.soundChase:
                case scp049State.chase:
                    {
                        PlayVoice(2);
                        audTimer = Random.Range(8, 13);
                        break;
                    }
            }
        }
    }

    void CheckSounds()
    {
        float lastdistance = 100f;
        float currdistance;
        int tempSound;
        foundSound = false;
        WorldSound currentSound;
        closeSounds = Physics.OverlapSphere(transform.position, listeningRange, soundLayer);
        if (closeSounds.Length != 0)
        {
            for (int i = 0; i < closeSounds.Length; i++)
            {
                currentSound = closeSounds[i].gameObject.GetComponent<WorldSound>();
                tempSound = currentSound.SoundLevel;
                currdistance = Vector3.Distance(transform.position, closeSounds[i].transform.position);
                if (currdistance > closeRange)
                    tempSound -= 1;

                if (currSoundLevel < tempSound)
                {
                    if (currdistance < lastdistance)
                    {
                        lastdistance = currdistance;
                        currTarget = closeSounds[i].gameObject.transform.position;
                        currSoundLevel = tempSound;
                        //soundlevel = "sonido " + data.npcvalue[valSoundLevel] + " distancia " + currdistance + " ";
                        foundSound = true;
                    }

                }
            }
        }
    }

    Vector3 getRandomPoint()
    {
        currTarget = transform.position + Random.insideUnitSphere * 5;
        currTarget.y = transform.position.y;
        return currTarget;
    }

    Vector3 getPatrol()
    {
        if (!hasPath)
        {
            if (!GameController.instance.mapless && allowMapPath)
            {
                currTarget = GameController.instance.GetPatrol(transform.position, 4, 0);
                pathIsMap = true;
            }
            else
            {
                currTarget = getRandomPoint();
                pathIsMap = false;
            }
        }
        return currTarget;
    }

    /// <summary>
    /// PLays a sound from any library
    /// </summary>
    /// <param name="library"></param>
    void PlayVoice(int library)
    {
        voiceSource.Stop();
        float delay = 0;
        if (library == 1)
        {
            voiceSource.clip = Search[Random.Range(0, Search.Length)];
            delay = 0.5f;
        }
        if (library == 2)
        {
            voiceSource.clip = Chase[Random.Range(0, Chase.Length)];
            delay = 2;
        }
        voiceSource.PlayDelayed(delay);
    }

    void NPCEvent()
    {
        switch (state)
        {
            case scp049State.idle:
                {
                    agent.ResetPath();
                    break;
                }
            case scp049State.patrol:
                {
                    if (agent.remainingDistance < 0.5)
                        state = scp049State.idle;

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
        state = scp049State.idle;
        currAnim = state;
        agent.Warp(warppoint);
        isEvent = true;
    }

    public override void Spawn(bool beActive, Vector3 warppoint)
    {
        base.Spawn(beActive, warppoint);
        data.isActive = beActive;
        animator.Rebind();
        state = scp049State.idle;
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
        state = (scp049State)newState;
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
    /// Check if SCP 049 face is on the camera view
    /// </summary>
    /// <returns></returns>
    bool CanSee()
    {
        Vector3 playerDir = GameController.instance.playercache.transform.position - transform.position;
        distanceFromPlayer = Vector3.Distance(transform.position, GameController.instance.playercache.transform.position);
        return (distanceFromPlayer < 3 || Vector3.Dot(playerDir.normalized, transform.forward) > viewLimit) && !Physics.Raycast(transform.position+Vector3.up*1.5f, playerDir, distanceFromPlayer, ground) && distanceFromPlayer < 20f;
    }
}

