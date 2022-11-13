using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class C
{
    public static readonly int FPS = 30;
    public static readonly Vector2 VEC_DROP = new Vector2(0, -0.03f);
    public static readonly Vector2 VEC_DROP_QUICK = new Vector2(0, -0.3f);
    public static readonly float PUYO_R = 0.5f + VEC_DROP.y * 0.001f;
    public static readonly int TOUCH_CNT = 15;
    public static readonly int FIX_CNT = 30;
    public static readonly Vector2 VEC_X = new Vector2(1, 0);
    public static readonly Vector2 VEC_Y = new Vector2(0, 1);

}

public class Main : MonoBehaviour
{
    InputController InputController;
    PuyoPuyo puyoPuyo;
    Field field;
    PuyoManager puyoManager;
    int cnt;

    private void Awake()
    {
        Application.targetFrameRate = C.FPS;

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

        reset();
    }

    public void reset()
    {
        cnt = 0;

        GameObject[] gO = GameObject.FindGameObjectsWithTag("PUYO");
        for (int i = 0; i < gO.GetLength(0); i++)
        {
            Destroy(gO[i]);
        }

        puyoPuyo = null;
        field.reset();
        puyoManager.reset();

        newPuyoPuyo();

        // Debug
        for (int n = 0; n < 8; n++)
        {
            Puyo p =
                new Puyo(
                    field,
                    Instantiate(
                        Resources.Load<GameObject>("puyoC"),
                        new Vector2(Random.Range(1, 7) + 0.5f, Random.Range(1, 13) + 0.5f),
                        Quaternion.identity
                    )
                );
            if (p.getPos() == new Vector2(3.5f, 12.5f) ||
                p.getPos() == new Vector2(4.5f, 12.5f))
            {
                Destroy(p.getGameObject());
                p = null;
            }
            else
            {
                field.setPuyo(p);
            }
        }
    }

    void Update()
    {
        if (puyoPuyo == null)
        {
            if (!newPuyoPuyo())
            {
                reset();
                return;
            }
        }

        if (puyoPuyo != null)
        {
            switch (InputController.update())
            {
                case 4:
                    if (-C.VEC_X == puyoPuyo.move(-C.VEC_X)) cnt = 0;
                    break;
                case 6:
                    if (C.VEC_X == puyoPuyo.move(C.VEC_X)) cnt = 0;
                    break;
                case 2:
                    if (-C.VEC_Y / 2 == puyoPuyo.move(-C.VEC_Y / 2)) cnt = 0;
                    break;
                case 8:
                    if (C.VEC_Y / 2 == puyoPuyo.move(C.VEC_Y / 2)) cnt = 0;
                    break;
                case 16:
                    puyoPuyo.rotate(1);
                    cnt = 0;
                    break;
                case 14:
                    puyoPuyo.rotate(-1);
                    cnt = 0;
                    break;
            }

            if (C.VEC_DROP != puyoPuyo.move(C.VEC_DROP)) cnt++;
            else cnt = 0;

            if (cnt == C.FIX_CNT)
            {
                cnt = 0;
                Puyo[] puyo = puyoPuyo.getPuyo();
                if (puyo[0].getPos().y <= puyo[1].getPos().y)
                {
                    puyoManager.addPuyo(puyo[0]);
                    puyoManager.addPuyo(puyo[1]);
                }
                else
                {
                    puyoManager.addPuyo(puyo[1]);
                    puyoManager.addPuyo(puyo[0]);
                }
                puyoPuyo = null;
            }
        }

        puyoManager.update();


        // render
        if (puyoPuyo != null) puyoPuyo.render();
        puyoManager.render();
        field.render();
    }

    private bool newPuyoPuyo()
    {

        if (field.getPuyo(new Vector2(3.5f, 12.5f)) != null ||
            field.getPuyo(new Vector2(4.5f, 12.5f)) != null)
        {
            return false;
        }

        puyoPuyo = new PuyoPuyo(
            new Puyo(
                field,
                Instantiate(Resources.Load<GameObject>("puyoA"), new Vector2(3.5f, 12.5f), Quaternion.identity)
            ),
            new Puyo(
                field,
                Instantiate(Resources.Load<GameObject>("puyoB"), new Vector2(4.5f, 12.5f), Quaternion.identity)
            )
        );
        return true;
    }
}
