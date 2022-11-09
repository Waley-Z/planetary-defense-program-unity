using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 10f;
    public float maxCameraSize = 9f;
    public float minCameraSize = 6f;

    CinemachineVirtualCamera vCam;
    //CinemachineComponentBase componentBase;

    // Start is called before the first frame update
    void Start()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        //componentBase = vCam.GetCinemachineComponent(CinemachineCore.Stage.Body);
        if (vCam == null)
            Debug.LogError("vcam init failed");
    }

    // Update is called once per frame
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            float newCameraSize = vCam.m_Lens.OrthographicSize - scroll * sensitivity;
            if (newCameraSize < minCameraSize || newCameraSize > maxCameraSize)
                return;
            vCam.m_Lens.OrthographicSize = newCameraSize;
        }
    }
}
