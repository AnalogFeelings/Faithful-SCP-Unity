using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EV_Puppet_Controller : MonoBehaviour
{
    public float Speed, Distance, Gravity, maxfallspeed, animOffset, stopDistance;
    Vector3 movement;
    Quaternion toAngle;
    float fallSpeed;
    int currentNode=0, currSeq=0;
    bool isPath, isRotate, isLook, isSequence=false, isPursuit=false, hasDoor=false;
    Transform[] ActualPath;
    Transform rotaAt, lookAt, Location;
    int pathNodes, audSeq;
    private CharacterController _controller;
    public GameObject Puppet_Mesh, Def_LookAt;
    Animator Puppet_Anim;
    HeadLookController Head;
    NavMeshAgent _navMeshagent;
    public LayerMask DoorLay;

    /// <summary>
    /// Audio Values
    /// </summary>

    AudioClip currAudio;
    AudioClip[] audioSeq;
    AudioSource Audio;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        Audio = GetComponent<AudioSource>();
        _navMeshagent = this.GetComponent<NavMeshAgent>();

        Puppet_Anim = Puppet_Mesh.GetComponent<Animator>();
        Puppet_Anim.SetFloat("AnimOffset", animOffset);
        Head = Puppet_Mesh.GetComponent<HeadLookController>();

    }

    // Update is called once per frame
    void Update()
    {
        CheckDoor();

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
        }


        ACT_Gravity();
        if (!isPursuit)
        {
            _controller.Move(movement);
            movement = Vector3.Lerp(movement, Vector3.zero, 0.05f);
            transform.rotation = Quaternion.Lerp(transform.rotation, toAngle, 4f*Time.deltaTime);
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

        Puppet_Anim.SetBool("move", (isPath||isPursuit));

    }


        void ACT_Gravity()
            {
                fallSpeed -= Gravity * Time.deltaTime;
                if (fallSpeed < maxfallspeed)
                    fallSpeed = maxfallspeed;

                if (_controller.isGrounded && fallSpeed < 0)
                {
                    fallSpeed = 0f;
                }

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



    public void PlaySound(AudioClip toPlay)
    {
        currAudio = toPlay;
        Audio.clip = currAudio;
        Audio.Play();
    }

    public void PlaySFX(AudioClip toPlay)
    {
        Audio.PlayOneShot(toPlay);
    }

    public void SetSeq(AudioClip[] newSeq)
    {
        audioSeq = newSeq;
        audSeq = newSeq.Length - 1;
        isSequence = true;
        currSeq = 0;
        PlaySound(audioSeq[0]);
    }

    public void AnimTrigger(int Number, bool value)
    {
        switch(Number)
        {
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
        }

    }


    void Sequence()
    {
        if (Audio.isPlaying == false)
        {
            if (currSeq != audSeq)
            {
                currSeq += 1;
                PlaySound(audioSeq[currSeq]);
            }
            else
                isSequence = false;
        }

    }

    void CheckDoor()
    {
            Collider[] Interact;
            Interact = Interact = Physics.OverlapSphere(transform.position, 1.9f, DoorLay);
            if (Interact.Length != 0)
            {
                Debug.DrawRay(transform.position, Interact[0].transform.position - transform.position);
                Interact[0].transform.gameObject.GetComponent<Object_Door>().ForceOpen(1.5f);
            }
    }

    public void puppetWarp(Vector3 here)
    {
        _controller.enabled = false;
        transform.position = here;
        _controller.enabled = true;
    }






}
