using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamBulletController : BulletController
{
    public Transform owner;

    protected override void Start()
    {
        rb = GetComponent<Rigidbody>();
        startTime = Time.time;
        timeBeforeDestroy = 1f;
        if (!owner)
            owner = GameManager.Player.transform;
    }

    protected override void Update()
    {
        GameObject player = GameManager.Player;
        transform.position = owner.position;
        transform.rotation = owner.rotation;

        if (Input.GetMouseButtonUp(0) || !player.GetComponent<Inventory>().useRocks(0.02f))
        {
            Destroy(gameObject);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Asteroid"))
        {
            other.GetComponent<Health>().changeHealth(-damage / 50);
        }
    }




}
