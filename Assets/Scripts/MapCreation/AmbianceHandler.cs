using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Ambiances {drip, fan, fuelpump, lowdrone, pulsing, rumble, servers1, ventilation, alarm };

public class AmbianceHandler : MonoBehaviour
{
    public Transform origin;
    public Ambiances Ambiance;
    public bool hasOrigin;
    public float Volume, closeDistance, spread, spatial;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
