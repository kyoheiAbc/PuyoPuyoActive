using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    InputController InputController;
    PuyoPuyo puyoPuyo;
    Field field;

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



        puyoPuyo = new PuyoPuyo(
                        new Puyo(0, Instantiate(Resources.Load<GameObject>("puyoA"), new Vector2(3.5f, 12.5f), Quaternion.identity)),
                        new Puyo(1, Instantiate(Resources.Load<GameObject>("puyoB"), new Vector2(4.5f, 12.5f), Quaternion.identity))
                    );

        field = new Field();

        int[] fieldSize = field.getSize();
        for (int y = 0; y < fieldSize[0]; y++)
        {
            for (int x = 0; x < fieldSize[1]; x++)
            {
                if (y == 0 || y == fieldSize[0] - 1 ||
                    x == 0 || x == fieldSize[1] - 1)
                {
                    field.setPuyo(
                        new Puyo(0, Instantiate(Resources.Load<GameObject>("puyo"),
                            new Vector2(x + 0.5f, y + 0.5f), Quaternion.identity)
                        )
                    );
                }
            }
        }

    }

    void Update()
    {

        int inputKey = InputController.update();


        puyoPuyo.move(new Vector2(0, -0.05f));

        if (inputKey == 4) puyoPuyo.move(new Vector2(-1, 0));
        if (inputKey == 6) puyoPuyo.move(new Vector2(1, 0));
        if (inputKey == 2) puyoPuyo.move(new Vector2(0, -1));
        if (inputKey == 8) puyoPuyo.move(new Vector2(0, 1));

    }
}
