using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoonController : MonoBehaviour
{
    public int weaponNum = -1;
    public float weaponSpeed;
    public bool active = false;
    private Subscription<DeathEvent> death_event_subscription;
    private LineRenderer lr;
    private SphereCollider sc;
    private float lastFire;
    private WeaponController wc;
    private Inventory inv;
    private GameObject shadow;

    private float OverlapRadius = 2f;
    private int asteroidsLayer;

    // Start is called before the first frame update
    void Start()
    {
        death_event_subscription = EventBus.Subscribe<DeathEvent>(_OnDeathEvent);
        lr = GetComponent<LineRenderer>();
        sc = GetComponent<SphereCollider>();
        wc = GameManager.Player.GetComponent<WeaponController>();
        inv = GameManager.Player.GetComponent<Inventory>();
        asteroidsLayer = LayerMask.NameToLayer("Asteroids");
        shadow = new GameObject();
    }

    void _OnDeathEvent(DeathEvent e)
    {
        if (e.deadGameObject == gameObject)
        {
            //spawnExplosions();
            //spawnDebris();
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (!active)
            return;

        shadow.transform.position = transform.position;
        Vector3 mouse = Input.mousePosition;
        Vector3 worldPoint = Utils.ScreenToWorld(mouse);
        Vector2 offset = new Vector2(worldPoint.x - transform.position.x, worldPoint.y - transform.position.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg - 90;
        shadow.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        if (GameManager.IsStartPage)
            return;

        // laser beam
        if (weaponNum == 1)
        {
            if (Input.GetMouseButtonDown(0) && wc.weaponNum == 1 && GameManager.Player.GetComponent<PlayerController>().isPlayerMoveActive)
            {
                GameObject go =Instantiate(wc.weapons[weaponNum].prefab, shadow.transform.position, shadow.transform.rotation);
                go.GetComponent<LaserBeamBulletController>().owner = shadow.transform;
                go.GetComponent<LaserBeamBulletController>().damage *= weaponSpeed;
            }
            return;
        }



        if (Time.time - lastFire < 1 / weaponSpeed || weaponNum == -1)
        {
            return;
        }



        // find closest asteroid
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, OverlapRadius + weaponSpeed, 1 << asteroidsLayer);
        float minimumDistance = Mathf.Infinity;
        Transform nearestAsteroid = null;
        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject.GetComponent<AsteroidController>().trapped)
                continue;

            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < minimumDistance)
            {
                minimumDistance = distance;
                nearestAsteroid = collider.transform;
            }
        }
        if (nearestAsteroid == null)
        {
            return;
        }

        Debug.Log("Nearest Asteroid: " + nearestAsteroid + "; Distance: " + minimumDistance);
        Vector2 dir = nearestAsteroid.position - transform.position;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

        WeaponAbstract weapon = wc.weapons[weaponNum];
        if (weapon.useRocks())
        {
            Instantiate(weapon.prefab, transform.position, rotation);
            lastFire = Time.time;
        }
    }

    void OnMouseOver()
    {
        if (GameManager.IsStartPage)
            return;

        //If your mouse hovers over the GameObject with the script attached, output this message
        Utils.IsMouseOverMoon = true;
        drawCircle(100, sc.radius * transform.localScale.x);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (weaponNum == 0 && inv.useRocks(200 * Mathf.Pow(2, weaponSpeed)))
            {
                checkTutorial();
                ToastManager.ToastErrorMsg("upgraded mechine gun");
                weaponSpeed++;
            }
            else if (weaponNum != 0 && inv.useRocks(200))
            {
                checkTutorial();
                ToastManager.ToastErrorMsg("deployed mechine gun");
                weaponNum = 0;
                weaponSpeed = 1f;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (weaponNum == 1 && inv.useRocks(200 * Mathf.Pow(2, weaponSpeed)))
            {
                checkTutorial();
                ToastManager.ToastErrorMsg("upgraded laser beam");
                weaponSpeed++;
            }
            else if (weaponNum != 1 && inv.useRocks(200))
            {
                checkTutorial();
                ToastManager.ToastErrorMsg("deployed laser beam");
                weaponNum = 1;
                weaponSpeed = 1f;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (weaponNum == 2 && inv.useRocks(200 * Mathf.Pow(2, weaponSpeed)))
            {
                checkTutorial();
                ToastManager.ToastErrorMsg("upgraded gravity trap");
                weaponSpeed++;
            }
            else if (weaponNum != 2 && inv.useRocks(200))
            {
                checkTutorial();
                ToastManager.ToastErrorMsg("deployed gravity trap");
                weaponNum = 2;
                weaponSpeed = 1f;
            }
        }
    }

    private void checkTutorial()
    {
        if (GameManager.TutorialStage == 5)
            EventBus.Publish(new TutorialStageChangeEvent(6));
    }

    void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
        lr.positionCount = 0;
        Utils.IsMouseOverMoon = false;
    }

    void drawCircle(int steps, float radius)
    {
        lr.positionCount = steps;

        for (int currentStep = 0; currentStep < steps; currentStep++)
        {
            float circumferenceProgress = (float) currentStep / steps;
            float currentRadian = circumferenceProgress * 2 * Mathf.PI;
            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);
            float x = xScaled * radius + transform.position.x;
            float y = yScaled * radius + transform.position.y;
            Vector3 currentPosition = new Vector3(x, y, 0);
            lr.SetPosition(currentStep, currentPosition);
        }
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, OverlapRadius + weaponSpeed);
    }
}
