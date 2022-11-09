using Assets.Generation.Planets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Galaxy : IPlanet
{

    [SerializeField] Color Color1 = ColorUtil.FromRGB("#ffffeb");
    [SerializeField] Color Color2 = ColorUtil.FromRGB("#ffe478");
    [SerializeField] Color Color3 = ColorUtil.FromRGB("#8fde5d");
    [SerializeField] Color Color4 = ColorUtil.FromRGB("#3d6e70");
    [SerializeField] Color Color5 = ColorUtil.FromRGB("#323e4f");
    [SerializeField] Color Color6 = ColorUtil.FromRGB("#322947");


    [SerializeField] GameObject Sprite;

    Material GalaxyMat;

    private float[] _color_times = new float[] { 0, 0.2f, 0.4f, 0.6f, 0.8f, 1.0f };

    // Start is called before the first frame update
    void Start()
    {
        GalaxyMat = Sprite.GetComponent<SpriteRenderer>().material;

        Initialize();
    }

    void Update()
    {
        UpdateTime(Time.time);
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
            // maybe later
        }

        UpdateColor();
    }

    public void SetPixel(float amount)
    {
        GalaxyMat.SetFloat(ShaderProperties.Key_Pixels, amount);
    }

    public void SetSeed(float seed)
    {
        GalaxyMat.SetFloat(ShaderProperties.Key_Seed, seed);
    }

    public void SetRotate(float r)
    {
        GalaxyMat.SetFloat(ShaderProperties.Key_Rotation, r);
    }

    public void UpdateTime(float time)
    {
        GalaxyMat.SetFloat(ShaderProperties.Key_time, time * 0.5f);
    }

    public void UpdateColor()
    {
        var tex1 = GradientUtil.GenerateShaderTex(new Color[] { Color1, Color2, Color3, Color4, Color5, Color6 }, _color_times);
        GalaxyMat.SetTexture(ShaderProperties.Key_GradientTex, tex1);
    }


    public override void UpdateViaEditor()
    {
        Initialize();
    }
}
