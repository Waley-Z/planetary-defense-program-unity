using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class LandMasses : IPlanet {

    [SerializeField] Color ColorLand1 = ColorUtil.FromRGB("#C8D45D");
    [SerializeField] Color ColorLand2 = ColorUtil.FromRGB("#63AB3F");
    [SerializeField] Color ColorLand3 = ColorUtil.FromRGB("#2F5753");
    [SerializeField] Color ColorLand4 = ColorUtil.FromRGB("#283540");

    [SerializeField] Color ColorWater1 = ColorUtil.FromRGB("#92E8C0");
    [SerializeField] Color ColorWater2 = ColorUtil.FromRGB("#4FA4B8");
    [SerializeField] Color ColorWater3 = ColorUtil.FromRGB("#2C354D");

    [SerializeField] Color ColorCloud1 = ColorUtil.FromRGB("#DFE0E8");
    [SerializeField] Color ColorCloud2 = ColorUtil.FromRGB("#A3A7C2");
    [SerializeField] Color ColorCloud3 = ColorUtil.FromRGB("#686F99");
    [SerializeField] Color ColorCloud4 = ColorUtil.FromRGB("#404973");

    [SerializeField] GameObject Water;
    [SerializeField] GameObject Land;
    [SerializeField] GameObject Cloud;
    Material WaterMat;
    Material LandMat;
    Material CloudsMat;

    void Start()
    {
        WaterMat = Water.GetComponent<SpriteRenderer>().material;
        LandMat = Land.GetComponent<SpriteRenderer>().material;
        CloudsMat = Cloud.GetComponent<SpriteRenderer>().material;
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
        WaterMat.SetFloat(ShaderProperties.Key_Pixels, amount);
        LandMat.SetFloat(ShaderProperties.Key_Pixels, amount);
        CloudsMat.SetFloat(ShaderProperties.Key_Pixels, amount);
    }

    public void SetLight(Vector2 pos)
    {
        WaterMat.SetVector(ShaderProperties.Key_Light_origin, pos);
        LandMat.SetVector(ShaderProperties.Key_Light_origin, pos);
        CloudsMat.SetVector(ShaderProperties.Key_Light_origin, pos);
    }

    public void SetCloudCover(float cover)
    {
        CloudsMat.SetFloat(ShaderProperties.Key_Cloud_cover, cover);
    }

    public void SetSeed(float seed)
    {
        WaterMat.SetFloat(ShaderProperties.Key_Seed, seed);
        LandMat.SetFloat(ShaderProperties.Key_Seed, seed);
        CloudsMat.SetFloat(ShaderProperties.Key_Seed, seed);
    }

    public void SetRotate(float r)
    {
        WaterMat.SetFloat(ShaderProperties.Key_Rotation, r);
        LandMat.SetFloat(ShaderProperties.Key_Rotation, r);
        CloudsMat.SetFloat(ShaderProperties.Key_Rotation, r);
    }

    public void UpdateTime(float time)
    {
        CloudsMat.SetFloat(ShaderProperties.Key_time, time * 0.5f);
        WaterMat.SetFloat(ShaderProperties.Key_time, time );
        LandMat.SetFloat(ShaderProperties.Key_time, time);
    }
    
    public void UpdateColor()
    {
        LandMat.SetColor(ShaderProperties.Key_Color1, ColorLand1);
        LandMat.SetColor(ShaderProperties.Key_Color2, ColorLand2);
        LandMat.SetColor(ShaderProperties.Key_Color3, ColorLand3);
        LandMat.SetColor(ShaderProperties.Key_Color4, ColorLand4);

        WaterMat.SetColor(ShaderProperties.Key_Color1, ColorWater1);
        WaterMat.SetColor(ShaderProperties.Key_Color2, ColorWater2);
        WaterMat.SetColor(ShaderProperties.Key_Color3, ColorWater3);

        CloudsMat.SetColor(ShaderProperties.Key_BaseColor, ColorCloud1);
        CloudsMat.SetColor(ShaderProperties.Key_OutlineColor, ColorCloud2);
        CloudsMat.SetColor(ShaderProperties.Key_ShadowBaseColor, ColorCloud3);
        CloudsMat.SetColor(ShaderProperties.Key_ShadowOutlineColor, ColorCloud4);
    }

    public override void UpdateViaEditor()
    {
        Initialize();
    }
}
