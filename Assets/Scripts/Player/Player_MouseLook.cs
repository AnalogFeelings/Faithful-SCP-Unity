using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player_MouseLook : MonoBehaviour
{
    Vector2 rotation = new Vector2(0, 0);
    public float speed = 3;
    private void Start()
    {

    }
    void LateUpdate()
    {
        rotation.y += Input.GetAxis("Mouse X") * Time.timeScale;
        rotation.x += -Input.GetAxis("Mouse Y") * Time.timeScale;
        rotation.x = Mathf.Clamp(rotation.x, -25f, 30f);
        transform.eulerAngles = (Vector2)rotation * speed;

    }
}