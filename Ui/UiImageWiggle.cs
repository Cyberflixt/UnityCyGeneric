using System;
using UnityEngine;

public class UiImageWiggle : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private float min_angle = 0;
    [SerializeField] private float max_angle = 0;
    [SerializeField] private float duration = 2;

    float EaseInOutQuad(float x)
    {
        return x < 0.5 ? 2 * x * x : 1 - (-2 * x + 2) * (-2 * x + 2) / 2;
    }

    public static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }

    void Update()
    {
        if (rect)
        {
            float t = (Time.time % (duration * 2) - duration) / duration;
            if (t < 0)
                t = -t;
            rect.localEulerAngles = new Vector3(0, 0, Lerp(min_angle, max_angle, EaseInOutQuad(t)));
        }
    }
}
