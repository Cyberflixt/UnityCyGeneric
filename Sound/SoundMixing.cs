using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixing : MonoBehaviour
{
    [SerializeField] private AudioMixerSnapshot snapshotNormal;
    [SerializeField] private AudioMixerSnapshot snapshotDowned;

    public static SoundMixing instance;

    private void Awake()
    {
        instance = this;
        TransitionToNormal(0);
    }

    public static void TransitionToNormal(float duration = .5f){
        instance.snapshotNormal.TransitionTo(duration);
    }
    public static void TransitionToDowned(float duration = .5f){
        instance.snapshotDowned.TransitionTo(duration);
    }
}
