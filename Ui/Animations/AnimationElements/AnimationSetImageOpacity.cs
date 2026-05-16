using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[AddTypeMenu("UI/Set Image Opacity")]
public class AnimationSetImageOpacity : AnimationElementDelayed
{
    public Image target;
    public float opacity = 1;

    protected override IEnumerator VirtAnimationCoroutine(Func<bool> isPlaying)
    {
        target.color = new Color(target.color.r, target.color.g, target.color.b, opacity);
        return null;
    }
}
