using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoAtmosphere : IPlanet {
   
    [SerializeField] Color ColorLand1 = ColorUtil.FromRGB("#A3A7C2");
    [SerializeField] Color ColorLand2 = ColorUtil.FromRGB("#4C6885");
    [SerializeField] Color ColorLand3 = ColorUtil.FromRGB("#3A3F5E");

    [SerializeField] Color ColorCrater1 = ColorUtil.FromRGB("#4C6885");
    [SerializeField] Color ColorCrater2 = ColorUtil.FromRGB("#3A3F5E");
    

    [SerializeField] GameObject Land;
    [SerializeField] GameObject Craters;
    Material LandMat;
    Material CratersMat;

    void Start()
    {
        LandMat = Land.GetComponent<SpriteRenderer>().material;
        CratersMat = Craters.GetComponent<SpriteRenderer>().material;
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

        //SetSeed((float)val);
        SetSeed(Random.Range(0, 10));

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
        LandMat.SetFloat(ShaderProperties.Key_Pixels, amount);
        CratersMat.SetFloat(ShaderProperties.Key_Pixels, amount);
    }

    public void SetLight(Vector2 pos)
    {
        LandMat.SetVector(ShaderProperties.Key_Light_origin, pos);
        CratersMat.SetVector(ShaderProperties.Key_Light_origin, pos);
    }

    public void SetSeed(float seed)
    {
        LandMat.SetFloat(ShaderProperties.Key_Seed, seed);
        CratersMat.SetFloat(ShaderProperties.Key_Seed, seed);
    }

    public void SetRotate(float r)
    {
        LandMat.SetFloat(ShaderProperties.Key_Rotation, r);
        CratersMat.SetFloat(ShaderProperties.Key_Rotation, r);
    }

    public void UpdateTime(float time)
    {
        LandMat.SetFloat(ShaderProperties.Key_time, time  * 0.5f);
        CratersMat.SetFloat(ShaderProperties.Key_time, time  * 0.5f);
    }


    public void UpdateColor()
    {
        LandMat.SetColor(ShaderProperties.Key_Color1, ColorLand1);
        LandMat.SetColor(ShaderProperties.Key_Color2, ColorLand2);
        LandMat.SetColor(ShaderProperties.Key_Color3, ColorLand3);

        CratersMat.SetColor(ShaderProperties.Key_Color1, ColorCrater1);
        CratersMat.SetColor(ShaderProperties.Key_Color2, ColorCrater2);
    }

    public override void UpdateViaEditor()
    {
        Initialize();
    }
}