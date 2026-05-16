using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[Serializable]
[AddTypeMenu("Transform/Rotation Animation")]
public class AnimationRotation : AnimationElementDelayed
{
    [Header("Animation")]
    public Transform transform;
    public bool setStartRotation = false;
    [ShowIf("setStartRotation")]
    public Vector3 startRotation = Vector3.zero;
    public Vector3 endRotation = Vector3.zero;
    public float duration = 1;
    public Ease ease = Ease.InOutQuad;

    protected override IEnumerator VirtAnimationCoroutine(Func<bool> isPlaying)
    {
        if (setStartRotation)
            transform.localEulerAngles = startRotation;
            
        string easeId = transform.GetInstanceID()+"AnimationRotation";
        DOTween.Kill(easeId);

        var tween = transform.DOLocalRotate(endRotation, duration, RotateMode.FastBeyond360).SetEase(ease).SetId(easeId);
        if (waitForCompletion)
            yield return tween.WaitForCompletion();
    }
}
