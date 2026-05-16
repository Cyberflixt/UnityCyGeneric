using System;
using System.Collections;

[Serializable]
[AddTypeMenu("Flow/Start In Parallel")]
public class AnimationStartInParallel : AnimationElementDelayed
{
    public AnimationSequence startInParallel;

    protected override IEnumerator VirtAnimationCoroutine(Func<bool> isPlaying)
    {
        startInParallel.Play(isPlaying);
        return null;
    }
}
