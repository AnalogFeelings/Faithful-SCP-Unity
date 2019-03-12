using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EV_medibay : MonoBehaviour
{
    public GameObject light1, light2;
    // Start is called before the first frame update
    void OnEnable()
    {
        light1.SetActive(false);
        light2.SetActive(false);
    }
}
