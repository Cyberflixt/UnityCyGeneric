using System;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    public float minIntensity = 2;
    public float maxIntensity = 3;

    [SerializeField] private float freq0 = 1.3f;
    [SerializeField] private float freq1 = 3.45f;
    [SerializeField] private float freq2 = 5.45f;

    private new Light light;

    void Start()
    {
        light = GetComponent<Light>();
    }

    void Update()
    {
        double f = Math.Sin(freq0 * Time.time) * Math.Sin(freq1 * Time.time) * Math.Sin(freq2 * Time.time);

        light.intensity = minIntensity + (maxIntensity - minIntensity) * (float)(f*f);
    }
}
