using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityTrapBulletController : BulletController
{
    protected override void Start()
    {
        base.Start();
        if (Utils.IfPositionVisible(transform.position))
            CinemachineShake.Instance.ShakeCamera(5, .1f);
    }


    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.CompareTag("Asteroid"))
        {
            //other.GetComponent<Rigidbody>().AddForce(-dir, ForceMode.Impulse);
            //other.GetComponent<Health>().changeHealth(-damage);
            other.GetComponent<AsteroidController>().trapped = true;
            Orbit orbit = other.GetComponent<Orbit>();
            orbit.pivot = GameManager.Planet;
            orbit.orbitSpeed = 200;
            orbit.isOrbit = true;

            Vector3 dir = GetComponent<Rigidbody>().velocity;
            Vector3 normal = Quaternion.Euler(0, 0, 90) * (other.transform.position - GameManager.Planet.transform.position);
            //if (Vector3.Dot(dir.normalized, normal.normalized) < 0)
            //Debug.Log(dir);
            //Debug.Log(normal);
            Debug.Log(Vector3.Dot(dir.normalized, normal.normalized));
            float speedScale = Vector3.Dot(dir.normalized, normal.normalized);
            Debug.Log(speedScale);
            if (speedScale < 0.3f && speedScale >= 0)
            {
                speedScale = 0.3f;
            } else if (speedScale > -0.3f && speedScale <= 0)
            {
                speedScale = -0.3f;
            }
            orbit.orbitSpeed *= speedScale;
            Destroy(gameObject);
        }
    }
}
