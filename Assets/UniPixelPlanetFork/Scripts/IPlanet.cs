using UnityEngine;

public abstract class IPlanet : MonoBehaviour {

    [SerializeField] public int Pixel = 100;
    [SerializeField] public string Seed = "Seed";
    [SerializeField] public float CalcSeed;
    [SerializeField] public bool GenerateColors = false;

    public abstract void Initialize();

    public abstract void UpdateViaEditor();
}
