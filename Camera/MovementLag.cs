using UnityEngine;

public class MovementLag : MonoBehaviour
{
    [SerializeField] private Transform projectionSpace;
    [SerializeField] private float speed = 5f;

    private Vector3 oldLocalPos;
    private Vector3 defaultLocalPos;


    void Start()
    {
        oldLocalPos = projectionSpace.InverseTransformPoint(transform.position);
        defaultLocalPos = transform.parent.InverseTransformPoint(transform.position);
    }

    void Update()
    {
        transform.position = Vector3.Lerp(projectionSpace.TransformPoint(oldLocalPos), transform.parent.TransformPoint(defaultLocalPos), Time.deltaTime * speed);
        oldLocalPos = projectionSpace.InverseTransformPoint(transform.position);

        //transform.position = transform.parent.TransformPoint(defaultLocalPos);
    }

    public void Teleported(Quaternion rotationOffset)
    {
        oldLocalPos = rotationOffset * oldLocalPos;
    }
}
