using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public class ShakeData
    {
        public float time;
        public float strength;
        public Vector3 position;
        public float duration;

        public ShakeData(float strength, Vector3 position, float duration)
        {
            time = Time.time;
            this.strength = strength;
            this.position = position;
            this.duration = duration;
        }
    }
    
    public static List<ShakeData> shakes = new();

    public Quaternion rotation = Quaternion.identity;
    public static CameraShake instance = null;
    public static bool active
    {
        get {return instance != null;}
    }

    private void OnEnable()
    {
        instance = this;
    }
    private void OnDisable()
    {
        instance = null;
    }


    public static void AddShake(float strength, Vector3 position, float duration)
    {
        shakes.Add(new ShakeData(strength, position, duration));
    }

    void Update()
    {
        // Get total amplitude from shakes
        float totalAmplitude = 0;

        for (int i = 0; i < shakes.Count; i++)
        {
            ShakeData data = shakes[i];
            float t = 1 - (Time.time - data.time) / data.duration;

            if (t > 0)
            {
                float dist = Vector3.Distance(transform.position, data.position);

                float amplitude = t * t * data.strength / dist;
                totalAmplitude += amplitude;
            }
            else
            {
                shakes.RemoveAt(i);
                i--;
            }
        }

        float freq = 100;
        transform.localPosition = new Vector3(
            Mathf.Sin(Time.time * freq),
            Mathf.Sin(Time.time * freq * 1.32f),
            Mathf.Sin(Time.time * freq * .8489f)
        ) * totalAmplitude;

        float rotFac = 80f * totalAmplitude;
        rotation = Quaternion.Euler(
            Mathf.Sin(Time.time * freq) * rotFac,
            Mathf.Sin(Time.time * freq * 1.32f) * rotFac,
            Mathf.Sin(Time.time * freq * .8489f) * rotFac
        );
        transform.localRotation = rotation;
    }
}
