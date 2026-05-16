using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RopeRenderer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;
    [SerializeField] private int resolution = 10;

    [SerializeField] private int dipY = 3;

    void Awake()
    {
        Refresh();
    }

    private float dipFactor(float t)
    {
        return 1 - (2*t - 1) * (2*t - 1);
    }

    [Button]
    public void Refresh()
    {
        Vector3[] points = new Vector3[resolution+1];

        for (int i = 0; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            Vector3 linearPos = Vector3.Lerp(startPosition.position, endPosition.position, t);
            linearPos.y -= dipFactor(t) * dipY;
            points[i] = linearPos;
        }

        lineRenderer.positionCount = resolution+1;
        lineRenderer.SetPositions(points);
    }
}
