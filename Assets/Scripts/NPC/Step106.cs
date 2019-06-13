using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step106 : MonoBehaviour
{
    public AudioClip[] steps;
    AudioSource foot;
    // Start is called before the first frame update
    void Start()
    {
        foot = GetComponent<AudioSource>();
    }

    void StepSound()
    {
        foot.clip = steps[Random.Range(0, steps.Length)];
        foot.Play();
        DecalSystem.instance.Decal(transform.position, new Vector3(90f, 0, 0), 1f, false, 0.4f, 2, 0);
    }
}
