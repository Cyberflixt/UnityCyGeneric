using UnityEngine;
using UnityEngine.UI;

public class UiBarSprites : MonoBehaviour
{
    public Image[] images;
    public Sprite spriteOff;
    public Sprite spriteOn;

    public void SetValue(int value)
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].sprite = i < value ? spriteOn : spriteOff;
        }
    }
}
