using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Utils
{
    public static bool IsMouseOverUI = false;
    public static bool IsMouseOverMoon = false;

    public static Vector3 ScreenToWorld(Vector3 pos, float zOffset = 0)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(pos);
        worldPosition.z = zOffset;
        return worldPosition;
    }

    //public static bool IsMouseOverUI()
    //{
    //    return EventSystem.current.IsPointerOverGameObject();
    //}

    public static bool IfPositionVisible(Vector3 pos)
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(pos);
        return viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1;
    }
}

