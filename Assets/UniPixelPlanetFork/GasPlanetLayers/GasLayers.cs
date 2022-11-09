using Assets.Generation.Planets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GasLayers : IPlanet {
  
    [SerializeField] Color Color1 = ColorUtil.FromRGB("#eec39a");
    [SerializeField] Color Color2 = ColorUtil.FromRGB("#d9a066");
    [SerializeField] Color Color3 = ColorUtil.FromRGB("#8f563b");

    [SerializeField] Color ColorDark1 = ColorUtil.FromRGB("#663931");
    [SerializeField] Color ColorDark2 = ColorUtil.FromRGB("#45283c");
    [SerializeField] Color ColorDark3 = ColorUtil.FromRGB("#222034");

    [SerializeField] GameObject GasPlanet;
    [SerializeField] GameObject Ring;
    
    Material GasPlanetMat;
    Material RingMat;

    private float[] _color_times = new float[] { 0, 0.5f, 1.0f };

    private void Start()
    {
        GasPlanetMat = GasPlanet.GetComponent<SpriteRenderer>().material;
        RingMat = Ring.GetComponent<SpriteRenderer>().material;
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
        }

        UpdateColor();
    }

    void Update()
    {
        UpdateTime(Time.time);
    }

    public void SetPixel(float amount)
    {
        GasPlanetMat.SetFloat(ShaderProperties.Key_Pixels, amount);
        RingMat.SetFloat(ShaderProperties.Key_Pixels, amount * 3f);
    }

    public void SetLight(Vector2 pos)
    {
        GasPlanetMat.SetVector(ShaderProperties.Key_Light_origin, pos * 1.3f  );
        RingMat.SetVector(ShaderProperties.Key_Light_origin, pos * 1.3f );
    }

    public void SetSeed(float seed)
    {
        GasPlanetMat.SetFloat(ShaderProperties.Key_Seed, seed);
        RingMat.SetFloat(ShaderProperties.Key_Seed, seed);
    }

    public void SetRotate(float r)
    {
        GasPlanetMat.SetFloat(ShaderProperties.Key_Rotation, r);
        RingMat.SetFloat(ShaderProperties.Key_Rotation, r + 0.7f);
    }

    public void UpdateTime(float time)
    {
        GasPlanetMat.SetFloat(ShaderProperties.Key_time, time * 0.5f);
        RingMat.SetFloat(ShaderProperties.Key_time, time  * 0.5f * -3f);
    }

    public void UpdateColor()
    {

        var tex1 = GradientUtil.GenerateShaderTex(new Color[] { Color1, Color2, Color3 }, _color_times);
        var tex2 = GradientUtil.GenerateShaderTex(new Color[] { ColorDark1, ColorDark2, ColorDark3 }, _color_times);

        GasPlanetMat.SetTexture(ShaderProperties.Key_TextureKeyword1, tex1);
        GasPlanetMat.SetTexture(ShaderProperties.Key_TextureKeyword2, tex2);

        RingMat.SetTexture(ShaderProperties.Key_TextureKeyword1, tex1);
        RingMat.SetTexture(ShaderProperties.Key_TextureKeyword2, tex2);
    }

    public override void UpdateViaEditor()
    {
        Initialize();
    }
}