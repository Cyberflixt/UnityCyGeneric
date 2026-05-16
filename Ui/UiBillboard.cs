using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class UiBillboard : MonoBehaviour
{
    private new Transform camera;

    void Start()
    {
        camera = Camera.main.transform;
    }

    void Update()
    {
        transform.rotation = camera.rotation;
    }
}
