using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    InputController InputController;

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
        InputController = new InputController();

    }

    void Update()
    {
        int inputKey = InputController.update();
        if (inputKey != 0) Debug.Log(inputKey);
    }
}
