using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.renderQueue = 2000;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += 0.3f * Camera.main.velocity * Time.fixedDeltaTime;
    }
}
