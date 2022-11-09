using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionToastController : MonoBehaviour
{
    public float maxTime = 5f;
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime > maxTime)
        {
            Destroy(gameObject);
        }
    }
}
