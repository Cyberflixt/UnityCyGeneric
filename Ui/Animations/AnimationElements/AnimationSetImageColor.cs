using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[AddTypeMenu("UI/Set Image Color")]
public class AnimationSetImageColor : AnimationElementDelayed
{
    public Image target;
    public Color color = Color.white;

    protected override IEnumerator VirtAnimationCoroutine(Func<bool> isPlaying)
    {
        target.color = color;
        return null;
    }
}
