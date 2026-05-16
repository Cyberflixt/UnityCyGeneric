using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimationSequence
{
    public IEnumerator Play(Func<bool> isPlaying);
}

[Serializable]
public class AnimationSequence : IAnimationSequence
{
    [SerializeReference, SubclassSelector]
    public List<AnimationElementBase> animations = new();

    public IEnumerator Play(Func<bool> isPlaying)
    {
        foreach (AnimationElementBase animation in animations)
        {
            yield return animation.AnimationCoroutine(isPlaying);
        }
    }
}
