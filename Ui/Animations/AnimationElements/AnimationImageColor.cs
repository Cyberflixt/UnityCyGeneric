using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[AddTypeMenu("UI/Image Color Animation")]
public class AnimationImageColor : AnimationElementDelayed
{
    [Header("Animation")]
    public Image target;
    public bool setStartColor = false;
    [ShowIf("setStartColor")]
    public Color startColor = Color.white;
    public Color endColor = Color.white;
    public float duration = 1;
    public Ease ease = Ease.InOutQuad;

    protected override IEnumerator VirtAnimationCoroutine(Func<bool> isPlaying)
    {
        if (setStartColor)
            target.color = startColor;
            
        string easeId = target.GetInstanceID()+"AnimationImageColor";
        DOTween.Kill(easeId);

        var tween = DOTween.To(() => target.color, (x) => target.color = x, endColor, duration).SetEase(ease).SetId(easeId);
        if (waitForCompletion)
            yield return tween.WaitForCompletion();
    }
}
