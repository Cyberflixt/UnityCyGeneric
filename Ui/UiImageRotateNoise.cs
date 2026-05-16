using System;
using UnityEngine;

public class UiImageRotateNoise : MonoBehaviour
{
    public Vector3 min_rotation = Vector3.zero;
    public Vector3 max_rotation = Vector3.zero;
    public float strength = 1;
    public float damping = 0;

    private RectTransform rect;
    private Vector3 oldRot = Vector3.zero;

    public static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }

    void Start()
    {
        rect = transform.GetComponent<RectTransform>();
    }

    private bool rotationZero = false;
    void Update()
    {
        if (strength > 0)
        {
            rotationZero = false;
            Vector3 rot = new(
                UnityEngine.Random.Range(min_rotation.x, max_rotation.x) * strength,
                UnityEngine.Random.Range(min_rotation.y, max_rotation.y) * strength,
                UnityEngine.Random.Range(min_rotation.z, max_rotation.z) * strength
            );

            if (damping > 0)
            {
                oldRot = Vector3.Lerp(rot, oldRot, damping * Time.deltaTime);
                rect.localEulerAngles = oldRot;
            }
            else
            {
                rect.localEulerAngles = rot;
            }
        }
        else if (!rotationZero)
        {
            rotationZero = true;
            rect.localRotation = Quaternion.identity;
        }
    }
}
