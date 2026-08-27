using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public TMP_Text label;
    public TMP_Text labelBack;
    public Transform anchor;
    public float duration = 1;
    public AnimationCurve scaleAnim = AnimationCurve.Linear(0, 1, 1, 0);
    public Gradient color;
    public AnimationCurve offsetY = AnimationCurve.Linear(0, 1, 1, 0);
    public CanvasGroup canvasGroup;
    public AnimationCurve alpha = AnimationCurve.Linear(0, 1, 1, 0);

    [Header("Noise")]
    public Vector2 noiseStrength = new(10, 10);
    public float noiseFrequencyFactor = 8;
    public Vector2 noiseFrequencies = new(12, 9);
    public AnimationCurve noiseOverTime = AnimationCurve.Linear(0, 1, 1, 0);

    private float timeLeft;
    private Vector3 scaleBase;

    void Start()
    {
        timeLeft = duration;
        scaleBase = transform.localScale;

        Destroy(gameObject, duration);
    }

    // Update is called once per frame
    void Update()
    {
        float t = 1 - timeLeft / duration;
        timeLeft -= Time.deltaTime;

        label.color = color.Evaluate(t);
        canvasGroup.alpha = alpha.Evaluate(t);
        transform.localScale = scaleBase * scaleAnim.Evaluate(t);

        if (noiseStrength != Vector2.zero)
        {
            float f = Time.time * noiseFrequencyFactor;
            anchor.localPosition = new Vector3(
                Mathf.Sin(f * noiseFrequencies.x) * noiseStrength.x,
                Mathf.Sin(f * noiseFrequencies.y) * noiseStrength.y,
                0
            ) * noiseOverTime.Evaluate(t) + new Vector3(0, offsetY.Evaluate(t));
        }
    }

    public void SetText(string text)
    {
        label.text = text;
        if (labelBack != null)
            labelBack.text = text;
    }
}
