using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    InputController InputController;
    PuyoPuyo puyoPuyo;
    PuyoManager puyoManager;
    Field field;

    Puyo debug;

    private void Awake()
    {
        Application.targetFrameRate = 10;

        // Camera
        Camera cam = Camera.main;
        float aspect = (float)Screen.height / (float)Screen.width;
        if (aspect >= 2) cam.orthographicSize = cam.orthographicSize * aspect / 2;
    }

    void Start()
    {
        InputController = new InputController();
        field = new Field();
        puyoManager = new PuyoManager();
        puyoPuyo = new PuyoPuyo();

        // GameObject[] g = new GameObject[2]{
        //     Instantiate(Resources.Load<GameObject>("puyoA"), new Vector2(3.5f, 12.5f), Quaternion.identity),
        //     Instantiate(Resources.Load<GameObject>("puyoB"), new Vector2(4.5f, 12.5f), Quaternion.identity)
        // };

        // puyoPuyo.init(g, field, puyoManager);


        debug = new Puyo();
        debug.init(
            Instantiate(Resources.Load<GameObject>("puyoA"), new Vector2(3.5f, 12.5f), Quaternion.identity),
            field,
            puyoManager
        );
    }

    void Update()
    {
        int inputKey = InputController.update();

        if (inputKey == 4) debug.move(new Vector2(-1, 0));
        if (inputKey == 6) debug.move(new Vector2(1, 0));
        if (inputKey == 2) debug.move(new Vector2(0, -1));
        if (inputKey == 8) debug.move(new Vector2(0, 1));

        debug.move(new Vector2(0, -0.1f));

        debug.render();
    }

}
