using UnityEngine;

public class FollowCameraRotation : MonoBehaviour
{
    void LateUpdate()
    {
        Vector3 v = Camera.main.transform.forward.Flat();
        if (v != Vector3.zero)
            transform.forward = v;
    }
}
