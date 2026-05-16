using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[Serializable]
[AddTypeMenu("Transform/Anchors Animation")]
public class AnimationAnchors : AnimationElementDelayed
{
    [Header("Animation")]
    public RectTransform transform;
    public bool setStartAnchors = false;
    [ShowIf("setStartAnchors")]
    public Vector2 startAnchorMin = Vector2.zero;
    [ShowIf("setStartAnchors")]
    public Vector2 startAnchorMax = Vector2.zero;
    public Vector2 endAnchorMin = Vector2.zero;
    public Vector2 endAnchorMax = Vector2.zero;
    public float duration = 1;
    public Ease ease = Ease.InOutQuad;

    protected override IEnumerator VirtAnimationCoroutine(Func<bool> isPlaying)
    {
        if (setStartAnchors)
        {
            transform.anchorMin = startAnchorMin;
            transform.anchorMax = startAnchorMax;
        }

        string easeId = transform.GetInstanceID()+"AnimationAnchors";
        DOTween.Kill(easeId);

        var tween = DOTween.To(
            () => new Vector4(transform.anchorMin.x, transform.anchorMin.y, transform.anchorMax.x, transform.anchorMax.y),
            v => {
                transform.anchorMin = new Vector2(v.x, v.y);
                transform.anchorMax = new Vector2(v.z, v.w);
            }, new Vector4(endAnchorMin.x, endAnchorMin.y, endAnchorMax.x, endAnchorMax.y), duration).SetEase(ease).SetId(easeId);

        if (waitForCompletion)
            yield return tween.WaitForCompletion();
    }
}
