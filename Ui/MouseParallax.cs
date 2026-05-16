using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseParallax : MonoBehaviour
{
    public float strength = 30;
    public float lerpSmooth = 10;

    private Vector3 position = Vector3.zero;
    private InputAction mouseInput;

    void Start()
    {
        mouseInput = InputExt.actions["MousePosition"];
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 mousePosition = mouseInput.ReadValue<Vector2>();
        //float maxDim = Screen.width > Screen.height ? Screen.width : Screen.height;

        position = Vector3.Lerp(position, (mousePosition - screenCenter) * strength, lerpSmooth * Time.deltaTime);
        transform.localPosition = position;
    }
}
