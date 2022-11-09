using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMoonsController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Utils.IsMouseOverUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Utils.IsMouseOverUI = false;
    }
}
