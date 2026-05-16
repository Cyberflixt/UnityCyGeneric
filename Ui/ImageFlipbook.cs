using UnityEngine;
using UnityEngine.UI;

public class ImageFlipbook : MonoBehaviour
{
    // Settings
    public float interval = 0.3f;
    public Image image;
    public Sprite[] sprites;
    
    // Runtime
    private float timeAccu = 0;
    private int index = 0;

    void Update()
    {
        timeAccu -= Time.deltaTime;
        if (timeAccu < 0)
        {
            timeAccu += interval;
            index++;
            if (index >= sprites.Length)
                index = 0;
            image.sprite = sprites[index];
        }
    }
}
