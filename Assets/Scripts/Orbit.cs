using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public GameObject pivot;
    public float orbitSpeed;
    public bool isOrbit = false;
    private float angularSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isOrbit && pivot != null)
        {
            angularSpeed = orbitSpeed / (pivot.transform.position - transform.position).magnitude;
            transform.RotateAround(pivot.transform.position, Vector3.forward, angularSpeed * Time.deltaTime);
        }
    }
}
