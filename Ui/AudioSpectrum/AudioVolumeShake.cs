using System;
using UnityEngine;

public class AudioVolumeShake : MonoBehaviour
{
    public AudioSpectrumProcessor processor;
    public AnimationCurve strengthCurve = AnimationCurve.Linear(0,0,1,1);
    public float strength = 20f;

    public float freqX = 5.1f;
    public float freqY = 7.2f;

    void LateUpdate()
    {
        float loudness = processor.GetLoudness();
        float factor = strengthCurve.Evaluate(loudness) * strength;
        transform.localPosition = new Vector3(Mathf.Sin(freqX * Time.time) * factor, Mathf.Sin(freqY * Time.time) * factor, 0);
    }
}
