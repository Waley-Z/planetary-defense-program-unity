using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public Sprite spaceShipIdle;
    public Sprite spaceShipAccelerate;
    public float thruster = .5f;
    public float thrusterDeltaTime = 2f;
    public float speed;
    public float maxSpeed;
    public bool isRotationActive = true;
    public bool isPlayerMoveActive = true;
    private Rigidbody rb;
    private Camera cam;
    private Quaternion mouseAngle;
    private SpriteRenderer sr;
    private float lastIdleTime;

    public void freezeControl()
    {
        isRotationActive = false;
        isPlayerMoveActive = false;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        sr = GetComponent<SpriteRenderer>();
        isRotationActive = true;
        isPlayerMoveActive = true;
    }

    void FixedUpdate()
    {
        if (GameManager.IsStartPage)
            return;
        
        speed = rb.velocity.magnitude;

        if (Input.GetKey(KeyCode.Space) && isPlayerMoveActive)
        {
            Vector3 force = transform.up * thruster;
            if (lastIdleTime - Time.time > thrusterDeltaTime)
                force *= 2;
            rb.AddForce(force, ForceMode.VelocityChange);
            sr.sprite = spaceShipAccelerate;
        }
        else
        {
            sr.sprite = spaceShipIdle;
            lastIdleTime = Time.time;
        }

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }

    private void Update()
    {
        if (GameManager.IsStartPage)
        {
            //Vector3 position = transform.position;
            //float leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
            //float rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
            //float downBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
            //float upBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

            //position.x = Mathf.Clamp(position.x, leftBorder, rightBorder);
            //position.y = Mathf.Clamp(position.y, downBorder, upBorder);

            //transform.position = position;
            return;
        }
        if (GameManager.TutorialStage == 0)
        {
            if ((transform.position - GameManager.Planet.transform.position).magnitude > 3)
            {
                EventBus.Publish(new TutorialStageChangeEvent(1));
            }
        }

        if (isRotationActive)
        {
            Vector3 mouse = Input.mousePosition;
            Vector3 screenPoint = cam.WorldToScreenPoint(transform.localPosition);
            Vector2 offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg - 90;
            mouseAngle = Quaternion.Euler(0f, 0f, angle);
            transform.rotation = mouseAngle;
        }

        if (Input.GetMouseButtonDown(0) && !Utils.IsMouseOverUI && isPlayerMoveActive)
        {
            GetComponent<WeaponController>().fire(gameObject);
        }
    }
}
