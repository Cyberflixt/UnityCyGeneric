using UnityEngine;

public class UiBarSimpleDual : MonoBehaviour
{
    [SerializeField] private RectTransform bar;
    [SerializeField] private RectTransform barSlow;

    [Header("Value = 0")]
    [SerializeField] private Vector2 anchorsMin0 = new(0,0);
    [SerializeField] private Vector2 anchorsMax0 = new(0,1);
    [Header("Value = 1")]
    [SerializeField] private Vector2 anchorsMin1 = new(0,0);
    [SerializeField] private Vector2 anchorsMax1 = new(1,1);

    public float value = 0.5f;
    public float valueMax = 1;
    public float valueMin = 0;
    private const float speedAnim = 10f;
    private const float speedSlowAnim = 1f;

    private float valueAnim;
    private float valueSlowAnim;

    void Start()
    {
        valueAnim = LerpInverse(valueMin, valueMax, value);
        valueSlowAnim = valueAnim;
    }

    float LerpInverse(float min, float max, float v)
    {
        return (v - min) / (max - min);
    }

    void Update()
    {
        float t = LerpInverse(valueMin, valueMax, value);
        valueAnim = Mathf.Lerp(valueAnim, t, speedAnim * Time.deltaTime);

        if (valueAnim > valueSlowAnim)
            valueSlowAnim = valueAnim;
        valueSlowAnim = Mathf.Lerp(valueSlowAnim, t, speedSlowAnim * Time.deltaTime);

        bar.anchorMin = Vector2.Lerp(anchorsMin0, anchorsMin1, valueAnim);
        barSlow.anchorMin = Vector2.Lerp(anchorsMin0, anchorsMin1, valueSlowAnim);
        
        bar.anchorMax = Vector2.Lerp(anchorsMax0, anchorsMax1, valueAnim);
        barSlow.anchorMax = Vector2.Lerp(anchorsMax0, anchorsMax1, valueSlowAnim);
    }

    public void ConnectMinMaxValueFloat(MinMaxValueFloat v)
    {
        value = v.value;
        valueMin = v.min;
        valueMax = v.max;
        v.onChanged += () =>
        {
            value = v.value;
            valueMin = v.min;
            valueMax = v.max;
        };
    }
}
