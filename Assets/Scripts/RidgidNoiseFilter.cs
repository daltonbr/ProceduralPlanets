using UnityEngine;

public class RidgidNoiseFilter : INoiseFilter
{
    private NoiseSettings.RidgidNoiseSettings _noiseSettings;
    Noise noise = new Noise();

    public RidgidNoiseFilter(NoiseSettings.RidgidNoiseSettings noiseSettings)
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
        float weight = 1;

        for (int layer = 0; layer < _noiseSettings.numLayers; layer++)
        {
            float v = 1 - Mathf.Abs(noise.Evaluate(point * frequency + _noiseSettings.center));
            v *= v;
            v *= weight;
            weight = Mathf.Clamp01(v * _noiseSettings.weightMultiplier);
            noiseValue = v * amplitude;
            frequency *= _noiseSettings.roughness;
            amplitude *= _noiseSettings.persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - _noiseSettings.minValue);
        return noiseValue * _noiseSettings.strength;
    }
}
