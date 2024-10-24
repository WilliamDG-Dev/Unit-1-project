using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSMeter : MonoBehaviour
{
    public TextMeshProUGUI fpsMeter;
    private float deltaTime;
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        int fps = Mathf.CeilToInt(1 / deltaTime);
        fpsMeter.text = "FPS: " + fps;
    }
}
