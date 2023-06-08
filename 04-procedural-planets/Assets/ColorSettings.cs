using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ColorSettings : ScriptableObject
{
    public Material planetMaterial;
    public BiomeColorSettings biomeColorSettings;

    [System.Serializable]
    public class BiomeColorSettings
    {
        public Biome[] biomes;

        [System.Serializable]
        public class Biome
        {
            public Gradient gradient;
            public Color tint;

            [Range(0, 1)]
            public float startHeight;

            [Range(0, 1)]
            public float tintPercent;
        }
    }
}
