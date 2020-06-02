using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public Camera mainCamera;
    public float shakeAmount = 0f;

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    public void Shake(float amount, float length)
    {
        shakeAmount = amount;
        InvokeRepeating("BeginShake", 0, 0.01f);
        InvokeRepeating("StopShake", length, 0f);

    }
    private void BeginShake()
    {
        if (shakeAmount > 0)
        {
            Vector3 cameraPosition = mainCamera.transform.position;

            float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
            float offsetY = Random.value * shakeAmount * 2 - shakeAmount;

            cameraPosition.x += offsetX;
            cameraPosition.y += offsetY;

            mainCamera.transform.position = cameraPosition;
        }

    }

    private void StopShake()
    {
        CancelInvoke("BeginShake");
        mainCamera.transform.localPosition = Vector3.zero;

    }
}
