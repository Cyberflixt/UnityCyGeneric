using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public class ShakeData
    {
        public float time;
        public float strength;
        public float duration;
        public Vector3 position;
        public bool hasPosition;

        public ShakeData(float strength, float duration, Vector3 position, bool hasPosition)
        {
            time = Time.time;
            this.strength = strength;
            this.duration = duration;
            this.position = position;
            this.hasPosition = hasPosition;
        }
    }

    public static List<ShakeData> shakes = new();

    public Quaternion rotation = Quaternion.identity;
    public static CameraShake instance = null;
    public static bool active
    {
        get { return instance != null; }
    }

    private void OnEnable()
    {
        instance = this;
    }
    private void OnDisable()
    {
        instance = null;
    }


    public static void AddShake(float strength, float duration, Vector3 position)
    {
        shakes.Add(new ShakeData(strength, duration, position, true));
    }

    public static void AddShake(float strength, float duration)
    {
        shakes.Add(new ShakeData(strength, duration, Vector3.zero, false));
    }

    private const bool useMaxAmplitude = true; // Use strongest shake or sum up all shakes strengths otherwise

    void Update()
    {
        // Get total amplitude from shakes
        float totalAmplitude = 0;
        float maxAmplitude = 0;

        for (int i = 0; i < shakes.Count; i++)
        {
            ShakeData data = shakes[i];
            float t = 1 - (Time.time - data.time) / data.duration;

            if (t > 0)
            {
                if (data.hasPosition)
                {
                    float dist = Vector3.Distance(transform.position, data.position);

                    float amplitude = t * t * data.strength / dist * 0.01f;
                    if (amplitude > maxAmplitude)
                        maxAmplitude = amplitude;
                    totalAmplitude += amplitude;
                }
                else
                {
                    float amplitude = t * t * data.strength * 0.001f;
                    if (amplitude > maxAmplitude)
                        maxAmplitude = amplitude;
                    totalAmplitude += amplitude;
                }
            }
            else
            {
                shakes.RemoveAt(i);
                i--;
            }
        }

        float finalAmplitude = useMaxAmplitude ? maxAmplitude : totalAmplitude;

        float freq = 100;
        transform.localPosition = new Vector3(
            Mathf.Sin(Time.time * freq),
            Mathf.Sin(Time.time * freq * 1.32f),
            Mathf.Sin(Time.time * freq * .8489f)
        ) * finalAmplitude;

        float rotFac = 80f * finalAmplitude;
        rotation = Quaternion.Euler(
            Mathf.Sin(Time.time * freq) * rotFac,
            Mathf.Sin(Time.time * freq * 1.32f) * rotFac,
            Mathf.Sin(Time.time * freq * .8489f) * rotFac
        );
        transform.localRotation = rotation;
    }
}
