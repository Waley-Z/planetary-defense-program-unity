using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class StartTextController : MonoBehaviour
{
    void OnMouseUpAsButton()
    {
        GameManager.loadMainGame();
    }
}
