using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[AddTypeMenu("UI/Image Opacity Animation")]
public class AnimationImageOpacity : AnimationElementDelayed
{
    [Header("Animation")]
    public Image target;
    public bool setStartOpacity = false;
    [ShowIf("setStartOpacity")]
    public float startOpacity = 1;
    public float endOpacity = 1;
    public float duration = 1;
    public Ease ease = Ease.InOutQuad;

    protected override IEnumerator VirtAnimationCoroutine(Func<bool> isPlaying)
    {
        if (setStartOpacity)
            target.color = new Color(target.color.r, target.color.g, target.color.b, startOpacity);
            
        string easeId = target.GetInstanceID()+"AnimationImageOpacity";
        DOTween.Kill(easeId);

        var tween = DOTween.To(() => target.color.a, (x) => target.color = new Color(target.color.r, target.color.g, target.color.b, x), endOpacity, duration).SetEase(ease).SetId(easeId);
        if (waitForCompletion)
            yield return tween.WaitForCompletion();
    }
}
