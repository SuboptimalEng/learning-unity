using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public enum FilterType
    {
        Simple,
        Ridged
    };

    public FilterType filterType;

    // note: unable to get conditional rendering to hide settings
    // [ConditionalHide("filterType", 0)]
    public SimpleNoiseSettings simpleNoiseSettings;

    // note: unable to get conditional rendering to hide settings
    // [ConditionalHide("filterType", 1)]
    public RidgedNoiseSettings ridgedNoiseSettings;

    [System.Serializable]
    public class SimpleNoiseSettings
    {
        [Range(1, 8)]
        public int numOfLayers = 1;

        public float strength = 1;
        public float baseRoughness = 1;
        public float roughness = 2;
        public float persistence = 0.5f;
        public Vector3 center;
        public float minValue;
    }

    [System.Serializable]
    public class RidgedNoiseSettings : SimpleNoiseSettings
    {
        public float weightMultiplier = 0.8f;
    }
}
