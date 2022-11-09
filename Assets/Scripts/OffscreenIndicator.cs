using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffscreenIndicator : MonoBehaviour
{
    private GameObject indicatorPrefab;
    private GameObject indicator;
    private Subscription<DeathEvent> death_event_subscription;
    private Orbit orbit;

    void Start()
    {
        indicatorPrefab = GameAssets.GetPrefab("EnemyIndicator");
        indicator = Instantiate(indicatorPrefab, GameObject.Find("EnemyIndicators").transform);
        indicator.SetActive(false);
        death_event_subscription = EventBus.Subscribe<DeathEvent>(_OnDeathEvent);
        orbit = GetComponent<Orbit>();
    }

    // Update is called once per frame
    void Update()
    {
        if (orbit.isOrbit)
        {
            if (indicator.activeSelf)
                indicator.SetActive(false);
            return;
        }

        if (!Utils.IfPositionVisible(transform.position))
        {
            if (!indicator.activeSelf)
                indicator.SetActive(true);

            Vector2 dir = Camera.main.transform.position - transform.position;
            RaycastHit2D ray = Physics2D.Raycast(transform.position, dir);

            if (ray.collider)
            {
                indicator.transform.position = ray.point;

                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
                indicator.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                //indicator.transform.localScale 
            }
            
        }
        else
        {
            if (indicator.activeSelf)
                indicator.SetActive(false);
        }
    }

    void _OnDeathEvent(DeathEvent e)
    {
        if (e.deadGameObject == gameObject)
        {
            Destroy(indicator);
        }
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe(death_event_subscription);
        if (indicator != null)
            Destroy(indicator);
    }
}
