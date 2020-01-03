using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class LightFlicker : MonoBehaviour
{
        // Properties
    public string waveFunction = "sin"; // possible values: sin, tri(angle), sqr(square), saw(tooth), inv(verted sawtooth), noise (random)
    public float varbase = 0.0f; // start
    public float amplitude = 1.0f; // amplitude of the wave
    public float phase = 0.0f; // start point inside on wave cycle
    public float frequency = 0.5f; // cycle frequency per second
    public HDAdditionalLightData light;
    public float x;

    // Keep a copy of the original color
    private float originalColor;
     
    // Store the original color
    void Start() {
        light = GetComponent<HDAdditionalLightData>();
        originalColor = light.lightDimmer;
        x = transform.position.x;
    }
     
    void Update() {
      light.lightDimmer = originalColor * EvalWave();
      transform.position = new Vector3(x + (EvalWave()/10), transform.position.y, transform.position.z);
    }
     
    float EvalWave () {
      float x = (Time.time + phase)*frequency;
        float y;
     
      x = Mathf.Floor(x); // normalized value (0..1)
     
      if (waveFunction=="sin") {
        y = Mathf.Sin(x*2*Mathf.PI);
      }
      else if (waveFunction=="tri") {
        if (x < 0.5)
          y = 4.0f * x - 1.0f;
        else
          y = -4.0f * x + 3.0f;  
      }    
      else if (waveFunction=="sqr") {
        if (x < 0.5)
          y = 1.0f;
        else
          y = -1.0f;  
      }    
      else if (waveFunction=="saw") {
          y = x;
      }    
      else if (waveFunction=="inv") {
        y = 1.0f - x;
      }    
      else if (waveFunction=="noise") {
        y = 1 - (Random.value*2);
      }
      else {
        y = 1.0f;
      }        
      return (y*amplitude)+varbase;     
    }
     

}
