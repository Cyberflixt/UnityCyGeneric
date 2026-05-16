using System;
using System.Collections;
using UnityEngine;

[Serializable]
[AddTypeMenu("Transform/Set Position")]
public class AnimationSetPosition : AnimationElementDelayed
{
    public Transform transform;
    public Vector3 position = Vector3.zero;

    protected override IEnumerator VirtAnimationCoroutine(Func<bool> isPlaying)
    {
        transform.localPosition = position;
        return null;
    }
}
