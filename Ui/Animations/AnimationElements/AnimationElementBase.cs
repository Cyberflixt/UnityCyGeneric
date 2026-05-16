using System;
using System.Collections;
using UnityEngine;

[Serializable]
public abstract class AnimationElementBase
{
    public virtual IEnumerator AnimationCoroutine(Func<bool> isPlaying)
    {
        yield break;
    }
    
    protected virtual IEnumerator VirtAnimationCoroutine(Func<bool> isPlaying)
    {
        yield break;
    }
}
