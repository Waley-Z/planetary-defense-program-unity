using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidDebrisController : MonoBehaviour
{
    public float collectRadius = 1f;
    public float slope = 0.05f;
    private Rigidbody rb;
    private GameObject player;
    private bool isChasing;
    private float currentSpeed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 dir = Random.insideUnitCircle.normalized * 2;
        rb.AddForce(dir, ForceMode.VelocityChange);
        player = GameManager.Player;
        isChasing = false;

        var euler = transform.eulerAngles;
        euler.z = Random.Range(0f, 360f);
        transform.eulerAngles = euler;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChasing && (transform.position - player.transform.position).magnitude < collectRadius)
        {
            currentSpeed = rb.velocity.magnitude;
            isChasing = true;
        }
    }

    private void FixedUpdate()
    {
        if (isChasing)
        {
            currentSpeed += slope;
            //rb.AddForce((player.transform.position - transform.position).normalized * 10, ForceMode.Acceleration);
            rb.velocity = currentSpeed * (player.transform.position - transform.position).normalized;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player || other.CompareTag("Blackhole"))
        {
            EventBus.Publish(new DeathEvent(gameObject));
            Destroy(gameObject);
            player.GetComponent<Inventory>().rocks += Random.Range(5, 10);
        }
            
    }
}
