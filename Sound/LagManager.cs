using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioListener))]
public class LagManager : MonoBehaviour
{
    [Header("Audio Glitch Settings")]
    [Tooltip("The size of the stutter loop in samples (e.g., 2000-5000 gives that classic short buzz/stutter)")]
    [SerializeField] private int loopSampleSize = 3500;

    public static LagManager instance;

    private static bool isLagging = false;
    private float[] audioBuffer;
    private int writeIndex = 0;
    private int readIndex = 0;

    void Update()
    {
        if (Camera.main)
        {
            transform.position = Camera.main.transform.position;
            transform.rotation = Camera.main.transform.rotation;
        }

        // Freeze game
        if (isLagging)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    void Awake()
    {
        instance = this;
        InitializeBuffer();
    }

    private void InitializeBuffer()
    {
        if (audioBuffer == null || audioBuffer.Length != loopSampleSize)
        {
            audioBuffer = new float[loopSampleSize];
            writeIndex = 0;
            readIndex = 0;
        }
    }

    public static void Lag(float duration)
    {
        instance.StartCoroutine(LagCoroutine(duration));
    }

    public static IEnumerator LagCoroutine(float duration)
    {
        isLagging = true;
        AudioListener.pause = true;
        yield return new WaitForSecondsRealtime(duration);
        isLagging = false;
        AudioListener.pause = false;
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (audioBuffer == null)
            InitializeBuffer();

        // Record incoming audio into ring buffer
        if (!isLagging)
        {
            for (int i = 0; i < data.Length; i++)
            {
                audioBuffer[writeIndex] = data[i];
                writeIndex = (writeIndex + 1) % audioBuffer.Length;
            }
            // Sync read pointer to where we left off
            readIndex = writeIndex; 
        }
        else
        {
            // If lagging, overwrite Unity's outgoing data with our short looped ring buffer
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = audioBuffer[readIndex];
                readIndex = (readIndex + 1) % audioBuffer.Length;
            }
        }
    }
}
