using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class AudioSpectrumTriangles : Graphic
{
    public AudioSpectrumProcessor audioProcessor;

    [Header("L Shape Corner")]
    public Transform topPosition;
    public Transform bottomPosition;
    public Transform bottomSidePosition;

    [Header("Settings")]
    public int points = 20;
    public float volumeFactor = 20f;
    public float volumeAdd = -.02f;
    public AudioSpectrumUnit audioUnit = AudioSpectrumUnit.Linear01;
    public float unitFactor = 1f;
    [Range(0, 1)] public float minimumFrequency = 0.02f;
    [Range(0, 1)] public float maximumFrequency = 0.05f;
    public float distMinimum = 0;
    public float distMaximum = 1;
    public float volumeMaximum = 1;
    public bool spiky = false;
    public bool inverseVolume = false;

    void LateUpdate()
    {
        SetVerticesDirty();
    }

    private static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        float[] spectrumData = AudioSpectrumProcessor.GetSpectrumData(audioProcessor, out bool isRandom);
        float blockSamples = (maximumFrequency - minimumFrequency) / points * spectrumData.Length;

        Vector3 bottom = bottomPosition.localPosition;
        Vector3 top = topPosition.localPosition;
        Vector3 sideOffset = bottomSidePosition.localPosition - bottom;

        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        vh.Clear();

        // Set bars scale
        for (int bar_i = 0; bar_i <= points; bar_i++)
        {
            int freq_start_i = (int)(minimumFrequency * spectrumData.Length + blockSamples * bar_i);

            // Get average frequency volume
            float sum = 0;
            for (int i = 0; i < blockSamples; i++)
                sum += spectrumData[freq_start_i + i];
            sum = Mathf.Max(sum / blockSamples, .001f); // Between ]0, 1]

            // Get volume
            float volume = AudioSpectrumProcessor.ConvertAudioUnit(Mathf.Clamp01(sum * volumeFactor + volumeAdd), audioUnit);
            if (!isRandom)
                volume *= unitFactor;
            if (volume > volumeMaximum)
                volume = volumeMaximum;
            if (inverseVolume)
                volume = volumeMaximum - volume;

            // Get distance
            float distance;
            distance = Lerp(distMinimum, distMaximum, volume);

            Vector3 vertical = Vector3.Lerp(bottom, top, bar_i / (float)points);
            vertex.position = vertical + sideOffset * distMinimum;
            vh.AddVert(vertex);
            vertex.position = vertical + sideOffset * distance;
            vh.AddVert(vertex);

            if (bar_i > 0)
            {
                vh.AddTriangle(bar_i * 2 - 2, bar_i * 2 - 1, bar_i * 2);
                if (spiky)
                    vh.AddTriangle(bar_i * 2 - 2, bar_i * 2, bar_i * 2 + 1);
                else
                    vh.AddTriangle(bar_i * 2 - 1, bar_i * 2, bar_i * 2 + 1);
            }
        }
    }
}
