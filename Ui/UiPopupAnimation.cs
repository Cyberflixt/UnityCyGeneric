using System.Collections;
using UnityEngine;

public class UiPopupAnimation : MonoBehaviour
{
    public GameObject container;
    public RectTransform anchor;
    public AudioSource audioOpen;
    public AudioSource audioClose;
    public CanvasGroup canvasGroup;

    [Header("Animation")]
    public float duration = 0.5f;
    public AnimationCurve alpha = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public AnimationCurve anchorXMin = AnimationCurve.EaseInOut(0, 0.5f, 1, 0.5f);
    public AnimationCurve anchorYMin = AnimationCurve.EaseInOut(0, 0.5f, 1, 0.5f);
    public AnimationCurve anchorXMax = AnimationCurve.EaseInOut(0, 0.5f, 1, 0.5f);
    public AnimationCurve anchorYMax = AnimationCurve.EaseInOut(0, 0.5f, 1, 0.5f);

    protected bool opened;
    private int animToken = 0;
    private float currentAnimationTime = 0;

    protected virtual void Start()
    {
        // Default state
        opened = false;
        currentAnimationTime = 0;
        UpdateGraphics();
    }

    public virtual void Open()
    {
        if (!opened)
        {
            opened = true;

            audioOpen.Play();
            StartCoroutine(OpenAnim());
        }
    }

    public virtual void Close()
    {
        if (opened)
        {
            opened = false;

            audioClose.Play();
            StartCoroutine(CloseAnim());
        }
    }

    private IEnumerator CloseAnim()
    {
        int token = ++animToken;
        while (token == animToken && currentAnimationTime > 0)
        {
            UpdateGraphics();

            yield return null;
            currentAnimationTime -= Time.deltaTime;
        }
        if (token == animToken)
        {
            currentAnimationTime = 0;
            UpdateGraphics();
        }
    }

    private IEnumerator OpenAnim()
    {
        int token = ++animToken;
        while (token == animToken && currentAnimationTime < duration)
        {
            currentAnimationTime += Time.deltaTime;
            UpdateGraphics();

            yield return null;
        }
        if (token == animToken)
        {
            currentAnimationTime = duration;
            UpdateGraphics();
        }
    }

    private void UpdateGraphics()
    {
        if (currentAnimationTime == 0)
        {
            container.SetActive(false);
        }
        else
        {
            // Normalize
            float t = currentAnimationTime / duration;

            // Alpha
            canvasGroup.alpha = alpha.Evaluate(t);

            // Anchors
            anchor.anchorMin = new Vector2(
                anchorXMin.Evaluate(t), anchorYMin.Evaluate(t)
            );
            anchor.anchorMax = new Vector2(
                anchorXMax.Evaluate(t), anchorYMax.Evaluate(t)
            );

            // Enable if disabled
            //if (!container.activeSelf)
                container.SetActive(true);

        }
    }
}
