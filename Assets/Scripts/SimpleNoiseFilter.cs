using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNoiseFilter : INoiseFilter
{
    private NoiseSettings.SimpleNoiseSettings _noiseSettings;
    Noise noise = new Noise();

    public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings noiseSettings)
    {
        _noiseSettings = noiseSettings;
    }

    public float Evaluate(Vector3 point)
    {
        // transposing the range (-1, 1) to (0, 1)
        //float noiseValue = (noise.Evaluate(point * _noiseSettings.roughness + _noiseSettings.center) + 1) * 0.5f;
        float noiseValue = 0;
        float frequency = _noiseSettings.baseRoughness;
        float amplitude = 1;

        for (int layer = 0; layer < _noiseSettings.numLayers; layer++)
        {
            float v = noise.Evaluate(point * frequency + _noiseSettings.center);
            noiseValue = (v + 1f) * 0.5f * amplitude;
            frequency *= _noiseSettings.roughness;
            amplitude *= _noiseSettings.persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - _noiseSettings.minValue);
        return noiseValue * _noiseSettings.strength;
    }
}

