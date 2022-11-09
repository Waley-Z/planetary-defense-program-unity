using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Asteroid : IPlanet {
    
    [SerializeField] Color Color1 = ColorUtil.FromRGB("#a3a7c2");
    [SerializeField] Color Color2 = ColorUtil.FromRGB("#4c6885");
    [SerializeField] Color Color3 = ColorUtil.FromRGB("#3a3f5e");


    [SerializeField] GameObject AsteroidSprite;

    private Material AsteroidMat;
    
    private string[] init_colors = new string[] {"#a3a7c2", "#4c6885", "#3a3f5e"};

    public bool isDebris = false;
    private int[] asteroidSeeds = new int[] { 1, 4, 6, 7, 8 };
    private int[] debrisSeeds = new int[] { 2, 3, 9 };

    void Start()
    {
        AsteroidMat = AsteroidSprite.GetComponent<SpriteRenderer>().material;

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
        float seed;
        if (isDebris)
            seed = debrisSeeds[Random.Range(0, debrisSeeds.Length - 1)];
        else
            seed = asteroidSeeds[Random.Range(0, asteroidSeeds.Length - 1)];
        SetSeed(seed);

        if (GenerateColors)
        {
            // maybe later
        }

        UpdateColor();
    }

    public void SetPixel(float amount)
    {
        AsteroidMat.SetFloat(ShaderProperties.Key_Pixels, amount);
    }

    public void SetLight(Vector2 pos)
    {
        AsteroidMat.SetVector(ShaderProperties.Key_Light_origin, pos);
    }

    public void SetSeed(float seed)
    {
        AsteroidMat.SetFloat(ShaderProperties.Key_Seed, seed);
    }

    public void SetRotate(float r)
    {
        AsteroidMat.SetFloat(ShaderProperties.Key_Rotation, r);
    }

    public void UpdateTime(float time)
    {
        return;
    }

    public void UpdateColor()
    {
        AsteroidMat.SetColor(ShaderProperties.Key_Color1, Color1);
        AsteroidMat.SetColor(ShaderProperties.Key_Color2, Color2);
        AsteroidMat.SetColor(ShaderProperties.Key_Color3, Color3);
    }

    public override void UpdateViaEditor()
    {
        Initialize();
    }

}
