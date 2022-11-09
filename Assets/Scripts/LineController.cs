using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    public GameObject a1;
    public GameObject a2;
    public float maxDistance;
    //public bool isCaptured = false;
    public bool isPlayer = false;

    //Subscription<CaptureEvent> capture_event_subscription;

    private LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Vector3.zero;
        lr = GetComponent<LineRenderer>();
        gameObject.GetComponent<Renderer>().material.renderQueue = 2000;
        //capture_event_subscription = EventBus.Subscribe<CaptureEvent>(_OnCaptureEvent);
    }

    // Update is called once per frame
    void Update()
    {
        lr.SetPositions(new Vector3[] { a1.transform.position, a2.transform.position});

        Attractor attractor1 = a1.GetComponent<Attractor>();
        Attractor attractor2 = a2.GetComponent<Attractor>();

        if ((attractor1.isPlayer) || (attractor2.isPlayer))
            isPlayer = true;
        //else if (attractor1.isCaptured && attractor2.isCaptured)
        //    isCaptured = true;

        if (isPlayer)
        {
            lr.material.color = Color.red;
            //} else if (isCaptured)
            //{
            //    lr.material.color = Color.blue;
        }
        else
        {
            lr.material.color = Color.white;
            if (a1 == null || a2 == null || (a1.transform.position - a2.transform.position).magnitude > maxDistance)
                Destroy(gameObject);
        }
    }

    //void _OnCaptureEvent(CaptureEvent e)
    //{
    //    if (e.capturedGameObject == a1 || e.capturedGameObject == a2)
    //    {
    //        isCaptured = true;
    //    }
    //}

    //private void OnDestroy()
    //{
    //    EventBus.Unsubscribe(capture_event_subscription);
    //}
}
