using System;
using System.Collections;
using UnityEngine.Events;

[Serializable]
[AddTypeMenu("Flow/Event")]
public class AnimationEvent : AnimationElementDelayed
{
    public UnityEvent call;

    protected override IEnumerator VirtAnimationCoroutine(Func<bool> isPlaying)
    {
        call.Invoke();
        return null;
    }
}
