
using UnityEngine;

public class SoundFootstepsEvents : MonoBehaviour
{
    [SerializeField] private Transform origin = null;
    [SerializeField] private SoundVariant sound;

    void Start()
    {
        if (origin == null) origin = transform;
    }
    void FootstepEvent(float volume = 1)
    {
        sound.Play(origin.position, volume);
    }
}
