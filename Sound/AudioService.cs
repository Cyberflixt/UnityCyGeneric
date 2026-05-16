using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioService : MonoBehaviour
{
    [SerializeField] private int minimumPooledSound = 50;

    private static readonly Dictionary<int, AudioClip> audioClipByHash = new();

    private static AudioSource flatAudioSource;
    private static AudioSource worldAudioSource;

    private static List<AudioSource> pooledFlatAudioSources = new();
    private static List<AudioSource> pooledWorldAudioSources = new();
    private static int pooledFlatAudioSourcesIndex = 0;
    private static int pooledWorldAudioSourcesIndex = 0;
    private static int pooledFlatAudioSourcesCount = 0;
    private static int pooledWorldAudioSourcesCount = 0;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        GameObject flatAudioSourceObject = new("runtime_flatAudioSource");
        flatAudioSource = flatAudioSourceObject.AddComponent<AudioSource>();
        flatAudioSource.spatialBlend = 0;
        flatAudioSource.playOnAwake = false;

        GameObject worldAudioSourceObject = new("runtime_worldAudioSource");
        worldAudioSource = worldAudioSourceObject.AddComponent<AudioSource>();
        worldAudioSource.spatialBlend = 1;
        worldAudioSource.playOnAwake = false;

        // Create pool
        for (int i = 0; i < minimumPooledSound; i++)
        {
            AudioSource clone = Instantiate(flatAudioSource);
            pooledFlatAudioSources.Add(clone);
            pooledFlatAudioSourcesCount++;

            clone = Instantiate(worldAudioSource);
            pooledWorldAudioSources.Add(clone);
            pooledWorldAudioSourcesCount++;
        }

        // Prehash
        AudioClip[] allAudios = Resources.LoadAll<AudioClip>("Audio");
        foreach(AudioClip audio in allAudios){
            int hash = Animator.StringToHash(audio.name);
            if (audioClipByHash.ContainsKey(hash))
            {
                Debug.LogError("Hash collision detected: \""+audio.name+"\" uses a preexisting hash!");
            }
            audioClipByHash[hash] = audio;
        }
    }

    public static void PlayAudioSourceClip(AudioSource source, AudioClip clip, float randomPitch = 0, float basePitch = 1)
    {
        if (randomPitch == 0)
            source.pitch = basePitch;
        else
            source.pitch = basePitch * (1 + UnityEngine.Random.Range(-100, 100) / 100 * randomPitch);
        source.PlayOneShot(clip, 1);
    }

    public static void PlayAudioFlatFromHash(int hash, float randomPitch = 0, float basePitch = 1)
    {
        if (++pooledFlatAudioSourcesIndex >= pooledFlatAudioSourcesCount)
            pooledFlatAudioSourcesIndex = 0;
        AudioSource src = pooledFlatAudioSources[pooledFlatAudioSourcesIndex];
        if (src.isPlaying)
        {
            Debug.LogWarning("Warning, flat sound poool was not sufficient!");
        }
        PlayAudioSourceClip(src, audioClipByHash[hash], randomPitch, basePitch);
    }

    public static void PlayAudioWorldFromHash(Vector3 position, int hash, float randomPitch = 0, float basePitch = 1)
    {
        if (++pooledWorldAudioSourcesIndex >= pooledWorldAudioSourcesCount)
            pooledWorldAudioSourcesIndex = 0;
        AudioSource src = pooledWorldAudioSources[pooledWorldAudioSourcesIndex];
        if (src.isPlaying)
        {
            Debug.LogWarning("Warning, world sound poool was not sufficient!");
        }
        PlayAudioSourceClip(src, audioClipByHash[hash], randomPitch, basePitch);
    }

    public static void PlayAudioFlatFromName(string name, float randomPitch = 0, float basePitch = 1)
    {
        PlayAudioFlatFromHash(Animator.StringToHash(name), randomPitch, basePitch);
    }

    public static void PlayAudioWorldFromName(Vector3 position, string name, float randomPitch = 0, float basePitch = 1)
    {
        PlayAudioWorldFromHash(position, Animator.StringToHash(name), randomPitch, basePitch);
    }
}
