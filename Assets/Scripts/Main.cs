using System.Collections.Generic;
using UnityEngine;

public static class C
{
    public static readonly int FPS = 30;
    public static readonly int FIELD_SIZE_X = 8;
    public static readonly int FIELD_SIZE_Y = 17;
    public static readonly int COLOR_NUMBER = 4;
    public static readonly int REMOVE_NUMBER = 4;
    public static readonly Vector2 VEC_0 = new Vector2(0, 0);
    public static readonly Vector2 VEC_1 = new Vector2(1, 1);
    public static readonly Vector2 VEC_255 = new Vector2(255, 255);
    public static readonly Vector2 VEC_X = new Vector2(1, 0);
    public static readonly Vector2 VEC_Y = new Vector2(0, 1);
    public static readonly Vector2 VEC_DROP = new Vector2(0, -0.03f);
    public static readonly Vector2 VEC_DROP_QUICK = new Vector2(0, -0.5f);
    public static readonly int FIX_CNT = 30;
    public static readonly float EFFECT_REMOVE_CNT = 30;
    public static readonly float EFFECT_FIX_CNT = 10;
    public static readonly GameObject[] PUYO = new GameObject[4] {
        Resources.Load<GameObject>("puyoA"),
        Resources.Load<GameObject>("puyoB"),
        Resources.Load<GameObject>("puyoC"),
        Resources.Load<GameObject>("puyoD")
    };
}

public class Main : MonoBehaviour
{
    InputController inputController;
    Field field;
    PuyoManager puyoManager;
    PuyoPuyo puyoPuyo;
    int fixCnt;
    int rmCnt;
    private void Awake()
    {
        Application.targetFrameRate = C.FPS;

        // Camera
        if ((float)Screen.height / (float)Screen.width >= 2)
        {
            Camera.main.orthographicSize =
                Camera.main.orthographicSize * (float)Screen.height / (float)Screen.width / 2;
        }
    }

    void Start()
    {
        inputController = new InputController();
        field = new Field();
        puyoManager = new PuyoManager();

        reset();
    }

    public void reset()
    {
        inputController.init();
        field.init();
        puyoManager.init();

        GameObject gO = Resources.Load<GameObject>("puyo");
        for (int y = 0; y < C.FIELD_SIZE_Y; y++)
        {
            for (int x = 0; x < C.FIELD_SIZE_X; x++)
            {
                if (y == 0 || y == C.FIELD_SIZE_Y - 1 ||
                    x == 0 || x == C.FIELD_SIZE_X - 1)
                {
                    gO.transform.position = new Vector2(x + 0.5f, y + 0.5f);
                    Puyo puyo = new Puyo(gO);
                    field.setPuyo(puyo);
                    puyoManager.addPuyo(puyo);
                }
            }
        }

        newPuyoPuyo();
        fixCnt = 0;
        rmCnt = 0;
    }

    void Update()
    {
        switch (inputController.update())
        {
            case 0:
                puyoPuyo.setFixEffectCnt((int)C.EFFECT_FIX_CNT);
                break;
            case 4:
                if (-C.VEC_X == puyoPuyo.move(-C.VEC_X, puyoManager.getList())) fixCnt = 0;
                break;
            case 6:
                if (C.VEC_X == puyoPuyo.move(C.VEC_X, puyoManager.getList())) fixCnt = 0;
                break;
            case 2:
                if (C.VEC_0 == puyoPuyo.move(-C.VEC_Y / 2, puyoManager.getList()))
                {
                    puyoPuyo.setFixEffectCnt(0);
                    fixCnt = C.FIX_CNT;
                }
                else
                {
                    fixCnt = 0;
                }
                break;
            case 14:
                puyoPuyo.rotate(-1, puyoManager.getList());
                fixCnt = 0;
                break;
            case 16:
                puyoPuyo.rotate(1, puyoManager.getList());
                fixCnt = 0;
                break;
        }


        if (C.VEC_0 == puyoPuyo.move(C.VEC_DROP, puyoManager.getList()))
        {
            fixCnt++;
        }
        else
        {
            fixCnt = 0;
        }


        if (fixCnt > C.FIX_CNT)
        {
            List<Puyo> puyo = puyoPuyo.getPuyo();

            if (field.getPuyo(puyo[0].getPos() - C.VEC_Y) != null ||
                field.getPuyo(puyo[1].getPos() - C.VEC_Y) != null)
            {

                fixCnt = 0;

                puyoManager.addPuyo(puyo[0]);
                puyoManager.addPuyo(puyo[1]);

                if (rmCnt < 0) rmCnt = 0;

                newPuyoPuyo();
                inputController.init();
            }
            else
            {
                fixCnt--;
            }
        }


        if (!puyoManager.update(field, puyoPuyo.getPuyo()) && rmCnt == 0)
        {
            rmCnt = -1;
            if (field.rmCheck())
            {
                rmCnt = 1;
            }
        }


        if (rmCnt > 0)
        {
            rmCnt++;
            if (rmCnt == C.EFFECT_REMOVE_CNT)
            {
                rmCnt = 0;

                field.rm();
                puyoManager.rm();
                GameObject[] gO = GameObject.FindGameObjectsWithTag("REMOVE");
                for (int i = 0; i < gO.Length; i++) Destroy(gO[i]);
            }
        }


        // render
        puyoPuyo.render();
        puyoManager.render();
    }

    private bool newPuyoPuyo()
    {
        puyoPuyo = new PuyoPuyo(
            new Puyo(
                Instantiate(C.PUYO[UnityEngine.Random.Range(0, C.COLOR_NUMBER)], new Vector2(3.5f, 12.5f), Quaternion.identity)
            ),
            new Puyo(
                Instantiate(C.PUYO[UnityEngine.Random.Range(0, C.COLOR_NUMBER)], new Vector2(4.5f, 12.5f), Quaternion.identity)
            )
        );
        return true;
    }
}
