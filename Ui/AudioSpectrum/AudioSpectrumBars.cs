using NUnit.Framework.Constraints;
using UnityEngine;

public class AudioSpectrumBars : MonoBehaviour
{
    public AudioSpectrumProcessor audioProcessor;
    public Transform barsParent;

    [Header("Settings")]
    public float volumeFactor = 50f;
    public float volumeAdd = -.02f;
    public AudioSpectrumUnit audioUnit = AudioSpectrumUnit.Linear01;
    public float unitFactor = 1f;
    [Range(0, 1)] public float minimumFrequency = 0.02f;
    [Range(0, 1)] public float maximumFrequency = 0.05f;
    public Vector3 scaleMinimum = new Vector3(0, .9f, 1);
    public Vector3 scaleMaximum = new Vector3(2, .9f, 1);

    // Runtime
    private Transform[] bars;
    private float[] spectrumData = new float[4096];

    void Start()
    {
        bars = new Transform[barsParent.childCount];
        for (int i = 0; i < bars.Length; i++)
        {
            bars[i] = barsParent.GetChild(i);
        }
    }

    [Button("Display random")]
    public void FillRandomValues()
    {
        Start();
        for (int i = 0; i < spectrumData.Length; i++)
        {
            spectrumData[i] = Mathf.Pow(UnityEngine.Random.Range(0, 1f), 2) / unitFactor;
        }
        UpdateDisplay();
    }

    private void LateUpdate()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        float[] spectrumData = AudioSpectrumProcessor.GetSpectrumData(audioProcessor, out bool isRandom);
        float blockSamples = (maximumFrequency - minimumFrequency) / bars.Length * spectrumData.Length;

        // Set bars scale
        for (int bar_i = 0; bar_i < bars.Length; bar_i++)
        {
            int freq_start_i = (int)(minimumFrequency * spectrumData.Length + blockSamples * bar_i);

            // Get average frequency volume
            float sum = 0;
            for (int i = 0; i < blockSamples; i++)
                sum += spectrumData[freq_start_i + i];
            sum = Mathf.Max(sum / blockSamples, .001f);

            float volume = AudioSpectrumProcessor.ConvertAudioUnit(Mathf.Clamp01(sum * volumeFactor + volumeAdd), audioUnit);
            if (!isRandom)
                volume *= unitFactor;

            bars[bar_i].localScale = Vector3.Lerp(scaleMinimum, scaleMaximum, volume);
        }
    }
}
