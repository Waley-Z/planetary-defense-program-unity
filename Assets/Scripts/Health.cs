using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 1f;
    [SerializeField] private float health;

    private void Start()
    {
        health = maxHealth;
    }

    public void changeHealth(float deltaHealth)
    {
        health += deltaHealth;
        if (health <= 0)
        {
            EventBus.Publish(new DeathEvent(gameObject));
        }
    }

    public float getHealthPercentage()
    {
        return health / maxHealth;
    }

    public float getHealth()
    {
        return health;
    }
}

public class DeathEvent
{
    public GameObject deadGameObject;

    public DeathEvent(GameObject _deadGameObject) { deadGameObject = _deadGameObject; }
}