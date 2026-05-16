using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[AddTypeMenu("UI/Canvas Group Opacity Animation")]
public class AnimationCanvasGroupOpacity : AnimationElementDelayed
{
    [Header("Animation")]
    public CanvasGroup target;
    public bool setStartOpacity = false;
    [ShowIf("setStartOpacity")]
    public float startOpacity = 1;
    public float endOpacity = 1;
    public float duration = 1;
    public Ease ease = Ease.InOutQuad;

    protected override IEnumerator VirtAnimationCoroutine(Func<bool> isPlaying)
    {
        if (setStartOpacity)
            target.alpha = startOpacity;
            
        string easeId = target.GetInstanceID()+"AnimationCanvasGroupOpacity";
        DOTween.Kill(easeId);

        var tween = DOTween.To(() => target.alpha, (x) => target.alpha = x, endOpacity, duration).SetEase(ease).SetId(easeId);
        if (waitForCompletion)
            yield return tween.WaitForCompletion();
    }
}
