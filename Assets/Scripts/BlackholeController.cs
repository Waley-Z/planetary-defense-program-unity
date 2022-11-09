using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeController : MonoBehaviour
{
    private void Update()
    {
        if (transform.localScale.x < 0.1)
        {
            Destroy(gameObject);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Moon"))
        {
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Asteroid"))
        {
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Blackhole"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
