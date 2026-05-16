using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[Serializable]
[AddTypeMenu("Transform/Set Anchors")]
public class AnimationSetAnchors : AnimationElementDelayed
{
    public RectTransform transform;
    public Vector2 anchorMin = Vector2.zero;
    public Vector2 anchorMax = Vector2.zero;

    protected override IEnumerator VirtAnimationCoroutine(Func<bool> isPlaying)
    {
        DOTween.Kill(transform.GetInstanceID()+"Anchors");
        transform.anchorMin = anchorMin;
        transform.anchorMax = anchorMax;
        yield break;
    }
}
