using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToastManager : MonoBehaviour
{
    public static ToastManager Instance;
    private static GameObject HUDPanel;
    public static GameObject CurrentInstruction;
    public static GameObject CurrentError;
    private static GameObject InstructionToastPrefab;
    private static GameObject ErrorToastPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        InstructionToastPrefab = GameAssets.GetPrefab("InstructionToast");
        ErrorToastPrefab = GameAssets.GetPrefab("ErrorToast");
    }

    public void Start()
    {
        getPanel();
    }

    private static bool getPanel()
    {
        HUDPanel = GameObject.Find("HUD Panel");
        return HUDPanel != null;
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ToastErrorMsg("hello" + Time.time);
        }
    }

    public static void ToastInstruction(string str, float duration = float.MaxValue)
    {
        if (!getPanel())
        {
            Debug.Log("no panel");
            return;
        }
        if (CurrentInstruction != null)
        {
            Destroy(CurrentInstruction);
        }
        CurrentInstruction = Instantiate(InstructionToastPrefab, HUDPanel.transform);
        CurrentInstruction.GetComponent<TextMeshProUGUI>().text = str;
        CurrentInstruction.GetComponent<InstructionToastController>().maxTime = duration;
    }

    public static void ToastErrorMsg(string str)
    {
        if (!getPanel())
            return;
        if (CurrentError != null)
            Destroy(CurrentError);
        CurrentError = Instantiate(ErrorToastPrefab, HUDPanel.transform);
        CurrentError.GetComponent<TextMeshProUGUI>().text = str;
    }
}
