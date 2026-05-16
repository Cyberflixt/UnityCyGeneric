using System;
using UnityEngine;

public enum AudioSpectrumUnit
{
    Linear01,
    Decibels,
    QuadOut,
    Exp8Out,
    Exp16Out,
    Exp32Out,
    Exp64Out,
    Exp128Out,
}

public class AudioSpectrumProcessor : MonoBehaviour
{
    // Settings
    public AudioSource audioSource;
    [Range(0f, .25f)]
    public float smoothSpectrum = 0;

    // Runtime
    private const int sampleCount = 2048; //4096
    private float[] spectrumData = new float[sampleCount];
    private float[] smoothedData = new float[sampleCount];
    private FFTWindow fFTWindow = FFTWindow.Triangle;


    private bool updated = false;
    private bool updatedLoudness = false;

    private void Update()
    {
        updated = false;
        updatedLoudness = false;
    }

    private float SmoothFunction(float x)
    {
        // -1 -> 0
        //  0 -> 1
        //  1 -> 0
        return (1 - x) * (x + 1);
    }

    private void UpdateData()
    {
        if (updated)
            return;
        updated = true;
        audioSource.GetSpectrumData(spectrumData, 0, fFTWindow);



        if (smoothSpectrum > 0)
        {
            int neighborsSide = (int)(smoothSpectrum * sampleCount);
            int averageTotal = neighborsSide * 2 + 1;

            for (int i = 0; i < sampleCount; i++)
            {
                float sum = 0;
                for (int j = i - neighborsSide; j <= i + neighborsSide; j++)
                {
                    float factor = SmoothFunction((j - i) / neighborsSide);
                    if (j < 0)
                        sum += spectrumData[0] * factor;
                    else if (j >= sampleCount)
                        sum += spectrumData[sampleCount - 1] * factor;
                    else
                        sum += spectrumData[j] * factor;
                }
                smoothedData[i] = sum / averageTotal;
            }

            (smoothedData, spectrumData) = (spectrumData, smoothedData);
        }
    }

    public float[] GetSpectrumData()
    {
        UpdateData();
        return spectrumData;
    }


    float loudness = 0;
    private void UpdateLoudness()
    {
        if (updatedLoudness)
            return;
        if (!updated)
            UpdateData();
        updatedLoudness = true;

        float sum = 0f;
        for (int i = 0; i < spectrumData.Length; i++)
            sum += spectrumData[i] * spectrumData[i];
        loudness = Mathf.Sqrt(sum / spectrumData.Length);


    }

    public float GetLoudness()
    {
        UpdateLoudness();
        return loudness;
    }

    public static float ConvertAudioUnit(float x, AudioSpectrumUnit unit)
    {
        switch (unit)
        {
            case AudioSpectrumUnit.Linear01:
                return x;
            case AudioSpectrumUnit.Decibels:
                return -Mathf.Log10(x) / 200;
            case AudioSpectrumUnit.QuadOut:
                return 1 - (1 - x) * (1 - x);
            case AudioSpectrumUnit.Exp8Out:
                return 1 - Mathf.Pow(1 - x, 8);
            case AudioSpectrumUnit.Exp16Out:
                return 1 - Mathf.Pow(1 - x, 16);
            case AudioSpectrumUnit.Exp32Out:
                return 1 - Mathf.Pow(1 - x, 32);
            case AudioSpectrumUnit.Exp64Out:
                return 1 - Mathf.Pow(1 - x, 64);
            case AudioSpectrumUnit.Exp128Out:
                return 1 - Mathf.Pow(1 - x, 128);
            default:
                return x;
        }
    }

    public static bool UseRandomData(AudioSpectrumProcessor processor)
    {
        return !Application.isPlaying || !processor;
    }

    public static float[] GetSpectrumData(AudioSpectrumProcessor processor, out bool isRandom)
    {
        isRandom = UseRandomData(processor);
        if (isRandom)
        {
            float[] spectrumData = new float[sampleCount];
            FillRandomSpectrum(spectrumData);
            return spectrumData;
        }

        return processor.GetSpectrumData();
    }

    public static void FillRandomSpectrum(float[] samples)
    {
        for (int i = 0; i < samples.Length; i++)
        {
            samples[i] = Mathf.Pow(UnityEngine.Random.Range(0, 1f), 2);
        }
    }
}
