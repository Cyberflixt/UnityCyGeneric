using UnityEngine;
using System;


[Serializable]
public class AudioClipVariant
{
    public AudioClip sound = null;
    public float volume = 1;
    public float randomPitch = .1f;
    public SoundsType type = SoundsType.SFX;

    public void Play(Vector3 position){
        if (sound != null && volume > 0)
            Sounds.PlayAudio(type, sound, position, volume, randomPitch);
    }
    public void PlayFlat(){
        if (sound != null && volume > 0)
            Sounds.PlayAudioFlat(type, sound, volume, randomPitch);
    }
    public void PlayAttach(Transform transform){
        if (sound != null && volume > 0)
            Sounds.PlayAudioAttach(type, sound, transform, volume, randomPitch);
    }
}