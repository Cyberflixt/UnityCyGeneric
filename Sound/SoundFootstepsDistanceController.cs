using UnityEngine;

public class SoundFootstepsDistanceController : MonoBehaviour
{
    public CharacterController characterController;
    public float stepSize = 2f;

    private SoundFootstepsDistance footstepsDistance;

    void Start()
    {
        Vector3 offset = characterController.height / -2f * Vector3.down;
        footstepsDistance = new SoundFootstepsDistance(transform, stepSize, offset);
    }

    void Update()
    {
        footstepsDistance.Update(characterController.isGrounded);
    }
}
