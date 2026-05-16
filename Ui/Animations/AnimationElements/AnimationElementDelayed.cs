using System;
using System.Collections;
using UnityEngine;

[Serializable]
public abstract class AnimationElementDelayed : AnimationElementBase
{
    [SerializeField] public float delay = 0;
    public bool waitForCompletion = false;
    
    public override IEnumerator AnimationCoroutine(Func<bool> isPlaying)
    {
        if (waitForCompletion)
        {
            yield return InnerAnimationCoroutine(isPlaying);
        }
        else
        {
            // Immediately run coroutine
            FrameCoroutines.StartCoroutine(InnerAnimationCoroutine(isPlaying));
        }
    }

    private IEnumerator InnerAnimationCoroutine(Func<bool> isPlaying)
    {
        if (isPlaying())
        {
            if (delay > 0)
            {
                float t = Time.time;
                yield return FrameCoroutines.WaitForSeconds(delay);
                if (isPlaying())
                {
                    yield return VirtAnimationCoroutine(isPlaying);
                }
            }
            else
            {
                yield return VirtAnimationCoroutine(isPlaying);
            }
        }
    }
}
