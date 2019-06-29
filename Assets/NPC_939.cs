using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

<<<<<<< HEAD


public class NPC_939 : MonoBehaviour
{
    private enum state_939 { idle, patrol, hearing, pursuit, attack };
=======
public class NPC_939 : MonoBehaviour
{
>>>>>>> b51288c654010cd38746c4fc80b787219a127ef4
    public NavMeshAgent Agent;
    Vector3 currentTarget;
    Collider[] CloseSounds;
    public float ListeningRange, walkSpeed, runSpeed;
    public int frame;
    public Animator Animator;
    public LayerMask SoundLayer;
    public int SoundLevel=0;
<<<<<<< HEAD
    public bool foundTarget;
    public bool destSet;
    state_939 state = state_939.idle;
=======
>>>>>>> b51288c654010cd38746c4fc80b787219a127ef4
    



    // Start is called before the first frame update
    void Start()
    {
<<<<<<< HEAD
        Agent.Warp(transform.position);
=======
        
>>>>>>> b51288c654010cd38746c4fc80b787219a127ef4
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
        switch (state)
        {
            case state_939.idle:
                {
                    Agent.isStopped = true;
                    destSet = false;
                    break; 
                }
            case state_939.pursuit:
                {
                    if (destSet == false)
                    {
                        Agent.isStopped = false;
                        Agent.SetDestination(currentTarget);
                        destSet = true;
                    }
                    else if (Agent.remainingDistance < 2)
                    {
                        destSet = false;
                        foundTarget = false;
                    }
                    break;
                }
            case state_939.hearing:
                {
                    Agent.isStopped = true;
                    destSet = false;
                    Vector3 lookrota = currentTarget - transform.position;
                    transform.rotation = Quaternion.LookRotation(lookrota);
                    foundTarget = false;
                    break;
                }
        }

        if (foundTarget == false)
        {
            CheckSounds();
            if (foundTarget)
            {
                if (SoundLevel <= 1)
                    state = state_939.hearing;
                else
                    state = state_939.pursuit;
            }
            else
                state = state_939.idle;



        }



=======
        
>>>>>>> b51288c654010cd38746c4fc80b787219a127ef4
    }

    void CheckSounds()
    {
        float lastdistance = 100f;
<<<<<<< HEAD
        SoundLevel = 0;
        float currdistance;
        foundTarget = false;
=======
        float currdistance;
>>>>>>> b51288c654010cd38746c4fc80b787219a127ef4
        WorldSound currentSound;
        CloseSounds = Physics.OverlapSphere(transform.position, ListeningRange, SoundLayer);
        if (CloseSounds.Length != 0)
        {
            for (int i = 0; i < CloseSounds.Length; i++)
            {
<<<<<<< HEAD
                
                currentSound = CloseSounds[i].gameObject.GetComponent<WorldSound>();
                if (SoundLevel < currentSound.SoundLevel)
                {
                    currdistance = Vector3.Distance(transform.position, CloseSounds[i].transform.position);
                    if (currdistance < lastdistance)
                    {
                        currentTarget = CloseSounds[i].gameObject.transform.position;
                        SoundLevel = currentSound.SoundLevel;
                        foundTarget = true;
                    }

                }
            }
        }
=======
                currentSound = CloseSounds[i].gameObject.GetComponent<WorldSound>();
                currdistance = Vector3.Distance(transform.position, CloseSounds[i].transform.position);
            }
        }

>>>>>>> b51288c654010cd38746c4fc80b787219a127ef4
    }


}
