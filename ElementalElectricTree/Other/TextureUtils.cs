using System.Collections.Generic;
using UnityEngine;

namespace SRML
{
    public static class TextureUtils
    {
        public static Texture2D CreateRamp(Color a, Color b)
        {
            Texture2D ramp = new Texture2D(128, 32);
            for (int x = 0; x < 128; x++)
            {
                Color curr = Color.Lerp(a, b, (float)x / 127f);
                for (int y = 0; y < 32; y++)
                {
                    ramp.SetPixel(x, y, curr);
                }
            }
            ramp.name = string.Format("generatedTexture-{0}", ramp.GetInstanceID());
            ramp.Apply();
            return ramp;
        }

        public static Texture2D CreateRamp(Color a, Color b, params Color[] others)
        {
            Texture2D ramp = new Texture2D(128, 32);
            List<Color> colors = new List<Color>
            {
                a,
                b
            };
            colors.AddRange(others);
            int stage = Mathf.RoundToInt(128f / (float)(colors.Count - 1));
            for (int x = 0; x < 128; x++)
            {
                Color curr = Color.Lerp(colors[0], colors[1], (float)(x % stage / (stage - 1)));
                bool flag = x % stage == stage - 1;
                if (flag)
                {
                    colors.RemoveAt(0);
                }
                for (int y = 0; y < 32; y++)
                {
                    ramp.SetPixel(x, y, curr);
                }
            }
            ramp.name = string.Format("generatedTexture-{0}", ramp.GetInstanceID());
            ramp.Apply();
            return ramp;
        }

        public static Texture2D CreateRamp(string hexA, string hexB)
        {
            Color a;
            ColorUtility.TryParseHtmlString("#" + hexA.ToUpper(), out a);
            Color b;
            ColorUtility.TryParseHtmlString("#" + hexB.ToUpper(), out b);
            return TextureUtils.CreateRamp(a, b);
        }

        public static Texture2D CreateRamp(string hexA, string hexB, params string[] hexs)
        {
            Color a;
            ColorUtility.TryParseHtmlString("#" + hexA.ToUpper(), out a);
            Color b;
            ColorUtility.TryParseHtmlString("#" + hexB.ToUpper(), out b);
            List<Color> colors = new List<Color>();
            foreach (string hex in hexs)
            {
                Color c;
                ColorUtility.TryParseHtmlString("#" + hex.ToUpper(), out c);
                colors.Add(c);
            }
            return TextureUtils.CreateRamp(a, b, colors.ToArray());
        }
    }
}