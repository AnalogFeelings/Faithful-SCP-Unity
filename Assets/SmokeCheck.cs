using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class SmokeCheck : MonoBehaviour
{
    VisualEffect sfx;
    public LayerMask ground;
    // Start is called before the first frame update
    void OnEnable()
    {
        sfx = GetComponent<VisualEffect>();
        RaycastHit ray;
        Vector3 rota = transform.rotation * Vector3.up;
        if (Vector3.Dot(Vector3.up, rota) < 0)
        {
            Debug.Log("Object detected as pointing down, dot result " + Vector3.Dot(Vector3.up, transform.rotation.eulerAngles) + " rota " + rota);
            if (Physics.Raycast(transform.position, -Vector3.up, out ray, 10, ground, QueryTriggerInteraction.Ignore))
            {
                sfx.SetVector3("Floor", ray.point);
                sfx.SetVector3("Normal", ray.normal);
            }
        }
        else
        {
            Debug.Log("Object detected as pointing up, dot result" + Vector3.Dot(Vector3.up, transform.rotation.eulerAngles) + " rota " + rota);
            if (Physics.Raycast(transform.position, Vector3.up, out ray, 10, ground, QueryTriggerInteraction.Ignore))
            {
                sfx.SetVector3("Floor", ray.point);
                sfx.SetVector3("Normal", ray.normal);
            }
        }
    }


}
