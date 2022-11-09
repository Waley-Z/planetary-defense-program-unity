using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LavaWorld : IPlanet {
    
    [SerializeField] Color ColorLand1 = ColorUtil.FromRGB("#8f4d57");
    [SerializeField] Color ColorLand2 = ColorUtil.FromRGB("#52333f");
    [SerializeField] Color ColorLand3 = ColorUtil.FromRGB("#3d2936");

    [SerializeField] Color ColorCrater1 = ColorUtil.FromRGB("#52333f");
    [SerializeField] Color ColorCrater2 = ColorUtil.FromRGB("#3d2936");

    [SerializeField] Color ColorRiver1 = ColorUtil.FromRGB("#ff8933");
    [SerializeField] Color ColorRiver2 = ColorUtil.FromRGB("#e64539");
    [SerializeField] Color ColorRiver3 = ColorUtil.FromRGB("#ad2f45");

    [SerializeField] GameObject PlanetUnder;
    [SerializeField] GameObject Craters;
    [SerializeField] GameObject LavaRivers;
    Material PlanetMat;
    Material CratersMat;
    Material RiverMat;


    private void Start()
    {
        PlanetMat = PlanetUnder.GetComponent<SpriteRenderer>().material;
        CratersMat = Craters.GetComponent<SpriteRenderer>().material;
        RiverMat = LavaRivers.GetComponent<SpriteRenderer>().material;
        Initialize();
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
        SetRiverCutOff(((float)rng.NextDouble() * 0.25f) + 0.4f);

        if (GenerateColors)
        {

        }

        UpdateColor();
    }

    void Update()
    {
        UpdateTime(Time.time);
    }

    public void SetRiverCutOff(float amount)
    {
        RiverMat.SetFloat(ShaderProperties.Key_Color_River_Cutoff, amount);
    }

    public void SetPixel(float amount)
    {
        PlanetMat.SetFloat(ShaderProperties.Key_Pixels, amount);
        CratersMat.SetFloat(ShaderProperties.Key_Pixels, amount);
        RiverMat.SetFloat(ShaderProperties.Key_Pixels, amount);
    }

    public void SetLight(Vector2 pos)
    {
        PlanetMat.SetVector(ShaderProperties.Key_Light_origin, pos);
        CratersMat.SetVector(ShaderProperties.Key_Light_origin, pos);
        RiverMat.SetVector(ShaderProperties.Key_Light_origin, pos);
    }

    public void SetSeed(float seed)
    {
        PlanetMat.SetFloat(ShaderProperties.Key_Seed, seed);
        CratersMat.SetFloat(ShaderProperties.Key_Seed, seed);
        RiverMat.SetFloat(ShaderProperties.Key_Seed, seed);
    }

    public void SetRotate(float r)
    {
        PlanetMat.SetFloat(ShaderProperties.Key_Rotation, r);
        CratersMat.SetFloat(ShaderProperties.Key_Rotation, r);
        RiverMat.SetFloat(ShaderProperties.Key_Rotation, r);
    }

    public void UpdateTime(float time)
    {
        PlanetMat.SetFloat(ShaderProperties.Key_time, time * 0.5f);
        CratersMat.SetFloat(ShaderProperties.Key_time, time  * 0.5f);
        RiverMat.SetFloat(ShaderProperties.Key_time, time  * 0.5f);
    }

    public void UpdateColor()
    {
        PlanetMat.SetColor(ShaderProperties.Key_Color1, ColorLand1);
        PlanetMat.SetColor(ShaderProperties.Key_Color2, ColorLand2);
        PlanetMat.SetColor(ShaderProperties.Key_Color3, ColorLand3);

        CratersMat.SetColor(ShaderProperties.Key_Color1, ColorCrater1);
        CratersMat.SetColor(ShaderProperties.Key_Color2, ColorCrater2);

        RiverMat.SetColor(ShaderProperties.Key_Color1, ColorRiver1);
        RiverMat.SetColor(ShaderProperties.Key_Color2, ColorRiver2);
        RiverMat.SetColor(ShaderProperties.Key_Color3, ColorRiver3);
    }

    public override void UpdateViaEditor()
    {
        Initialize();
    }
}
