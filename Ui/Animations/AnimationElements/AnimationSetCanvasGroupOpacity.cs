using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[AddTypeMenu("UI/Set Canvas Group Opacity")]
public class AnimationSetCanvasGroupOpacity : AnimationElementDelayed
{
    public CanvasGroup target;
    public float opacity = 1;

    protected override IEnumerator VirtAnimationCoroutine(Func<bool> isPlaying)
    {
        target.alpha = opacity;
        return null;
    }
}
