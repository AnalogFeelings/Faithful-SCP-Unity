using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbianceController : MonoBehaviour
{
    public bool enableAmbiance;
    public static AmbianceController instance;
    public AudioClip[] AmbianceLibrary;
    public AudioClip[] GenericAmbiance;
    float ambiancetimer = 0, GENambiancetimer = 0, ambiancefreq = 3;
    public float ambifreq, GENambiancefreq;
    public bool custom = true;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (enableAmbiance && AmbianceLibrary.Length != 0)
        DoAmbiance();

    }

    void DoAmbiance()
    {
        ambiancetimer -= Time.deltaTime;
        if (ambiancetimer <= 0)
        {
            int i = Random.Range(0, AmbianceLibrary.Length);
            GameController.instance.MixAmbiance.PlayOneShot(AmbianceLibrary[i]);
            ambiancetimer = ambiancefreq * Random.Range(1, 5);
        }
    }

    public void GenAmbiance()
    {
        GENambiancetimer -= Time.deltaTime;
        if (GENambiancetimer <= 0)
        {
            int i = Random.Range(0, GenericAmbiance.Length);

            GameController.instance.MixAmbiance.PlayOneShot(GenericAmbiance[i]);
            GENambiancetimer = GENambiancefreq * Random.Range(2, 5);
        }
    }

    public void ChangeAmbiance(AudioClip[] NewAmbiance, float freq)
    {
        AmbianceLibrary = NewAmbiance;
        ambiancefreq = freq;
        custom = true;
        //GameController.instance.zoneAmbiance = -1;
    }

    public void NormalAmbiance(AudioClip[] NewAmbiance)
    {
        AmbianceLibrary = NewAmbiance;
        ambiancefreq = ambifreq;
        custom = false;
        //GameController.instance.zoneAmbiance = -1;
    }
}
