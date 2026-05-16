using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRigidbodyVelocity : MonoBehaviour
{
    public new AudioSource audio;
    public new Rigidbody rigidbody;
    public float velocityMin = .1f;
    public float velocityMax = 1f;
    public float volumeMin = 0f;
    public float volumeMax = 1f;
    public float pitchMin = 1f;
    public float pitchMax = 1f;
    public float speedSmoothing = 2f;


    private float t = 0;
    
    // Update is called once per frame
    void Update()
    {
        // Get speed
        float speed = Mathf.Clamp(rigidbody.linearVelocity.magnitude, velocityMin, velocityMax);
        speed = (speed - velocityMin) / (velocityMax - velocityMin);

        // Lerp speed
        float smoothing = Time.deltaTime * speedSmoothing;
        if (smoothing > 1)
            t = speed;
        else
            t = Mathf.Lerp(t, speed, smoothing);
        
        if (t is float.NaN)
            t = 1;

        if (volumeMin == volumeMax){
            audio.volume = volumeMin;
        } else {
            audio.volume = Sounds.mainVolume * Mathf.Lerp(volumeMin, volumeMax, t);
        }

        if (pitchMin == pitchMax){
            audio.pitch = pitchMin;
        } else {
            audio.pitch = Mathf.Lerp(pitchMin, pitchMax, t);
        }
    }
}
