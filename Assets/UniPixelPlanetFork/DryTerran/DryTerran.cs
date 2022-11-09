using Assets.Generation.Planets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DryTerran : IPlanet {
    
    [SerializeField] Color ColorLand1 = ColorUtil.FromRGB("#ff8933");
    [SerializeField] Color ColorLand2 = ColorUtil.FromRGB("#e64539");
    [SerializeField] Color ColorLand3 = ColorUtil.FromRGB("#ad2f45");
    [SerializeField] Color ColorLand4 = ColorUtil.FromRGB("#52333f");
    [SerializeField] Color ColorLand5 = ColorUtil.FromRGB("#3d2936");

    [SerializeField] private GameObject Land;
    
    Material LandMat;

    private float[] _color_times = new float[] { 0, 0.2f, 0.4f, 0.6f, 1.0f };


    void Start()
    {
        LandMat = Land.GetComponent<SpriteRenderer>().material;
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
            // maybe later 

        }

        UpdateColor();
    }

    void Update()
    {
        UpdateTime(Time.time);
    }
    public void SetPixel(float amount)
    {
        LandMat.SetFloat(ShaderProperties.Key_Pixels, amount);
    }

    public void SetLight(Vector2 pos)
    {
        LandMat.SetVector(ShaderProperties.Key_Light_origin, pos);
    }

    public void SetSeed(float seed)
    {
        LandMat.SetFloat(ShaderProperties.Key_Seed, seed);
    }

    public void SetRotate(float r)
    {
        LandMat.SetFloat(ShaderProperties.Key_Rotation, r);
    }

    public void UpdateTime(float time)
    {
        LandMat.SetFloat(ShaderProperties.Key_time, time  );
    }

    public void UpdateColor()
    {
        LandMat.SetTexture(ShaderProperties.Key_GradientTex, GradientUtil.GenerateShaderTex(new Color[] { ColorLand1, ColorLand2, ColorLand3, ColorLand4, ColorLand5 }, _color_times));
    }

    public override void UpdateViaEditor()
    {
        Initialize();
    }


}
