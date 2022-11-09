using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LandRivers : IPlanet {
    

    [SerializeField] Color ColorLand1 = ColorUtil.FromRGB("#63AB3F");
    [SerializeField] Color ColorLand2 = ColorUtil.FromRGB("#3B7D4F");
    [SerializeField] Color ColorLand3 = ColorUtil.FromRGB("#2F5753");
    [SerializeField] Color ColorLand4 = ColorUtil.FromRGB("#283540");

    [SerializeField] Color ColorRiver = ColorUtil.FromRGB("#4FA4B8");
    [SerializeField] Color ColorRiverDark = ColorUtil.FromRGB("#404973");

    [SerializeField] Color ColorCloud1 = ColorUtil.FromRGB("#FFFFFF");
    [SerializeField] Color ColorCloud2 = ColorUtil.FromRGB("#DFE0E8");
    [SerializeField] Color ColorCloud3 = ColorUtil.FromRGB("#686F99");
    [SerializeField] Color ColorCloud4 = ColorUtil.FromRGB("#404973");

    [SerializeField] GameObject Land;
    [SerializeField] GameObject Cloud;

    Material LandMat;
    Material CloudMat;

    void Start()
    {
        LandMat = Land.GetComponent<SpriteRenderer>().material;
        CloudMat = Cloud.GetComponent<SpriteRenderer>().material;
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
        // Random.Range(0.35f, 0.6f)
        SetCloudCover(((float)rng.NextDouble() * 0.25f) + 0.35f);
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
        CloudMat.SetFloat(ShaderProperties.Key_Pixels, amount);
    }

    public void SetLight(Vector2 pos)
    {
        LandMat.SetVector(ShaderProperties.Key_Light_origin, pos);
        CloudMat.SetVector(ShaderProperties.Key_Light_origin, pos);
    }

    public void SetCloudCover(float cover)
    {
        CloudMat.SetFloat(ShaderProperties.Key_Cloud_cover, cover);
    }

    public void SetSeed(float seed)
    {
        LandMat.SetFloat(ShaderProperties.Key_Seed, seed);
        CloudMat.SetFloat(ShaderProperties.Key_Seed, seed);
    }

    public void SetRotate(float r)
    {
        LandMat.SetFloat(ShaderProperties.Key_Rotation, r);
        CloudMat.SetFloat(ShaderProperties.Key_Rotation, r);
    }

    public void UpdateTime(float time)
    {
        LandMat.SetFloat(ShaderProperties.Key_time, time * 0.25f);
        CloudMat.SetFloat(ShaderProperties.Key_time, time * 0.5f);
    }

    public void UpdateColor()
    {
        LandMat.SetColor(ShaderProperties.Key_Color1, ColorLand1);
        LandMat.SetColor(ShaderProperties.Key_Color2, ColorLand2);
        LandMat.SetColor(ShaderProperties.Key_Color3, ColorLand3);
        LandMat.SetColor(ShaderProperties.Key_Color4, ColorLand4);

        LandMat.SetColor(ShaderProperties.Key_Color_River, ColorRiver);
        LandMat.SetColor(ShaderProperties.Key_Color_River_Dark, ColorRiverDark);

        CloudMat.SetColor(ShaderProperties.Key_BaseColor, ColorCloud1);
        CloudMat.SetColor(ShaderProperties.Key_OutlineColor, ColorCloud2);
        CloudMat.SetColor(ShaderProperties.Key_ShadowBaseColor, ColorCloud3);
        CloudMat.SetColor(ShaderProperties.Key_ShadowOutlineColor, ColorCloud4);

    }

    public override void UpdateViaEditor()
    {
        Initialize();
    }
}
