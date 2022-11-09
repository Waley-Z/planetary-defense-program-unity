using Assets.Generation.Planets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole : IPlanet
{

    [SerializeField] GameObject BlackholeObj;
    [SerializeField] GameObject BlackholeRingObj;

    [SerializeField] Color ColorBlack = ColorUtil.FromRGB("#272736");
    [SerializeField] Color ColorHole1 = ColorUtil.FromRGB("#ffffeb");
    [SerializeField] Color ColorHole2 = ColorUtil.FromRGB("#ed7b39");

    [SerializeField] Color ColorRing1 = ColorUtil.FromRGB("#ffffeb");
    [SerializeField] Color ColorRing2 = ColorUtil.FromRGB("#fff540");
    [SerializeField] Color ColorRing3 = ColorUtil.FromRGB("#ffb84a");
    [SerializeField] Color ColorRing4 = ColorUtil.FromRGB("#ed7b39");
    [SerializeField] Color ColorRing5 = ColorUtil.FromRGB("#bd4035");


    [SerializeField] Gradient gra;

    private float[] _color_times = new float[] { 0, 1.0f };
    private float[] _color_times_ring = new float[] { 0, 0.25f, 0.5f, 0.75f, 1.0f };


    Material BlackholeMat;
    Material BlackholeRingMat;

    void Start()
    {
        BlackholeMat = BlackholeObj.GetComponent<SpriteRenderer>().material;
        BlackholeRingMat = BlackholeRingObj.GetComponent<SpriteRenderer>().material;
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
        var rng = new System.Random(Mathf.RoundToInt(Random.Range(0, 1000)));

        var val = rng.NextDouble();
        val = val < 0.1f ? val + 1 : val * 10;
        CalcSeed = (float)val;
        SetSeed((float)val);

        if (GenerateColors)
        {
        }

        UpdateColor();
    }

    public void SetPixel(float amount)
    {
        BlackholeMat.SetFloat(ShaderProperties.Key_Pixels, amount);
    }

    public void SetSeed(float seed)
    {
        BlackholeRingMat.SetFloat(ShaderProperties.Key_Seed, seed);
    }

    public void SetRotate(float r)
    {
        BlackholeRingMat.SetFloat(ShaderProperties.Key_Rotation, r);
    }

    public void UpdateTime(float time)
    {
        BlackholeRingMat.SetFloat(ShaderProperties.Key_time, time * 0.5f);
    }

    public void UpdateColor()
    {
        BlackholeMat.SetColor("_Black_color", ColorBlack);
        var tex1 = GradientUtil.GenerateShaderTex(new Color[] { ColorHole1, ColorHole2 }, _color_times);
        
        BlackholeMat.SetTexture(ShaderProperties.Key_GradientTex, tex1);
        gra = GradientUtil.GetGradient(new Color[] { ColorRing1, ColorRing2, ColorRing3, ColorRing4, ColorRing5 }, _color_times_ring);


        var tex2 = GradientUtil.GenerateShaderTex(new Color[] { ColorRing1, ColorRing2, ColorRing3, ColorRing4, ColorRing5 }, _color_times_ring);

        BlackholeRingMat.SetTexture(ShaderProperties.Key_GradientTex, tex2);
    }

    public override void UpdateViaEditor()
    {
        Initialize();
    }


 
}
