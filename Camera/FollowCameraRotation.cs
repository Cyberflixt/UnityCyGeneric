using UnityEngine;

public class FollowCameraRotation : MonoBehaviour
{
    void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward.Flat();
    }
}
