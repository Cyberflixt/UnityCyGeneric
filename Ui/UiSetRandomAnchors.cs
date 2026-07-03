using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UiSetRandomAnchors : MonoBehaviour
{
    [SerializeField] private Vector2 minAnchorsA = new(0, 0);
    [SerializeField] private Vector2 minAnchorsB = new(0, 0);
    [SerializeField] private Vector2 maxAnchorsA = new(0, 0);
    [SerializeField] private Vector2 maxAnchorsB = new(1, 1);

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        float t = Random.Range(0f, 1f);
        rectTransform.anchorMin = Vector2.Lerp(minAnchorsA, minAnchorsB, t);
        rectTransform.anchorMax = Vector2.Lerp(maxAnchorsA, maxAnchorsB, t);
    }
}
