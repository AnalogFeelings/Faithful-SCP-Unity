using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_939 : MonoBehaviour
{
    public NavMeshAgent Agent;
    Vector3 currentTarget;
    Collider[] CloseSounds;
    public float ListeningRange, walkSpeed, runSpeed;
    public int frame;
    public Animator Animator;
    public LayerMask SoundLayer;
    public int SoundLevel=0;
    



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckSounds()
    {
        float lastdistance = 100f;
        float currdistance;
        WorldSound currentSound;
        CloseSounds = Physics.OverlapSphere(transform.position, ListeningRange, SoundLayer);
        if (CloseSounds.Length != 0)
        {
            for (int i = 0; i < CloseSounds.Length; i++)
            {
                currentSound = CloseSounds[i].gameObject.GetComponent<WorldSound>();
                currdistance = Vector3.Distance(transform.position, CloseSounds[i].transform.position);
            }
        }

    }


}
