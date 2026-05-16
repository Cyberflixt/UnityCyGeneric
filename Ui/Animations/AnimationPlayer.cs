using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    [SerializeField] private bool interruptPlayingAnimations = true;
    [SerializeField] private AnimationSequenceNamed[] animations;
    [SerializeField] private AnimationPlayerGroup animationPlayerGroup;

    private int animationToken = 0;

    public void PlayAnimation(string name)
    {
        if (interruptPlayingAnimations)
            IncrementAnimationToken();

        for (int i = 0; i < animations.Length; i++)
        {
            if (animations[i].name == name)
            {
                int token = GetAnimationToken();
                FrameCoroutines.StartCoroutine(animations[i].Play(() =>
                {
                    return token == GetAnimationToken();
                }));
            }
        }
    }
    
    public void PlayFirstAnimation()
    {
        PlayAnimation(animations[0].name);
    }

    private void IncrementAnimationToken()
    {
        if (animationPlayerGroup)
            animationPlayerGroup.animationToken++;
        else
            animationToken++;
    }
    
    private int GetAnimationToken()
    {
        if (animationPlayerGroup)
            return animationPlayerGroup.animationToken;
        return animationToken;
    }
}
