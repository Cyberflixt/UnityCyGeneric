using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// Component to play sound on rigid body collisions automatically
/// </summary>
[RequireComponent(typeof(ObjectData))]
public class SoundCollider : MonoBehaviour
{
    private SoundColliderInstance soundCollider;

    private const float smallMass = 1;
    private const float maxVolume = 2f;
    private const float thresholdMin = .000005f; // minimum force
    private const float thresholdLight = .4f; // maximum force

    private void Awake()
    {
        ObjectData data = GetComponent<ObjectData>();
        soundCollider = new SoundColliderInstance(data.material, GetComponent<Rigidbody>());
    }

    private void OnCollisionEnter(Collision collision)
    {
        soundCollider.CollisionSound(collision);
    }

    private static (AudioClip, string) GetSoundName(ObjectMaterial material, Rigidbody rb = null)
    {
        // Get sound name using material and weight
        bool small = false;
        if (rb != null)
            small = rb.mass <= smallMass;

        // Return name
        string name = "Collision" + material.ToString();
        if (small)
        {
            // Small
            AudioClip audio = Sounds.GetAudioClip(name + "Small");
            if (audio)
                return (audio, name + "Small");
        }

        // Large
        return (Sounds.GetAudioClip(name), name);
    }

    public static void PlayHitSound(ObjectMaterial material, Rigidbody rigidbody)
    {
        // Get audio name
        (AudioClip clip, string audioName) = GetSoundName(material, rigidbody);

        // Play audio!
        if (clip)
            Sounds.PlayAudio(SoundsType.SFX, clip, rigidbody.transform.TransformPoint(rigidbody.centerOfMass), maxVolume);
        else // Sound doesn't exist
            throw new Exception($"Sound \"{audioName}\" or its collisions variations were not found!");
    }

    public static void PlayHitSound(ObjectMaterial material, Vector3 position)
    {
        // Get audio name
        (AudioClip clip, string audioName) = GetSoundName(material, null);

        // Play audio!
        if (clip)
            Sounds.PlayAudio(SoundsType.SFX, clip, position, maxVolume);
        else // Sound doesn't exist
            throw new Exception($"Sound \"{audioName}\" or its collisions variations were not found!");
    }

    public static bool PlayCollisionSoundByForce(float force, Vector3 position, ObjectMaterial material, Rigidbody rigidbody = null)
    {
        if (force < thresholdMin)
            return false;

        // Get audio name
        (AudioClip clip, string audioName) = GetSoundName(material, rigidbody);
        if (force < thresholdLight)
        {
            // Get "Light" variation if it exists
            AudioClip found = Sounds.GetAudioClip(audioName + "Light");
            if (found) clip = found;
        }

        // Play audio!
        float volume = Mathf.Clamp01(force * .3f) * maxVolume;
        if (clip)
            Sounds.PlayAudio(SoundsType.SFX, clip, position, volume);
        else // Sound doesn't exist
            throw new Exception($"Sound \"{audioName}\" or its collisions variations were not found!");
        return true;
    }
}

public class SoundColliderInstance
{
    [SerializeField] public ObjectMaterial material = ObjectMaterial.Concrete;

    private float previousTick = 0;
    private Rigidbody rigidbody;
    public SoundColliderInstance(ObjectMaterial material, Rigidbody rigidbody)
    {
        this.material = material;
        this.rigidbody = rigidbody;
        previousTick = 0;
    }

    private const float cooldown = .2f; // cooldown between audio

    public void CollisionSound(Collision collision)
    {
        // Cooldown over?
        if (Time.time - previousTick > cooldown)
        {
            // Is force strong enough to trigger sound?
            float force = collision.impulse.magnitude;
            if (SoundCollider.PlayCollisionSoundByForce(force, collision.contacts[0].point, material, rigidbody))
            {
                previousTick = Time.time;
            }
        }
    }
}