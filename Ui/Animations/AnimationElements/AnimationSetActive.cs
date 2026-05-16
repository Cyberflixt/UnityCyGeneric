using System;
using System.Collections;
using UnityEngine;

[Serializable]
[AddTypeMenu("Transform/Set Active")]
public class AnimationSetActive : AnimationElementDelayed
{
    [Header("Animation")]
    public GameObject gameObject;
    public bool setActive = true;

    protected override IEnumerator VirtAnimationCoroutine(Func<bool> isPlaying)
    {
        gameObject.SetActive(setActive);
        return null;
    }
}
