using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class testpath_ : MonoBehaviour
{
    [SerializeField]
    Transform _destination;

    NavMeshAgent _navMeshagent;
    Animator anim;
    Vector3 velocity;
    public GameObject meshChild;

    // Use this for initialization
    public void Start()
    {
        _navMeshagent = this.GetComponent<NavMeshAgent>();
        anim = meshChild.GetComponent<Animator>();
    }

    public void LateUpdate()
    {
        if (_navMeshagent == null)
        {
            Debug.LogError("Nav Mesh Agent component not found attached to " + gameObject.name);
        }
        else
        {
            if (Input.GetButtonDown("Blink"))
                SetDestination();
        }
        velocity = _navMeshagent.velocity;

        bool shouldMove = velocity.magnitude > 0.5f && _navMeshagent.remainingDistance > _navMeshagent.radius;

        // Update animation parameters
        anim.SetBool("move", shouldMove);
    }

    private void SetDestination()
    {
        if (_destination != null)
        {
            Vector3 targetVector = _destination.transform.position;
            _navMeshagent.SetDestination(targetVector);
        }
    }
}
