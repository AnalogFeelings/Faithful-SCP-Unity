using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EV_Puppet_Controller : MonoBehaviour
{
    public float Speed, Distance, Gravity, maxfallspeed, animOffset, stopDistance, pushoverrange, pushSpeed = 0.125f;
    Vector3 movement;
    Quaternion toAngle;
    float fallSpeed;
    int currentNode = 0, currSeq = 0;
    bool isPath, isRotate, isLook, isSequence = false, isPursuit = false, hasDoor = false, hasSubs, isPushing = false, active=true;
    Transform[] ActualPath;
    Transform rotaAt, lookAt, Location;
    int pathNodes, audSeq;
    public CharacterController _controller;
    public GameObject Puppet_Mesh, Def_LookAt;
    Animator Puppet_Anim;
    HeadLookController Head;
    NavMeshAgent _navMeshagent;
    public LayerMask DoorLay, PlayerLay;
    public string charName;
    public bool PushOver=false;

    /// <summary>
    /// Audio Values
    /// </summary>

    AudioClip currAudio;
    AudioClip[] audioSeq;
    AudioSource Audio;

    // Start is called before the first frame update
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        Audio = GetComponent<AudioSource>();
        _navMeshagent = this.GetComponent<NavMeshAgent>();

        Puppet_Anim = Puppet_Mesh.GetComponent<Animator>();
        Puppet_Anim.SetFloat("AnimOffset", animOffset);
        Head = Puppet_Mesh.GetComponent<HeadLookController>();


    }

    void Start()
    {
        Head.effect = 0;
    }

    public void DeactivateCollision()
    {
        _controller.enabled = false;
        active = false;
    }
    public void EnableCollision()
    {
        _controller.enabled = true;
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            CheckDoor();

            if (PushOver && !isPursuit && !isPath)
                PlayerPush();

            if (isPath)
            {
                ACT_Path();
            }
            if (isRotate)
            {
                Vector3 Point = new Vector3(rotaAt.position.x, transform.position.y, rotaAt.position.z) - transform.position;
                toAngle = Quaternion.LookRotation(Point);
            }
            if (isPursuit)
                FindPath();



            if (isLook)
            {
                Head.target = lookAt.transform.position;
            }
            else
            {
                Head.target = Def_LookAt.transform.position;
                if (Head.effect > 0.005)
                    Head.effect = Mathf.Lerp(Head.effect, 0, 0.125f * Time.deltaTime);
                else
                    Head.effect = 0;
            }


            if (!isPursuit)
            {
                if (!_controller.isGrounded)
                    ACT_Gravity();
                if (Time.deltaTime != 0)
                _controller.Move(movement);
                movement = Vector3.Lerp(movement, Vector3.zero, 4f * Time.deltaTime);
                if (isPath)
                    transform.rotation = Quaternion.Lerp(transform.rotation, toAngle, 8f * Time.deltaTime);
                else
                    transform.rotation = Quaternion.Lerp(transform.rotation, toAngle, 3f * Time.deltaTime);
            }
        }
    }

    void LateUpdate()
    {
        ACT_Anim();
        if (isSequence)
            Sequence();
    }

    void ACT_Path()
    {
        if (Vector3.Distance(new Vector3(ActualPath[currentNode].position.x, transform.position.y, ActualPath[currentNode].position.z), transform.position) < Distance)
        {
            if (currentNode != pathNodes)
                currentNode += 1;
        }

        Vector3 Point = new Vector3(ActualPath[currentNode].position.x, transform.position.y, ActualPath[currentNode].position.z) - transform.position;

        toAngle = Quaternion.LookRotation(Point);

        movement = (transform.forward * Speed) * Time.deltaTime;

        if ((Vector3.Distance(new Vector3(ActualPath[currentNode].position.x, transform.position.y, ActualPath[currentNode].position.z), transform.position) < stopDistance) && currentNode == pathNodes)
        {
            isPath = false;
        }
    }




    void ACT_Anim()
    {

        Puppet_Anim.SetBool("move", (isPath||isPursuit||isPushing));

    }


    void ACT_Gravity()
    {
        if (_controller.isGrounded)
            fallSpeed = 0;
        fallSpeed -= Gravity * Time.deltaTime;
        if (fallSpeed < maxfallspeed)
            fallSpeed = maxfallspeed;

        movement.y = fallSpeed;
    }


    public void SetPath(Transform[] NewPath)
    {
        ActualPath = NewPath;
        pathNodes = NewPath.Length-1;
        isPath = true;
        isRotate = false;
        isPursuit = false;
        currentNode = 0;
    }

    public void SetRota(Transform LookAt)
    {
        rotaAt = LookAt;
        isRotate = true;
        isPath = false;
        isPursuit = false;
    }
    public void StopRota()
    {
        isRotate = false;
    }

    public void SetLookAt(Transform LookAt)
    {
        Head.effect = 1;
        lookAt = LookAt;
        isLook = true;
    }
    public void StopLookAt()
    {
        isLook = false;
    }

    public void PausePath()
    {
        isPath = false;
    }
    public void ResumePath()
    {
        isPath = true;
    }

    public void SetPursuit(Transform Here)
    {
        isPath = false;
        isRotate = false;
        isPursuit = true;
        Location = Here;
        _navMeshagent.isStopped = false;
    }

    public void StopPursuit()
    {
        _navMeshagent.isStopped = true;
        isPursuit = false;
    }

    void FindPath()
    {
        Vector3 targetVector = Location.transform.position;
        _navMeshagent.SetDestination(targetVector);
    }



    public void PlaySound(AudioClip toPlay, bool _playSubs = false, bool Force = false)
    {
        currAudio = toPlay;
        Audio.clip = currAudio;
        Audio.Play();
        if (_playSubs)
            SubtitleEngine.instance.playSub(string.Format(GlobalValues.sceneStrings[currAudio.name], GlobalValues.charaStrings[charName]), true, Force);
    }
    public void StopSound()
    {
        Audio.Stop();
    }

    public void PlaySFX(AudioClip toPlay)
    {
        Audio.PlayOneShot(toPlay);
    }

    public void SetSeq(AudioClip[] newSeq, bool _hasSubs = false)
    {
        audioSeq = newSeq;
        audSeq = newSeq.Length - 1;
        isSequence = true;
        currSeq = 0;
        PlaySound(audioSeq[0], _hasSubs);
        hasSubs = _hasSubs;
    }

    public void AnimTrigger(int Number, bool value)
    {
        switch(Number)
        {
            case 2:
                {
                    Puppet_Anim.SetBool("param2", value);
                    break;
                }
            case 1:
                {
                    Puppet_Anim.SetBool("param1", value);
                    break;
                }
            case -1:
                {
                    Puppet_Anim.SetTrigger("param-1");
                    break;
                }
            case -2:
                {
                    Puppet_Anim.SetTrigger("param-2");
                    break;
                }
            case -3:
                {
                    Puppet_Anim.SetTrigger("param-3");
                    break;
                }
        }

    }


    void Sequence()
    {
        if (Audio.isPlaying == false)
        {
            if (currSeq != audSeq)
            {
                currSeq += 1;
                PlaySound(audioSeq[currSeq], hasSubs);
            }
            else
                isSequence = false;
        }

    }

    void CheckDoor()
    {
            Collider[] Interact;
            Interact = Interact = Physics.OverlapSphere(transform.position + (transform.forward * 1.7f), 1.9f, DoorLay);
            if (Interact.Length != 0)
            {
                Debug.DrawRay(transform.position+(transform.forward*1.5f), Interact[0].transform.position - transform.position);
                Interact[0].transform.gameObject.GetComponent<Object_Door>().ForceOpen(1.5f);
            }
    }

    void PlayerPush()
    {
        Collider[] Interact;
        Interact = Interact = Physics.OverlapCapsule(transform.position+Vector3.up * 4, transform.position, pushoverrange, PlayerLay);

        if (Interact.Length != 0)
        {
            Debug.DrawRay(transform.position + (transform.forward * 1.5f), Interact[0].transform.position - transform.position);
            movement -= ((Interact[0].transform.position - transform.position).normalized * (pushSpeed/2) * Time.deltaTime);
            isPushing = true;
        }
        else
            isPushing = false;
    }

    public void puppetWarp(Vector3 here, float rotation)
    {
        _navMeshagent.Warp(here);
        Vector3 rota = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(rota.x, rota.y + rotation, rota.z);
    }






}
