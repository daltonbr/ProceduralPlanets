using System;

public static class NoiseFilterFactory
{
    public static INoiseFilter CreateNoiseFilter(NoiseSettings noiseSettings)
    {
        switch (noiseSettings.filterType)
        {
            case NoiseSettings.FilterType.Simple:
                return new SimpleNoiseFilter(noiseSettings.simpleNoiseSettings);
                break;
            case NoiseSettings.FilterType.Ridgid:
                return new RidgidNoiseFilter(noiseSettings.ridgidNoiseSettings);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
