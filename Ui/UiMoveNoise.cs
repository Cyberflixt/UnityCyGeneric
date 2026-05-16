using System;
using UnityEngine;

public class UiMoveNoise : MonoBehaviour
{
    public Vector3 min_position = Vector3.zero;
    public Vector3 max_position = Vector3.zero;
    public float strength = 1;
    public float damping = 0;

    private RectTransform rect;
    private Vector3 oldPosition = Vector3.zero;

    public static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }

    void Start()
    {
        rect = transform.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (rect)
        {
            Vector3 pos = new(
                UnityEngine.Random.Range(min_position.x, max_position.x) * strength,
                UnityEngine.Random.Range(min_position.y, max_position.y) * strength,
                UnityEngine.Random.Range(min_position.z, max_position.z) * strength
            );

            if (damping > 0)
            {
                oldPosition = Vector3.Lerp(pos, oldPosition, damping * Time.deltaTime);
                rect.localPosition = oldPosition;
            }
            else
            {
                rect.localPosition = pos;
            }
        }
    }
}
