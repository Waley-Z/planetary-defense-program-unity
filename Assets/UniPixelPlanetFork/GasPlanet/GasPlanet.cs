using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GasPlanet : IPlanet {
    
    [SerializeField] Color ColorCloud1_1 = ColorUtil.FromRGB("#3b2027");
    [SerializeField] Color ColorCloud1_2 = ColorUtil.FromRGB("#3b2027");
    [SerializeField] Color ColorCloud1_3 = ColorUtil.FromRGB("#21181b");
    [SerializeField] Color ColorCloud1_4 = ColorUtil.FromRGB("#21181b");

    [SerializeField] Color ColorCloud2_1 = ColorUtil.FromRGB("#f0b541");
    [SerializeField] Color ColorCloud2_2 = ColorUtil.FromRGB("#cf752b");
    [SerializeField] Color ColorCloud2_3 = ColorUtil.FromRGB("#ab5130");
    [SerializeField] Color ColorCloud2_4 = ColorUtil.FromRGB("#7d3833");


    [SerializeField] GameObject Cloud1;
    [SerializeField] GameObject Cloud2;
    Material Cloud1Mat;
    Material Cloud2Mat;

    private void Start()
    {
        Cloud1Mat = Cloud1.GetComponent<SpriteRenderer>().material;
        Cloud2Mat = Cloud2.GetComponent<SpriteRenderer>().material;
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
        SetCloudCoverCloud1(0f);
        // Random.Range(0.28f, 0.5f)
        SetCloudCoverCloud2(((float)rng.NextDouble() * 0.25f) + 0.3f);
        
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
        Cloud1Mat.SetFloat(ShaderProperties.Key_Pixels, amount);
        Cloud2Mat.SetFloat(ShaderProperties.Key_Pixels, amount);
    }

    public void SetLight(Vector2 pos)
    {
        Cloud1Mat.SetVector(ShaderProperties.Key_Light_origin, pos);
        Cloud2Mat.SetVector(ShaderProperties.Key_Light_origin, pos);
    }

    public void SetSeed(float seed)
    {
        Cloud1Mat.SetFloat(ShaderProperties.Key_Seed, seed);
        Cloud2Mat.SetFloat(ShaderProperties.Key_Seed, seed);
    }

    public void SetCloudCoverCloud1(float cover)
    {
        Cloud1Mat.SetFloat(ShaderProperties.Key_Cloud_cover, cover);
    }
    public void SetCloudCoverCloud2(float cover)
    {
        
        Cloud2Mat.SetFloat(ShaderProperties.Key_Cloud_cover, cover);
    }

    public void SetRotate(float r)
    {
        Cloud1Mat.SetFloat(ShaderProperties.Key_Rotation, r);
        Cloud2Mat.SetFloat(ShaderProperties.Key_Rotation, r);
    }

    public void UpdateTime(float time)
    {
        Cloud1Mat.SetFloat(ShaderProperties.Key_time, time * 0.5f);
        Cloud2Mat.SetFloat(ShaderProperties.Key_time, time  * 0.5f);
    }


    public void UpdateColor()
    {
        Cloud1Mat.SetColor(ShaderProperties.Key_BaseColor, ColorCloud1_1);
        Cloud1Mat.SetColor(ShaderProperties.Key_OutlineColor, ColorCloud1_2);
        Cloud1Mat.SetColor(ShaderProperties.Key_ShadowBaseColor, ColorCloud1_3);
        Cloud1Mat.SetColor(ShaderProperties.Key_ShadowOutlineColor, ColorCloud1_4);

        Cloud2Mat.SetColor(ShaderProperties.Key_BaseColor, ColorCloud2_1);
        Cloud2Mat.SetColor(ShaderProperties.Key_OutlineColor, ColorCloud2_2);
        Cloud2Mat.SetColor(ShaderProperties.Key_ShadowBaseColor, ColorCloud2_3);
        Cloud2Mat.SetColor(ShaderProperties.Key_ShadowOutlineColor, ColorCloud2_4);
    }

    public override void UpdateViaEditor()
    {
        Initialize();
    }
}