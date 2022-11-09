using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoonButtonController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public GameObject prefab;
    public int cost = 1;
    public float minDistanceToPlanet = 4f;
    private GameObject moon;

    public void OnBeginDrag(PointerEventData data)
    {
        if (GameManager.TutorialStage < 4)
            return;
        moon = Instantiate(prefab, Utils.ScreenToWorld(data.position, 5), Quaternion.identity);
    }

    public void OnDrag(PointerEventData data)
    {
        if (moon == null)
            return;
        moon.transform.position = Utils.ScreenToWorld(data.position, 5);
    }

    public void OnEndDrag(PointerEventData data)
    {
        if (moon == null)
            return;
        float distance = (Utils.ScreenToWorld(data.position) - GameManager.Planet.transform.position).magnitude;
        if (distance < minDistanceToPlanet || Utils.IsMouseOverUI || !GameManager.Player.GetComponent<Inventory>().useRocks(cost))
        {
            if (distance < minDistanceToPlanet)
                ToastManager.ToastErrorMsg("That's too close!");
            Destroy(moon);
            return;
        }

        if (GameManager.TutorialStage == 4)
        {
            EventBus.Publish(new TutorialStageChangeEvent(5));
        }

        Orbit orbit = moon.GetComponent<Orbit>();
        if (orbit != null)
        {
            orbit.pivot = GameManager.Planet;
            orbit.isOrbit = true;
            orbit.orbitSpeed = 150 / Mathf.Pow(distance, 0.3f);
        }

        moon.transform.position = Utils.ScreenToWorld(data.position);
        MoonController mc = moon.GetComponent<MoonController>();
        if (mc)
            mc.active = true;
    }
}
