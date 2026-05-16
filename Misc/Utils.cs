using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    // Buncha general utilities and extension methods

    /// <summary>
    /// Destroys all children (Edit mode)
    /// </summary>
    public static void ClearAllChildrenEditMode(this Transform transform)
    {
        // Retrieve children in array
        var tempArray = new GameObject[transform.childCount];
        for (int i = 0; i < tempArray.Length; i++)
        {
            tempArray[i] = transform.GetChild(i).gameObject;
        }

        foreach (var child in tempArray)
        {
            UnityEngine.Object.DestroyImmediate(child);
        }
    }

    /// <summary>
    /// Destroys all children (Play mode)
    /// </summary>
    public static void ClearAllChildren(this Transform transform)
    {
        // Retrieve children in array
        var tempArray = new GameObject[transform.childCount];
        for (int i = 0; i < tempArray.Length; i++)
        {
            tempArray[i] = transform.GetChild(i).gameObject;
        }

        foreach (var child in tempArray)
        {
            UnityEngine.Object.Destroy(child);
        }
    }

    public static List<T> GetComponentsInDirectChildren<T>(this Transform parent)
    {
        List<T> children = new List<T>();
        foreach (Transform child in parent)
        {
            if (child.TryGetComponent<T>(out T component))
            {
                children.Add(component);
            }
        }
        return children;
    }

    /// <summary>
    /// Returns the given vector with absolute values
    /// </summary>
    public static Vector3 VectorAbs(this Vector3 vec)
    {
        return new Vector3(Math.Abs(vec.x), Math.Abs(vec.y), Math.Abs(vec.z));
    }

    /// <summary>
    /// Returns the colliders in a hitbox
    /// </summary>
    public static Collider[] Hitbox(Vector3 pos, Quaternion rotation, Vector3 size, LayerMask mask)
    {
        Collider[] hits = Physics.OverlapBox(pos, size / 2f, rotation, mask);
        return hits;
    }
    public static Collider[] Hitbox(Transform transform, Vector3 offset, Vector3 size, LayerMask mask)
    {
        Vector3 pos = transform.position + transform.rotation * offset;
        return Hitbox(pos, transform.rotation, size, mask);
    }

    public static void StartAnimation(this Animator animator, string name, float transition = .1f)
    {
        animator.CrossFade(name, transition, 0, 0f, 0f);
    }
    public static void StartAnimation(this Animator animator, int state_hash, float transition = .1f)
    {
        animator.CrossFade(state_hash, transition, 0, 0f, 0f);
    }

    public static Vector3 Flat(this Vector3 v)
    {
        return new Vector3(v.x, 0f, v.z);
    }

    public static Vector3 Flat(this Vector3 v, Transform relativeTransform)
    {
        if (relativeTransform == null)
            return Flat(v);

        Vector3 local = relativeTransform.InverseTransformVector(v);
        local.y = 0;
        return relativeTransform.TransformVector(local);
    }
    public static Transform FindRecursive(this Transform transform, string name)
    {
        if (transform.name == name)
            return transform;

        foreach (Transform child in transform)
        {
            Transform found = FindRecursive(child, name);
            if (found != null) return found;
        }

        return null;
    }
    public static List<Transform> FindAll(this Transform transform, string name)
    {
        List<Transform> found = new List<Transform>();
        FindAllRec(transform, name, found);

        return found;
    }
    private static Transform FindAllRec(this Transform transform, string name, List<Transform> found)
    {
        if (transform.name == name)
            found.Add(transform);

        foreach (Transform child in transform)
        {
            FindAllRec(child, name, found);
        }

        return null;
    }

    public static Vector3 Vector3Random()
    {
        float v = 1;
        return new Vector3(
            UnityEngine.Random.Range(-v, v),
            UnityEngine.Random.Range(-v, v),
            UnityEngine.Random.Range(-v, v)
        );
    }

    public static List<Renderer> GetRenderers(Transform start)
    {
        List<Renderer> renderers = new List<Renderer>();
        GetRenderers(start, renderers);
        return renderers;
    }
    public static List<Renderer> GetRenderers(Transform start, List<Renderer> renderers)
    {
        Renderer ren = start.GetComponent<Renderer>();
        if (ren)
        {
            renderers.Add(ren);
        }
        foreach (Transform child in start)
        {
            GetRenderers(child, renderers);
        }
        return renderers;
    }

    public static Vector3 Round(this Vector3 vector)
    {
        return new(Mathf.Round(vector.x), Mathf.Round(vector.y), Mathf.Round(vector.z));
    }

    public static Vector3 Round(this Vector3 vector, int precision)
    {
        return new(
            Mathf.Round(vector.x * precision) / precision,
            Mathf.Round(vector.y * precision) / precision,
            Mathf.Round(vector.z * precision) / precision);
    }

    public static T GetRandom<T>(this T[] arr)
    {
        return arr[UnityEngine.Random.Range(0, arr.Length-1)];
    }

    public static Vector3 NormalizedOrZero(this Vector3 vector)
    {
        if (vector == Vector3.zero)
            return Vector3.zero;
        return vector.normalized;
    }

    public static Vector3 Multiply(this Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }

    /// <summary>
    /// Ease outs a value efficiently, [0; +inf[ -> [0; 2[
    /// </summary>
    /// <param name="x">Value to ease out</param>
    /// <param name="slowness">x value at which y=1, the higher the slower the growth</param>
    public static float SlowSlope(float x, float slowness = 5)
    {
        return x * 2 / (x + slowness);
    }

    public static IEnumerator WaitTrueCallback(Func<bool> condition, Action callback)
    {
        while (condition())
            yield return null;
        callback();
    }

    public static IEnumerator DoForSeconds(float duration, Action<float> callback)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = time / duration;

            callback(alpha);

            yield return null;
        }
    }

    /*
    /// <summary>
    /// Starts a coroutine and execute it until its first yield
    /// </summary>
    public static Coroutine StartCoroutineNow(this MonoBehaviour behaviour, IEnumerator enumerator)
    {
        AdvanceToFirstYield(enumerator);
        return behaviour.StartCoroutine(enumerator);
    }

    /// <summary>
    /// Use this instead of WaitForSeconds if you're using StartCoroutineNow
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    public static IEnumerator WaitForSecondsNow(float seconds)
    {
        float startTime = Time.time;
        while (Time.time < startTime + seconds)
        {
            yield return null;
        }
    }
    */

    public static void AdvanceToFirstYield(IEnumerator e)
    {
        while (true)
        {
            // If MoveNext returns false the enumerator finished synchronously
            if (!e.MoveNext()) return;

            var current = e.Current;
            // If the current is a nested IEnumerator, dive into it and keep going
            if (current is IEnumerator nested)
            {
                e = nested;
                continue;
            }

            // Otherwise (null, WaitForSeconds, yield return null, etc.) - stop here.
            return;
        }
    }
}
