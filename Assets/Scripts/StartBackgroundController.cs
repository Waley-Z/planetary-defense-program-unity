using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBackgroundController : MonoBehaviour
{
    public float speed = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.renderQueue = 2000;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += speed * (new Vector3(-1, -1, 0)) * Time.fixedDeltaTime;
    }
}
