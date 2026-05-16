using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ErrorSoundNotifier : MonoBehaviour
{
    private AudioSource audioSource;
    private bool played = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Error || type == LogType.Exception)
        {
            PlayErrorSound();
        }
    }

    private void PlayErrorSound()
    {
        if (!played)
        {
            played = true;
            audioSource.Play();
        }
    }
}
