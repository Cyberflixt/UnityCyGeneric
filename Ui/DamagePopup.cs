using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public TMP_Text label;
    public float duration = 1;
    public Vector3 scaleBase = Vector3.one;
    public AnimationCurve scaleAnim = AnimationCurve.Linear(0,1,1,0);
    public Gradient color;
    public AnimationCurve offsetY = AnimationCurve.Linear(0,1,1,0);

    [Header("Noise")]
    public Vector3 noiseStrength = new(1, 1, 0);
    public float noiseFrequencyFactor = 1;
    public Vector3 noiseFrequencies = new(12f, 9f, 14f);
    public AnimationCurve noiseOverTime = AnimationCurve.Linear(0,1,1,0);

    private float timeLeft;
    private Transform labelTransform;
    private Vector3 positionBased;

    void Start()
    {
        timeLeft = duration;
        labelTransform = label.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float t = 1 - timeLeft / duration;
        timeLeft -= Time.deltaTime;

        label.color = color.Evaluate(t);
        labelTransform.localScale = scaleBase * scaleAnim.Evaluate(t);

        if (noiseStrength != Vector3.zero)
        {
            float f = Time.time * noiseFrequencyFactor;
            labelTransform.position = positionBased + new Vector3(
                Mathf.Sin(f * noiseFrequencies.x) * noiseStrength.x,
                Mathf.Sin(f * noiseFrequencies.y) * noiseStrength.y,
                Mathf.Sin(f * noiseFrequencies.z) * noiseStrength.z
            ) * noiseOverTime.Evaluate(t) + new Vector3(0, offsetY.Evaluate(t));
        }

        if (timeLeft <= 0)
            Destroy(gameObject);
    }

    public void SetText(string text)
    {
        label.text = text;
        positionBased = transform.position;
    }
}
