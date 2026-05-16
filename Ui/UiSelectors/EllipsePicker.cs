using UnityEngine;

public class EllipsePicker : MonoBehaviour
{
    // Equation:
    // (cos(x) * xFactor + xAdd; sin(x) * yFactor + yAdd)

    [SerializeField] private float xFactor = 1;
    [SerializeField] private float yFactor = 1;
    [SerializeField] private float xAdd = 0;
    [SerializeField] private float yAdd = 0;

    [SerializeField] private RectTransform element;
    [SerializeField] private float damping = .1f;
    [SerializeField] private bool wiggleEnabled = false;
    [SerializeField, ShowIf("wiggleEnabled")] private float wiggleStrength = 1;
    [SerializeField, ShowIf("wiggleEnabled")] private float wiggleFrequency = 1;


    private float currentAngle = 0;
    private float targetAngle = 0;


    public void SetTargetAngle(float angle)
    {
        targetAngle = angle;
    }

    public void SetElementAngle(float x)
    {
        element.localPosition = new Vector3(Mathf.Cos(x) * xFactor + xAdd, Mathf.Sin(x) * yFactor + yAdd);
    }

    void Update()
    {
        currentAngle = Mathf.Lerp(targetAngle, currentAngle, damping * Time.deltaTime);

        float angle = currentAngle;
        if (wiggleEnabled)
            angle += Mathf.Cos(Time.time * wiggleFrequency) * wiggleStrength;

        SetElementAngle(angle);
    }
}
