using System;
using System.Collections;

[Serializable]
[AddTypeMenu("Flow/Start Animation")]
public class AnimationStartAnimation : AnimationElementDelayed
{
    public AnimationPlayer player;
    public string name;

    protected override IEnumerator VirtAnimationCoroutine(Func<bool> isPlaying)
    {
        player.PlayAnimation(name);
        return null;
    }
}
