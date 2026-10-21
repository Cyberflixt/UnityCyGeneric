using UnityEngine;

public class AnimationStart : MonoBehaviour
{
    public AnimationPlayer player;
    public new string name;

    void Start()
    {
        player.PlayAnimation(name);
    }
}
