using UnityEngine;

public class SoundFootstepsDistance
{
    private readonly Transform transform;
    private readonly Vector3 offset;
    private readonly float stepSize;
    private const float volume = 0.3f;

    private float distance = 0;
    private Vector3 position;

    public SoundFootstepsDistance(Transform transform, float stepSize = 2f)
    {
        this.stepSize = stepSize;
        this.transform = transform;
        offset = Vector3.zero;

        position = transform.position;
    }

    public SoundFootstepsDistance(Transform transform, float stepSize, Vector3 offset)
    {
        this.offset = offset;
        this.stepSize = stepSize;
        this.transform = transform;

        position = transform.position;
    }

    public void Update(bool isGrounded = true)
    {
        if (!isGrounded)
            return;

        Vector3 oldPos = position;
        position = transform.position;

        // Add moved distance
        float add = (transform.position - oldPos).magnitude;
        distance += add;

        // Distance too high: Teleported?
        if (distance > stepSize * 2)
            distance = stepSize * 1.5f;

        // Moved distance is a full step?
        if (distance > stepSize)
        {
            distance -= stepSize;
            Footstep();
        }
    }

    public void Footstep()
    {
        // Get material
        string material = "Concrete"; // Fallback

        // Raycast down
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
        {
            if (hit.collider.transform.TryGetComponent(out ObjectData data))
                material = data.material.ToString();

            Sounds.PlayAudio(SoundsType.SFX, "footsteps" + material, transform.position + offset, volume);
        }
    }
    public void ResetDistance()
    {
        distance = 0;
    }
}
