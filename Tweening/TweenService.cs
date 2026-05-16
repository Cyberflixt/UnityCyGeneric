using System;
using System.Collections.Generic;
using UnityEngine;

public class TweenService : MonoBehaviour
{
    class PositionTween
    {
        public readonly Transform transform;
        public readonly Vector3 startPosition;
        public readonly Vector3 endPosition;
        public readonly bool localPosition;
        public readonly float duration;
        public readonly Func<float, float> easingFunction;
        public int loops;
        public float timeSpent;
        public PositionTween(Transform transform, Vector3 startPosition, Vector3 endPosition, bool localPosition, float duration, Func<float, float> easingFunction, int loops)
        {
            this.transform = transform;
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            this.localPosition = localPosition;
            this.duration = duration;
            this.easingFunction = easingFunction;
            this.loops = loops;
            timeSpent = 0;
        }
    }

    private static Dictionary<Transform, PositionTween> positionTweens = new();

    // Singleton
    private static TweenService instance;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public static void TweenLocalPosition(Transform transform, Vector3 endPosition, float duration, Func<float, float> easingFunction, int loops = 0)
    {
        // Local Space
        PositionTween data = new PositionTween(
            transform,
            transform.localPosition,
            endPosition,
            true,
            duration,
            easingFunction,
            loops
        );
        positionTweens[transform] = data;
        Debug.Log("started tween");
    }
    public static void TweenLocalPosition(Transform transform, Vector3 endPosition, float duration, EasingStyle easingStyle, EasingDirection easingDirection, int loops = 0)
    {
        TweenLocalPosition(transform, endPosition, duration, Eases.GetFunc(easingStyle, easingDirection), loops);
    }
    public static void TweenPosition(Transform transform, Vector3 endPosition, float duration, Func<float, float> easingFunction, int loops = 0)
    {
        // World Space
        PositionTween data = new PositionTween(
            transform,
            transform.position,
            endPosition,
            false,
            duration,
            easingFunction,
            loops
        );
        positionTweens[transform] = data;
    }
    public static void TweenPosition(Transform transform, Vector3 endPosition, float duration, EasingStyle easingStyle, EasingDirection easingDirection, int loops = 0)
    {
        TweenPosition(transform, endPosition, duration, Eases.GetFunc(easingStyle, easingDirection), loops);
    }

    void Update()
    {
        List<Transform> positionTweensDelete = new();
        foreach (Transform key in positionTweens.Keys)
        {
            PositionTween data = positionTweens[key];
            data.timeSpent += Time.deltaTime;

            // Loops
            while (data.timeSpent > data.duration && data.loops != 0)
            {
                if (data.loops > 0) // Any negative value means infite loops
                    data.loops--;
                data.timeSpent -= data.duration;
            }

            // Tween finished?
            if (data.timeSpent > data.duration)
            {
                // Finalize
                positionTweensDelete.Add(key);
                if (data.localPosition)
                    data.transform.localPosition = data.endPosition;
                else
                    data.transform.position = data.endPosition;
            }
            else
            {
                // Tween
                float t = data.easingFunction(data.timeSpent / data.duration);
                if (data.localPosition)
                    data.transform.localPosition = Vector3.Lerp(data.startPosition, data.endPosition, t);
                else
                    data.transform.position = Vector3.Lerp(data.startPosition, data.endPosition, t);
            }
        }
        foreach (Transform key in positionTweensDelete)
        {
            positionTweens.Remove(key);
        }
    }
}
