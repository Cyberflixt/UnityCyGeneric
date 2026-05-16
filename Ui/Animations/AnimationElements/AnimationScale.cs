using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[Serializable]
[AddTypeMenu("Transform/Scale Animation")]
public class AnimationScale : AnimationElementDelayed
{
    [Header("Animation")]
    public Transform transform;
    public bool setStartScale = false;
    [ShowIf("setStartScale")] public Vector3 startScale = Vector3.one;
    public Vector3 endScale = Vector3.one;
    public float duration = 1;
    public Ease ease = Ease.InOutQuad;

    protected override IEnumerator VirtAnimationCoroutine(Func<bool> isPlaying)
    {
        if (setStartScale)
            transform.localScale = startScale;
            
        string easeId = transform.GetInstanceID()+"AnimationScale";
        DOTween.Kill(easeId);

        var tween = DOTween.To(() => transform.localScale, (x) => transform.localScale = x, endScale, duration).SetEase(ease).SetId(easeId);
        if (waitForCompletion)
            yield return tween.WaitForCompletion();
    }
}
