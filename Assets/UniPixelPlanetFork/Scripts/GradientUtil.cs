using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Generation.Planets
{
    public static class GradientUtil
    {

        public static GradientColorKey[] GenerateColorKey(string[] colors, float[] color_times)
        {
            var colorKey = new GradientColorKey[colors.Length];
            

            for (int i = 0; i < colorKey.Length; i++)
            {
                colorKey[i].color = default;
                ColorUtility.TryParseHtmlString(colors[i], out colorKey[i].color);
                colorKey[i].time = color_times[i];
            }

            return colorKey;
        }

        public static Texture2D GenerateShaderTex(Color[] colors, float[] color_times)
        {
            var colorKey = new GradientColorKey[colors.Length];
            var alphaKey = new GradientAlphaKey[colors.Length];

            for (int i = 0; i < colorKey.Length; i++)
            {
                colorKey[i].color = colors[i];
                colorKey[i].time = color_times[i];
                alphaKey[i].alpha = 1.0f;
                alphaKey[i].time = color_times[i];
            }

            return GenerateShaderTex(colorKey, alphaKey);
        }

        public static Texture2D GenerateShaderTex(string[] colors, float[] color_times)
        {
            var colorKey = new GradientColorKey[colors.Length];
            var alphaKey = new GradientAlphaKey[colors.Length];

            for (int i = 0; i < colorKey.Length; i++)
            {
                colorKey[i].color = default;
                ColorUtility.TryParseHtmlString(colors[i], out colorKey[i].color);

                colorKey[i].time = color_times[i];
                alphaKey[i].alpha = 1.0f;
                alphaKey[i].time = color_times[i];
            }

            return GenerateShaderTex(colorKey, alphaKey);
        }

        public static Gradient GetGradient(Color[] colors, float[] color_times)
        {
            var colorKey = new GradientColorKey[colors.Length];
            var alphaKey = new GradientAlphaKey[colors.Length];

            for (int i = 0; i < colorKey.Length; i++)
            {
                colorKey[i].color = colors[i];
                colorKey[i].time = color_times[i];
                alphaKey[i].alpha = 1.0f;
                alphaKey[i].time = color_times[i];
            }

            var g = new Gradient();
            g.SetKeys(colorKey, alphaKey);

            return g;
        }

        public static Texture2D GenerateShaderTex(GradientColorKey[] colorkey, GradientAlphaKey[] alphakey)
        {
            var g = new Gradient();
            g.SetKeys(colorkey, alphakey);

            return CreateTexture(g);
        }

        public static Texture2D CreateTexture(Gradient g)
        {
            Texture2D texture = new Texture2D(128, 1);
            for (int h = 0; h < texture.height; h++)
            {
                for (int w = 0; w < texture.width; w++)
                {
                    texture.SetPixel(w, h, g.Evaluate((float)w / texture.width));
                }
            }

            texture.Apply();
            texture.wrapMode = TextureWrapMode.Clamp;
            return texture;
        }
    }
}
