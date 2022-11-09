using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IceWorld : IPlanet {

    [SerializeField] Color ColorLand1 = ColorUtil.FromRGB("#faffff");
    [SerializeField] Color ColorLand2 = ColorUtil.FromRGB("#c7d4e1");
    [SerializeField] Color ColorLand3 = ColorUtil.FromRGB("#928fb8");

    [SerializeField] Color ColorLakes1 = ColorUtil.FromRGB("#4fa4b8");
    [SerializeField] Color ColorLakes2 = ColorUtil.FromRGB("#4c6885");
    [SerializeField] Color ColorLakes3 = ColorUtil.FromRGB("#3a3f5e");

    [SerializeField] Color ColorCloud1 = ColorUtil.FromRGB("#e1f2ff");
    [SerializeField] Color ColorCloud2 = ColorUtil.FromRGB("#c0e3ff");
    [SerializeField] Color ColorCloud3 = ColorUtil.FromRGB("#5e70a5");
    [SerializeField] Color ColorCloud4 = ColorUtil.FromRGB("#404973");

    [SerializeField] GameObject PlanetUnder;
    [SerializeField] GameObject Lakes;
    [SerializeField] GameObject Clouds;
    Material PlanetUnderMat;
    Material LakesMat;
    Material CloudsMat;
    
    void Start()
    {
        PlanetUnderMat = PlanetUnder.GetComponent<SpriteRenderer>().material;
        LakesMat = Lakes.GetComponent<SpriteRenderer>().material;
        CloudsMat = Clouds.GetComponent<SpriteRenderer>().material;
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
        SetCloudCover(((float)rng.NextDouble() * 0.25f) + 0.4f);
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
        PlanetUnderMat.SetFloat(ShaderProperties.Key_Pixels, amount);
        LakesMat.SetFloat(ShaderProperties.Key_Pixels, amount);
        CloudsMat.SetFloat(ShaderProperties.Key_Pixels, amount);
    }

    public void SetLight(Vector2 pos)
    {
        PlanetUnderMat.SetVector(ShaderProperties.Key_Light_origin, pos);
        LakesMat.SetVector(ShaderProperties.Key_Light_origin, pos);
        CloudsMat.SetVector(ShaderProperties.Key_Light_origin, pos);
    }
    void SetCloudCover(float cover)
    {
        CloudsMat.SetFloat(ShaderProperties.Key_Cloud_cover, cover);
    }

    public void SetSeed(float seed)
    {
        PlanetUnderMat.SetFloat(ShaderProperties.Key_Seed, seed);
        LakesMat.SetFloat(ShaderProperties.Key_Seed, seed);
        CloudsMat.SetFloat(ShaderProperties.Key_Seed, seed);
    }

    public void SetRotate(float r)
    {
        PlanetUnderMat.SetFloat(ShaderProperties.Key_Rotation, r);
        LakesMat.SetFloat(ShaderProperties.Key_Rotation, r);
        CloudsMat.SetFloat(ShaderProperties.Key_Rotation, r);
    }

    public void UpdateTime(float time)
    {
        
        CloudsMat.SetFloat(ShaderProperties.Key_time, time * 0.5f);
        PlanetUnderMat.SetFloat(ShaderProperties.Key_time, time );
        LakesMat.SetFloat(ShaderProperties.Key_time, time);
    }

    public void UpdateColor()
    {
        PlanetUnderMat.SetColor(ShaderProperties.Key_Color1, ColorLand1);
        PlanetUnderMat.SetColor(ShaderProperties.Key_Color2, ColorLand2);
        PlanetUnderMat.SetColor(ShaderProperties.Key_Color3, ColorLand3);

        LakesMat.SetColor(ShaderProperties.Key_Color1, ColorLakes1);
        LakesMat.SetColor(ShaderProperties.Key_Color2, ColorLakes2);
        LakesMat.SetColor(ShaderProperties.Key_Color3, ColorLakes3);

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
