using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public float speed = 1.0f;
    public float directionError = 0f;
    public bool toPlanet = true;
    public bool trapped = false;
    private Rigidbody rb;
    private GameObject planet;
    private Health health;
    private Orbit orbit;
    private Subscription<DeathEvent> death_event_subscription;
    private GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        planet = GameManager.Planet;
        health = gameObject.GetComponent<Health>();
        rb = GetComponent<Rigidbody>();
        orbit = GetComponent<Orbit>();
        explosionPrefab = GameAssets.GetPrefab("Explosion");
        if (rb == null)
        {
            Debug.LogError("no rb");
            return;
        }
        death_event_subscription = EventBus.Subscribe<DeathEvent>(_OnDeathEvent);

        if (toPlanet)
        {
            Vector3 error = new Vector3(Random.value, Random.value, 0) * directionError;
            rb.velocity = (planet.transform.position - transform.position + error).normalized * speed;
        }
        else
        {
            //rb.velocity = new Vector3((Random.value - 0.5f) * 0.01f, (Random.value - 0.5f) * 0.01f, 0);
            orbit.isOrbit = true;
        }
        rb.angularVelocity = new Vector3(0, 0, Random.value);

    }

    private void OnCollisionEnter(Collision collision)
    {
        Health otherHealth = collision.gameObject.GetComponent<Health>();
        if (collision.gameObject.name == "Background")
            health.changeHealth(-health.getHealth());
        if (otherHealth == null)
            return;
        float relativeVelocity = collision.relativeVelocity.magnitude;
        float damage = Mathf.Max(relativeVelocity, 5);
        if (collision.gameObject == GameManager.Planet)
            damage = health.getHealth();



        //  asteroid
        if (collision.gameObject.GetComponent<AsteroidController>() != null)
        {
            if (orbit.isOrbit)
                otherHealth.changeHealth(-damage);
            else
                otherHealth.changeHealth(-damage / 4);
        } else
        {
            health.changeHealth(-damage); // self damage
            otherHealth.changeHealth(-damage);
        }
    }

    void _OnDeathEvent(DeathEvent e)
    {
        if (e.deadGameObject == gameObject)
        {
            spawnExplosions();
            spawnDebris();
            Destroy(gameObject);
        }
    }
    private void spawnExplosions()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }

    private void spawnDebris()
    {
        GameObject prefab = GameAssets.GetPrefab("AsteroidDebris");
        for (int i = 0; i < Random.Range(1, 4); i++)
        {
            GameObject go = Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe(death_event_subscription);
    }
}
