using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletController : MonoBehaviour
{
    public float speed = 20f;
    public float damage;
    protected Rigidbody rb;
    protected float startTime;
    protected float timeBeforeDestroy = 10f;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.up * speed;
        startTime = Time.time;
    }

    protected virtual void Update()
    {
        if (Time.time - startTime > timeBeforeDestroy)
            Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
    }
}
