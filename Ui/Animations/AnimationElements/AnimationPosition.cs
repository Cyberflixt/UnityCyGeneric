using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[Serializable]
[AddTypeMenu("Transform/Position Animation")]
public class AnimationPosition : AnimationElementDelayed
{
    [Header("Animation")]
    public Transform transform;
    public bool setStartPosition = false;
    [ShowIf("setStartPosition")]
    public Vector3 startPosition = Vector3.zero;
    public Vector3 endPosition = Vector3.zero;
    public float duration = 1;
    public Ease ease = Ease.InOutQuad;

    protected override IEnumerator VirtAnimationCoroutine(Func<bool> isPlaying)
    {
        if (setStartPosition)
            transform.localPosition = startPosition;
            
        string easeId = transform.GetInstanceID()+"AnimationImageColor";
        DOTween.Kill(easeId);

        var tween = transform.DOLocalMove(endPosition, duration).SetEase(ease).SetId(easeId);
        if (waitForCompletion)
            yield return tween.WaitForCompletion();
    }
}
