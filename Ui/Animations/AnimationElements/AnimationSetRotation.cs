using System;
using System.Collections;
using UnityEngine;

[Serializable]
[AddTypeMenu("Transform/Set Rotation")]
public class AnimationSetRotation : AnimationElementDelayed
{
    public Transform transform;
    public Vector3 rotation = Vector3.zero;

    protected override IEnumerator VirtAnimationCoroutine(Func<bool> isPlaying)
    {
        transform.localEulerAngles = rotation;
        return null;
    }
}
