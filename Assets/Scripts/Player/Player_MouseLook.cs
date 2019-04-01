using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player_MouseLook : MonoBehaviour
{
    Vector3 rotation = new Vector3(0, 0, 0);
    public float speed = 3;
    private void Start()
    {

    }
    void LateUpdate()
    {
        rotation.y += (Input.GetAxis("Mouse X")*speed)* Time.timeScale;
        rotation.x += -(Input.GetAxis("Mouse Y")*speed)* Time.timeScale;
        rotation.x = Mathf.Clamp(rotation.x, -75f, 75f);


        rotation.z = transform.eulerAngles.z;

        transform.eulerAngles = rotation;

    }
}