using UnityEngine;

public class AnimationRadioSelector : MonoBehaviour
{
    public string animationNameOpen = "Open";
    public string animationNameClose = "Close";

    public AnimationPlayer[] animationPlayers;

    public void SelectIndex(int index)
    {
        for (int i = 0; i < animationPlayers.Length; i++)
        {
            string anim = index == i ? animationNameOpen : animationNameClose;
            animationPlayers[i]?.PlayAnimation(anim);
        }
    }

    public void SelectNone()
    {
        foreach (AnimationPlayer player in animationPlayers)
        {
            player?.PlayAnimation(animationNameClose);
        }
    }
}
