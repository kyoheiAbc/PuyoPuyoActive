using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 30;

        // Camera
        Camera cam = Camera.main;
        float aspect = (float)Screen.height / (float)Screen.width;
        if (aspect >= 2) cam.orthographicSize = cam.orthographicSize * aspect / 2;
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
