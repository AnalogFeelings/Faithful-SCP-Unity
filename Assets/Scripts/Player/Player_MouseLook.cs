using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player_MouseLook : MonoBehaviour
{
    public Vector3 rotation = new Vector3(0, 0, 0);
    public Quaternion addedRota = Quaternion.identity;
    public float speed = 3;
    private void Start()
    {

    }
    void LateUpdate()
    {
        float z = transform.eulerAngles.z;


        if (addedRota != Quaternion.identity)
        {
            rotation = addedRota.eulerAngles;
            rotation.x = (rotation.x > 180) ? rotation.x - 360 : rotation.x;
        }

            rotation.y += (Input.GetAxis("Mouse X") * speed) * Time.timeScale;
            rotation.x += -(Input.GetAxis("Mouse Y") * speed) * Time.timeScale;
            rotation.x = Mathf.Clamp(rotation.x, -85f, 75f);



        rotation.z = z;



        transform.rotation =  Quaternion.Euler(rotation);
    }
}