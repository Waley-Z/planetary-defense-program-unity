using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIInfoController : MonoBehaviour
{
    private Health health;
    private Inventory inventory;
    TextMeshProUGUI tmp;


    // Start is called before the first frame update
    void Start()
    {
        health = GameManager.Planet.GetComponent<Health>();
        inventory = GameManager.Player.GetComponent<Inventory>();
        tmp = GetComponent<TextMeshProUGUI>();
        if (!health || !inventory)
            Debug.LogError("init error");
    }

    // Update is called once per frame
    void Update()
    {
        string healthStr = Mathf.RoundToInt(Mathf.Max(health.getHealthPercentage() * 100, 0)).ToString();
        string invStr = Mathf.FloorToInt(inventory.rocks).ToString();
        tmp.text = healthStr + "%\n" + invStr;
    }
}
