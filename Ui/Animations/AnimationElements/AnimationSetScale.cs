using System;
using System.Collections;
using UnityEngine;

[Serializable]
[AddTypeMenu("Transform/Set Scale")]
public class AnimationSetScale : AnimationElementDelayed
{
    public Transform transform;
    public Vector3 scale = Vector3.one;

    protected override IEnumerator VirtAnimationCoroutine(Func<bool> isPlaying)
    {
        transform.localScale = scale;
        return null;
    }
}
