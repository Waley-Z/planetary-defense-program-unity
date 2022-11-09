using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MachineGunBulletController : BulletController
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.CompareTag("Asteroid"))
        {
            Vector3 dir = GetComponent<Rigidbody>().velocity.normalized;
            other.GetComponent<Rigidbody>().AddForce(dir, ForceMode.Impulse);
            other.GetComponent<Health>().changeHealth(-damage);
            Destroy(gameObject);
        }
    }
}
