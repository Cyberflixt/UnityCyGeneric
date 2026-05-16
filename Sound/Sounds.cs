using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundsType
{
    SFX,
    Music,
}

public class Sounds : MonoBehaviour
{
    // Singleton
    public static Sounds instance;
    [SerializeField] private Transform prefab_Sound3D;
    [SerializeField] private Transform prefab_SoundFlat;
    [SerializeField] private AudioMixerGroup[] mixersArray;

    public static float mainVolume = 0.2f;
    public static Dictionary<string, AudioClip> audios = new Dictionary<string, AudioClip>();
    private static Dictionary<string, int> audiosArrayMax = new Dictionary<string, int>();


    // Events
    public void Start(){
        // Singleton
        if (instance){
            Destroy(gameObject);
            return;
        } else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        Preload();
    }

    private static void Preload()
    {
        // Preload
        AudioClip[] allAudios = Resources.LoadAll<AudioClip>("Audio");
        foreach(AudioClip audio in allAudios){
            audios[audio.name] = audio;
            
            // Save the max number at the end
            // 1. Remove numbers at the end
            string prefix = audio.name;
            int num = 0;
            int factor = 1;
            bool hasSuffix = false;
            while (prefix[prefix.Length-1] >= '0' && prefix[prefix.Length-1] <= '9'){
                num += (prefix[prefix.Length-1]-'0') * factor;
                prefix = prefix.Substring(0, prefix.Length-2);
                hasSuffix = true;
            }

            // Has a number at the end?
            if (hasSuffix){
                // Save max
                if (audiosArrayMax.ContainsKey(prefix)){
                    if (audiosArrayMax[prefix] < num)
                        audiosArrayMax[prefix] = num;
                } else {
                    audiosArrayMax[prefix] = num;
                }
            }
        }
    }

    private static int GetAudioPrefixMax(string audioName){
        int max = -1;
        while (audios.ContainsKey(audioName + (max + 1)))
        {
            max++;
        }
        return max;
    }

    /// <summary>
    /// Get AudioClip by name
    /// </summary>
    /// <param name="audioName">Name of the audio clip (random prefix will be added if not found)</param>
    public static AudioClip GetAudioClip(string audioName){
        if (audios.ContainsKey(audioName))
            return audios[audioName];

        int max = GetAudioPrefixMax(audioName);
        if (max > -1){
            return audios[audioName + UnityEngine.Random.Range(0, max)];
        }

        return null;
    }

    public static AudioSource SetAudioClip(SoundsType ty, Transform parent, AudioClip audioClip, float volume = 1, float randomPitch = 0){
        AudioSource audioSource = parent.GetComponent<AudioSource>();

        // Set audio and volume
        audioSource.outputAudioMixerGroup = instance.mixersArray[(int)ty];
        audioSource.clip = audioClip;
        audioSource.volume = volume * mainVolume;
        if (randomPitch != 0)
            audioSource.pitch += UnityEngine.Random.Range(-randomPitch, randomPitch);

        return audioSource;
    }

    public static void PlayAudioOneshot(AudioSource audioSource){
        // Play sound
        audioSource.Play();

        // Delete after time
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    /// <summary>
    /// Will search for a audio file named audioName and play it at position and at a volume
    /// </summary>
    /// <param name="audioName">Name of the audio file to search (Audio file must be in a "Resources" folder)</param>
    /// <param name="position">3D position of the audio</param>
    /// <param name="volume">volume, default = 1</param>
    public static AudioSource PlayAudio(SoundsType ty, string audioName, Vector3 position, float volume = 1, float randomPitch = 0){
        AudioClip audioClip = GetAudioClip(audioName);
        return PlayAudio(ty, audioClip, position, volume, randomPitch);
    }

    /// <summary>
    /// Will search for a audio file named audioName and play it at position and at a volume
    /// </summary>
    /// <param name="audioClip">AudioClip to play</param>
    /// <param name="position">3D position of the audio</param>
    /// <param name="volume">volume, default = 1</param>
    public static AudioSource PlayAudio(SoundsType ty, AudioClip audioClip, Vector3 position, float volume = 1, float randomPitch = 0){
        if (audioClip == null)
            throw new ArgumentException($"Tried to play a null AudioClip!");
        if (instance == null)
            return null;

        Transform audioSource = Instantiate(instance.prefab_Sound3D, position, Quaternion.identity);
        AudioSource src = SetAudioClip(ty, audioSource, audioClip, volume, randomPitch);
        PlayAudioOneshot(src);
        return src;
    }
    
    /// <summary>
    /// Play 2D AudioClip
    /// </summary>
    public static AudioSource PlayAudioFlat(SoundsType ty, AudioClip audioClip, float volume = 1, float randomPitch = 0){
        if (audioClip == null)
            throw new ArgumentException($"Tried to play a null AudioClip!");

        Transform audioSource = Instantiate(instance.prefab_SoundFlat, Vector3.zero, Quaternion.identity);
        AudioSource src = SetAudioClip(ty, audioSource, audioClip, volume, randomPitch);
        PlayAudioOneshot(src);
        return src;
    }
    /// <summary>
    /// Play 2D sound by name
    /// </summary>
    public static AudioSource PlayAudioFlat(SoundsType ty, string audioName, float volume = 1, float randomPitch = 0){
        AudioClip audioClip = GetAudioClip(audioName);
        return PlayAudioFlat(ty, audioClip, volume, randomPitch);
    }

    /// <summary>
    /// Create 2D sound by AudioClip
    /// </summary>
    public static AudioSource CreateAudioFlat(SoundsType ty, AudioClip audioClip, float volume = 1, float randomPitch = 0){
        Transform audioSource = Instantiate(instance.prefab_SoundFlat, Vector3.zero, Quaternion.identity);
        AudioSource src = SetAudioClip(ty, audioSource, audioClip, volume, randomPitch);
        return src;
    }
    /// <summary>
    /// Create 2D sound by name
    /// </summary>
    public static AudioSource CreateAudioFlat(SoundsType ty, string audioName, float volume = 1, float randomPitch = 0){
        AudioClip audioClip = GetAudioClip(audioName);
        return CreateAudioFlat(ty, audioClip, volume, randomPitch);
    }

    /// <summary>
    /// Play AudioClip, parented to a transform
    /// </summary>
    public static AudioSource PlayAudioAttach(SoundsType ty, AudioClip audioClip, Transform transform, float volume = 1, float randomPitch = 0){
        Transform audioSource = Instantiate(instance.prefab_Sound3D, transform);
        AudioSource src = SetAudioClip(ty, audioSource, audioClip, volume, randomPitch);
        PlayAudioOneshot(src);
        return src;
    }

    /// <summary>
    /// Play audio by given name, parented to a transform
    /// </summary>
    public static AudioSource PlayAudioAttach(SoundsType ty, string audioName, Transform transform, float volume = 1, float randomPitch = 0){
        AudioClip audioClip = GetAudioClip(audioName);
        return PlayAudioAttach(ty, audioClip, transform, volume, randomPitch);
    }


    /// <summary>
    /// Play multiple unique audios of a range
    /// Eg: 3, "Fire" -> Fire2 Fire3 Fire4 (random choice)
    /// </summary>
    /// <param name="max">Number of unique sounds to play</param>
    /// <param name="audioName">Sounds prefix (eg: "Hit" -> "Hit0", "Hit1", etc.)</param>
    public static void PlayAudioRandomRange(SoundsType ty, int max, string audioName, Vector3 position, float volume = 1){
        int audiosMax = GetAudioPrefixMax(audioName);

        int baseI = UnityEngine.Random.Range(0,audiosMax);
        for (int i = 0; i < max; i++){
            PlayAudio(ty, "debrisConcrete" + (baseI+i)%(audiosMax+1), position);
        }
    }
}
