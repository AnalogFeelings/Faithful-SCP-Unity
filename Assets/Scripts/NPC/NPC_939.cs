using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

    
public class NPC_939 : MonoBehaviour
{

    private enum state_939 { idle, patrol, hearing, hearing2, walk, run, attack };

    public NavMeshAgent Agent;
    public AudioSource Audio;
    GameObject Player;
    Vector3 currentTarget;
    Collider[] CloseSounds;
    public Transform[] patrol;
    public AudioClip [] Hello;
    public AudioClip[] Heard;
    public AudioClip[] Found;
    public AudioClip[] Attack;
    public AudioClip Hit;
    public AudioClip AttackTheme;
    Vector3 lookAt;
    public LayerMask groundlayer;


    public float ListeningRange, closeRange, walkSpeed, runSpeed, HearingTimer, Hearing2Timer, defIdle, defWalk, foundPlayer, foundPlayerRun, AttackCool = 1, AttackDistance, WaitTimerBase;
    float Timer = 0, AttackTimer = 0, playerDistance, playerCheck, WaitTimer;
    public int frame;
    public Animator Animator;
    public LayerMask SoundLayer;
    int SoundLevel=0;
    int currentNode=0, currentPatrol;
    public bool isDebuggin, debugSpeed, debugPlayerPos;


    bool foundTarget;
    bool destSet;
    bool stateSet, debugGameLoaded = false;
    bool checkPlayer = true;
    string soundlevel = "No sounds ";
    state_939 state = state_939.idle;

    



    // Start is called before the first frame update
    void Start()
    {
        Agent.Warp(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (!debugGameLoaded)
        {
            if (GameController.instance.doGameplay)
            {
                Player = GameController.instance.player;
                debugGameLoaded = true;
                checkPlayer = true;
            }

        }
        else
        {
            switch (state)
            {
                case state_939.idle:
                    {
                        if (!stateSet)
                        {
                            playerCheck = foundPlayer;
                            Animator.SetBool("move", false);
                            Agent.isStopped = true;
                            destSet = false;
                            stateSet = true;
                            Timer = Random.Range(defIdle, defIdle + 3);
                            if (isDebuggin)
                                Debug.Log("Volviendo a Idle");
                        }
                        break;
                    }
                case state_939.attack:
                    {
                        Animator.SetBool("move", true);
                        Animator.SetTrigger("reset");

                        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookAt), 5 * Time.deltaTime);
                        playerCheck = foundPlayerRun;
                        if (playerDistance < AttackDistance)
                        {
                            Agent.isStopped = true;
                        }
                        else
                        {
                            Agent.isStopped = false;
                            Agent.speed = runSpeed;
                            if (CheckPlayer())
                            {
                                Agent.SetDestination(Player.transform.position);
                                WaitTimer = WaitTimerBase;
                            }
                            else
                            {
                                WaitTimer -= Time.deltaTime;
                                if (WaitTimer <= 0)
                                {
                                    stateSet = false;
                                    state = state_939.idle;
                                    GameController.instance.DefMusic();
                                }
                            }
                        }
                        break;
                    }
                case state_939.patrol:
                    {
                        if (!stateSet)
                        {
                            playerCheck = foundPlayer;
                            Animator.SetBool("move", true);
                            Agent.isStopped = false;
                            Agent.speed = walkSpeed;
                            Agent.SetDestination(patrol[currentNode].position);
                            Timer = Random.Range(defWalk, defWalk + 3);
                            stateSet = true;
                            if (isDebuggin)
                                Debug.Log("Caminata");

                        }
                        else if (Agent.remainingDistance < 1)
                        {
                            currentNode += 1;
                            if (currentNode >= patrol.Length)
                                currentNode = 0;
                            stateSet = false;
                        }
                        break;
                    }
                case state_939.run:
                    {
                        if (destSet == false)
                        {
                            playerCheck = foundPlayerRun;
                            Animator.SetBool("move", true);
                            Agent.isStopped = false;
                            Agent.speed = runSpeed;
                            Agent.SetDestination(currentTarget);
                            destSet = true;
                            if (isDebuggin)
                                Debug.Log(soundlevel + "Corro hacia el!");
                            PlayVoice(4);
                        }
                        else if (Agent.remainingDistance < 1)
                        {
                            destSet = false;
                            foundTarget = false;
                            state = state_939.idle;
                        }
                        break;
                    }
                case state_939.walk:
                    {
                        if (destSet == false)
                        {
                            playerCheck = foundPlayer;
                            PlayVoice(3);
                            Animator.SetBool("move", true);
                            Agent.isStopped = false;
                            Agent.speed = walkSpeed;
                            Agent.SetDestination(currentTarget);
                            destSet = true;
                            if (isDebuggin)
                                Debug.Log(soundlevel + "Camino hacia el!");
                        }
                        else if (Agent.remainingDistance < 1)
                        {
                            destSet = false;
                            foundTarget = false;
                            state = state_939.idle;
                        }
                        break;
                    }
                case state_939.hearing:
                    {
                        if (!stateSet)
                        {
                            Agent.isStopped = true;
                            Animator.SetBool("move", false);
                            Animator.SetTrigger("look");
                            destSet = false;
                            Timer = HearingTimer;
                            if (isDebuggin)
                                Debug.Log(soundlevel + "Escuche algo?");
                            stateSet = true;
                            PlayVoice(1);
                        }
                        break;
                    }
                case state_939.hearing2:
                    {
                        if (!stateSet)
                        {
                            Agent.isStopped = true;
                            Animator.SetBool("move", false);
                            Animator.SetTrigger("vocal");
                            destSet = false;
                            Timer = Hearing2Timer;
                            if (isDebuggin)
                                Debug.Log(soundlevel + "Si escuche!");
                            stateSet = true;
                            PlayVoice(2);
                        }
                        break;
                    }
            }

            Timer -= Time.deltaTime;
            AttackTimer -= Time.deltaTime;

            if ((state != state_939.run && state != state_939.walk && state != state_939.attack) && Timer <= 0)
            {
                foundTarget = false;
                stateSet = false;

                switch (state)
                {
                    case state_939.idle:
                        {
                            state = state_939.patrol;
                            break;
                        }
                    case state_939.patrol:
                        {
                            state = state_939.idle;
                            break;
                        }
                }
            }
            if (state == state_939.run || state == state_939.walk || state == state_939.patrol || state == state_939.attack)
            {
                Animator.SetFloat("speed", Agent.velocity.magnitude);

                if (debugSpeed)
                    Debug.Log(Agent.velocity.magnitude);
            }





            if (foundTarget == false && state != state_939.attack)
            {
                CheckSounds();
                if (foundTarget)
                {
                    stateSet = false;

                    if (SoundLevel == 0)
                    {
                        switch (state)
                        {
                            case state_939.hearing2:
                                {
                                    state = state_939.walk;
                                    break;
                                }
                            case state_939.run:
                                {
                                    state = state_939.hearing2;
                                    break;
                                }
                            case state_939.hearing:
                                {
                                    state = state_939.hearing2;
                                    break;
                                }
                            default:
                                {
                                    Debug.Log("Cambiando a hearing!");
                                    state = state_939.hearing;
                                    break;
                                }
                        }
                    }
                    if (SoundLevel == 1)
                    {
                        switch (state)
                        {
                            case state_939.hearing2:
                                {
                                    state = state_939.walk;
                                    break;
                                }
                            case state_939.hearing:
                                {
                                    state = state_939.walk;
                                    break;
                                }
                            case state_939.run:
                                {
                                    state = state_939.walk;
                                    break;
                                }
                            default:
                                {
                                    state = state_939.hearing2;
                                    break;
                                }
                        }
                    }
                    if (SoundLevel > 1)
                    {
                        switch (state)
                        {
                            case state_939.hearing2:
                                {
                                    state = state_939.run;
                                    break;
                                }
                            case state_939.run:
                                {
                                    state = state_939.run;
                                    break;
                                }
                            default:
                                {
                                    state = state_939.walk;
                                    break;
                                }
                        }
                    }
                }
                else
                {
                    if (state != state_939.idle && state != state_939.patrol)
                        state = state_939.idle;
                }
            }

            if (checkPlayer)
            playerDistance = Vector3.Distance(Player.transform.position, transform.position);
            lookAt = new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z) - transform.position;
            if (debugPlayerPos)
                Debug.Log("Producto dot " + Vector3.Dot(transform.forward, lookAt.normalized));

            if (playerDistance < playerCheck)
            {
                if (state != state_939.attack && Vector3.Dot(transform.forward, lookAt.normalized) > 0.5f && CheckPlayer())
                {
                    WaitTimer = WaitTimerBase;
                    foundTarget = false;
                    stateSet = false;
                    PlayVoice(4);
                    MusicPlayer.instance.Music.PlayOneShot(AttackTheme);

                    if (state == state_939.run)
                        Animator.SetTrigger("leap");
                    state = state_939.attack;
                }
            }
            else if (state == state_939.attack)
                state = state_939.idle;
        }
    }

    void PlayVoice(int library)
    {
        Audio.Stop();
        float delay=0;
        if (library == 1)
        {
            Audio.clip = Hello[Random.Range(0,Hello.Length)];
            delay = 0.5f;
        }
        if (library == 2)
        {
            Audio.clip = Heard[Random.Range(0, Heard.Length)];
            delay = 2;
        }
        if (library == 3)
        {
            Audio.clip = Found[Random.Range(0, Found.Length)];
            delay = 0.5f;
        }
        if (library == 4)
        {
            Audio.clip = Attack[Random.Range(0, Attack.Length)];
        }
        Audio.PlayDelayed(delay);
    }


    bool CheckPlayer()
    {
        Debug.DrawRay(Player.transform.position, (transform.position + new Vector3(0, 0.4f, 0)) - Player.transform.position);

        if (Time.frameCount % frame == 0)
        {
            if (!Physics.Raycast(Player.transform.position, (transform.position + new Vector3(0, 0.4f, 0)) - Player.transform.position, playerDistance, groundlayer))
            {
                return true;
            }
        }
        return false;
    }




    void CheckSounds()
    {
        float lastdistance = 100f;
        SoundLevel = -1;
        float currdistance;
        int currentSoundLevel;
        foundTarget = false;
        WorldSound currentSound;
        CloseSounds = Physics.OverlapSphere(transform.position, ListeningRange, SoundLayer);
        if (CloseSounds.Length != 0)
        {
            for (int i = 0; i < CloseSounds.Length; i++)
            {
                currentSound = CloseSounds[i].gameObject.GetComponent<WorldSound>();
                currentSoundLevel = currentSound.SoundLevel;
                currdistance = Vector3.Distance(transform.position, CloseSounds[i].transform.position);
                if (currdistance > closeRange)
                    currentSoundLevel -= 1;

                if (SoundLevel < currentSoundLevel)
                {
                    if (currdistance < lastdistance)
                    {
                        currentTarget = CloseSounds[i].gameObject.transform.position;
                        SoundLevel = currentSoundLevel;
                        soundlevel = "sonido " + SoundLevel + " distancia " + currdistance + " ";
                        foundTarget = true;
                    }

                }
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && AttackTimer <= 0)
        {
            if (GameController.instance.isAlive)
            {
                Animator.SetTrigger("attack" + Random.Range(1, 3));
                other.gameObject.GetComponent<Player_Control>().Health -= 25;
                AttackTimer = AttackCool;
                Audio.PlayOneShot(Hit);

                if (other.gameObject.GetComponent<Player_Control>().Health <= 0)
                {
                    GameController.instance.deathmsg = GlobalValues.deathStrings["death_939"];
                    checkPlayer = false;
                    playerDistance = 100;
                    stateSet = false;
                    state = state_939.patrol;
                }
            }
            else if (checkPlayer)
            {
                checkPlayer = false;
                playerDistance = 100;
                stateSet = false;
                state = state_939.patrol;
            }
        }
    }


}
