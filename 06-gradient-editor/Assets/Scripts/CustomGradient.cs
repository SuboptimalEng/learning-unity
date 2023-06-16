using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomGradient
{
    [SerializeField]
    List<ColorKey> keys = new List<ColorKey>();

    public Color Evaluate(float time)
    {
        if (keys.Count == 0)
        {
            return Color.white;
        }

        ColorKey keyLeft = keys[0];
        ColorKey keyRight = keys[keys.Count - 1];

        for (int i = 0; i < keys.Count - 1; i++)
        {
            if (keys[i].Time <= time && keys[i + 1].Time >= time)
            {
                keyLeft = keys[i];
                keyRight = keys[i + 1];
                break;
            }
        }

        float blendTime = Mathf.InverseLerp(keyLeft.Time, keyRight.Time, time);
        return Color.Lerp(keyLeft.Color, keyRight.Color, blendTime);
    }

    public void RemoveKey(int index)
    {
        if (keys.Count >= 2)
        {
            keys.RemoveAt(index);
        }
    }

    public int UpdateKeyTime(int index, float time)
    {
        Color oldColor = keys[index].Color;
        RemoveKey(index);
        return AddKey(oldColor, time);
    }

    public void UpdateKeyColor(int index, Color col)
    {
        keys[index] = new ColorKey(col, keys[index].Time);
    }

    public int AddKey(Color color, float time)
    {
        ColorKey newKey = new ColorKey(color, time);
        for (int i = 0; i < keys.Count; i++)
        {
            if (newKey.Time < keys[i].Time)
            {
                keys.Insert(i, newKey);
                return i;
            }
        }
        keys.Add(newKey);
        return keys.Count - 1;
    }

    public int NumKeys
    {
        get { return keys.Count; }
    }

    public ColorKey GetKey(int i)
    {
        return keys[i];
    }

    public Texture2D GetTexture(int width)
    {
        Texture2D texture = new Texture2D(width, 1);
        Color[] colors = new Color[width];
        for (int i = 0; i < width; i++)
        {
            colors[i] = Evaluate((float)i / (width - 1));
        }
        texture.SetPixels(colors);
        texture.Apply();
        return texture;
    }

    [System.Serializable]
    public struct ColorKey
    {
        [SerializeField]
        Color color;

        [SerializeField]
        float time;

        // public float time { get; private set; }

        public ColorKey(Color color, float time)
        {
            this.color = color;
            this.time = time;
        }

        public Color Color
        {
            get { return color; }
        }

        public float Time
        {
            get { return time; }
        }
    }
}
