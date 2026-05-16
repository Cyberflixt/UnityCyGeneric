using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimationSequenceNamed : IAnimationSequence
{
    public string name = "unnamed";
    [SerializeReference, SubclassSelector]
    public List<AnimationElementBase> animations = new();

    public IEnumerator Play(Func<bool> isPlaying)
    {
        foreach (AnimationElementBase animation in animations)
        {
            if (animation != null)
            {
                IEnumerator e = animation.AnimationCoroutine(isPlaying);
                if (!e.MoveNext()) // Not done?
                    yield return e;
            }
        }
        yield break;
    }
}
