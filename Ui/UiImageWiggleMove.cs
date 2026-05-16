using System;
using UnityEngine;

public class UiImageWiggleMove : MonoBehaviour
{
    [SerializeField] private RectTransform rect;
    [SerializeField] private Vector3 positionA;
    [SerializeField] private Vector3 positionB;
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
            rect.localPosition = Vector3.Lerp(positionA, positionB, EaseInOutQuad(t));
        }
    }
}
