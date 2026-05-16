using UnityEngine;
using System;


[Serializable]
public class SoundVariant
{
    public string sound = "";
    public float volume = 1;
    public float randomPitch = .1f;
    public SoundsType type = SoundsType.SFX;

    public SoundVariant(string sound, float volume = 1, float randomPitch = 0, SoundsType type = SoundsType.SFX){
        this.sound = sound;
        this.volume = volume;
        this.randomPitch = randomPitch;
        this.type = type;
    }

    public void Play(Vector3 position, float volumeFac = 1){
        if (sound != "" && volume > 0)
            Sounds.PlayAudio(type, sound, position, volume * volumeFac, randomPitch);
    }
    public void PlayFlat(float volumeFac = 1){
        if (sound != "" && volume > 0)
            Sounds.PlayAudioFlat(type, sound, volume * volumeFac, randomPitch);
    }
    public void PlayAttach(Transform transform, float volumeFac = 1){
        if (sound != "" && volume > 0)
            Sounds.PlayAudioAttach(type, sound, transform, volume * volumeFac, randomPitch);
    }
}