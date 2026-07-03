using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FpsCamera : MonoBehaviour
{
    private static InputAction actionCamera;
    private static Vector2 cameraVector = Vector2.zero;
    private static Vector2 cameraVectorSmooth = Vector2.zero;

    [NonSerialized] public static float sensibility = 0.2f;

    private const float maxPitch = 90;

    private Quaternion baseRotation = Quaternion.identity;

    private void Start()
    {
        InputExt.ReadyCallback(() =>
        {
            actionCamera = InputExt.actions["Camera"];
        });
    }

    private void Update()
    {
        if (Cursor.lockState == CursorLockMode.None)
            return;

        if (InputExt.ready)
        {
            Vector2 cameraDelta = actionCamera.ReadValue<Vector2>();
            cameraVector += cameraDelta * sensibility;
            if (cameraVector.y > maxPitch)
                cameraVector.y = maxPitch;
            if (cameraVector.y < -maxPitch)
                cameraVector.y = -maxPitch;
        }

        cameraVectorSmooth = Vector2.Lerp(cameraVectorSmooth, cameraVector, Time.deltaTime * 20);
        transform.rotation = baseRotation * Quaternion.Euler(0, cameraVectorSmooth.x, 0) * Quaternion.Euler(-cameraVectorSmooth.y, 0, 0);
    }

    public void Rotate(Quaternion rotOffset)
    {
        baseRotation *= rotOffset;
    }
}
