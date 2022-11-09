using Assets.Generation.Planets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stars : IPlanet {
    [SerializeField] Color ColorBackground = ColorUtil.FromRGB("#ffffe4");

    [SerializeField] Color ColorStar1 = ColorUtil.FromRGB("#f5ffe8");
    [SerializeField] Color ColorStar2 = ColorUtil.FromRGB("#77d6c1");
    [SerializeField] Color ColorStar3 = ColorUtil.FromRGB("#1c92a7");
    [SerializeField] Color ColorStar4 = ColorUtil.FromRGB("#033e5e");

    [SerializeField] Color ColorFlare1 = ColorUtil.FromRGB("#77d6c1");
    [SerializeField] Color ColorFlare2 = ColorUtil.FromRGB("#ffffe4");

    [SerializeField] GameObject StarBackground;
    [SerializeField] GameObject Star;
    [SerializeField] GameObject StarFlares;

    Material StarBackgroundMat;
    Material StarMat;
    Material StarFlaresMat;

    // timing Gradient for the star
    float[] _color_times_star = new float[4] { 0f, 0.33f, 0.66f, 1.0f };
    // timing Gradient for the star flares
    float[] _color_times_star_flares = new float[2] { 0f, 1.0f };

    int[,] color_ranges = new int[,] { { 0, 30 }, { 50, 65 }, { 190, 230 }, { 335, 360 } };

    // Start is called before the first frame update
    void Start()
    {
        StarBackgroundMat = StarBackground.GetComponent<SpriteRenderer>().material;
        StarMat = Star.GetComponent<SpriteRenderer>().material;
        StarFlaresMat = StarFlares.GetComponent<SpriteRenderer>().material;

        Initialize();
    }

    public override void Initialize()
    {
        SetPixel(Pixel);

        var seedInt = Seed.GetHashCode();
        var rng = new System.Random(seedInt);

        var val = rng.NextDouble();
        val = val < 0.1f ? val + 1 : val * 10;
        CalcSeed = (float)val;

        SetSeed((float)val);

        if (GenerateColors)
        {
            var hue1 = ColorUtil.GetRandomHueColorByRanges(rng, color_ranges);
            var hue2 = ColorUtil.GetRandomHueColorByRanges(rng, color_ranges);

            var baseColor1 = Color.HSVToRGB(hue1, 0.5f, 0.85f);
            var baseColor2 = Color.HSVToRGB(hue2, 0.11f, 1f);

            ColorBackground = baseColor2;

            ColorStar1 = baseColor2;
            ColorStar2 = baseColor1;
            ColorStar3 = Color.HSVToRGB(hue1, 0.83f, 0.65f); ;
            ColorStar4 = Color.HSVToRGB(hue1, 1f, 0.4f); ;

            ColorFlare1 = baseColor1;
            ColorFlare2 = baseColor2;
        }


        UpdateColor();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime(Time.time);
    }

    public void UpdateTime(float time)
    {
        StarBackgroundMat.SetFloat(ShaderProperties.Key_time, time);
        StarMat.SetFloat(ShaderProperties.Key_time, time * 0.1f);
        StarFlaresMat.SetFloat(ShaderProperties.Key_time, time);
    }

    public void SetPixel(float amount)
    {
        StarBackgroundMat.SetFloat(ShaderProperties.Key_Pixels, amount * 2);
        StarMat.SetFloat(ShaderProperties.Key_Pixels, amount);
        StarFlaresMat.SetFloat(ShaderProperties.Key_Pixels, amount * 2);
    }

    public void SetLight(Vector2 pos)
    {
        return;
    }

    public void SetSeed(float seed)
    {
        StarBackgroundMat.SetFloat(ShaderProperties.Key_Seed, seed);
        StarMat.SetFloat(ShaderProperties.Key_Seed, seed);
        StarFlaresMat.SetFloat(ShaderProperties.Key_Seed, seed);
    }

    public void SetRotate(float r)
    {
        StarBackgroundMat.SetFloat(ShaderProperties.Key_Rotation, r);
        StarMat.SetFloat(ShaderProperties.Key_Rotation, r);
        StarFlaresMat.SetFloat(ShaderProperties.Key_Rotation, r);
    }

    

    public void UpdateColor()
    {
        StarBackgroundMat.SetColor(ShaderProperties.Key_Color1, ColorBackground);
        StarMat.SetTexture(ShaderProperties.Key_GradientTex, GradientUtil.GenerateShaderTex(new Color[] { ColorStar1, ColorStar2, ColorStar3, ColorStar4 }, _color_times_star));
        StarFlaresMat.SetTexture(ShaderProperties.Key_GradientTex, GradientUtil.GenerateShaderTex(new Color[] { ColorFlare1, ColorFlare2 }, _color_times_star_flares));
    }

    public override void UpdateViaEditor()
    {
        Initialize();
    }

}
