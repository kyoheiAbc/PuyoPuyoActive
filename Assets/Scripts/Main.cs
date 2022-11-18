using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class C
{
    public static readonly int FPS = 30;
    public static readonly Vector2 VEC_DROP = new Vector2(0, -0.03f);
    public static readonly Vector2 VEC_DROP_QUICK = new Vector2(0, -0.5f);
    public static readonly int TOUCH_CNT = 15;
    public static readonly int FIX_CNT = 30;
    public static readonly float EFFECT_REMOVE_CNT = 15;
    public static readonly float EFFECT_FIX_CNT = 15;
    public static readonly int COLOR_NUMBER = 4;

    public static readonly int REMOVE_NUMBER = 4;
    public static readonly Vector2 VEC_255 = new Vector2(255, 255);

    public static readonly Vector2 VEC_0 = new Vector2(0, 0);
    public static readonly Vector2 VEC_X = new Vector2(1, 0);
    public static readonly Vector2 VEC_Y = new Vector2(0, 1);
    public static readonly int FIELD_SIZE_X = 8;
    public static readonly int FIELD_SIZE_Y = 17;
    public static readonly GameObject[] PUYO = new GameObject[4] {
        Resources.Load<GameObject>("puyoA"),
        Resources.Load<GameObject>("puyoB"),
        Resources.Load<GameObject>("puyoC"),
        Resources.Load<GameObject>("puyoD")
    };
}

public class Main : MonoBehaviour
{
    InputController InputController;
    Field field;
    PuyoManager puyoManager;
    PuyoPuyo puyoPuyo;
    int cnt;
    bool rm;
    bool willRm;
    int rmCnt;

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
        field.reset();
        puyoManager.reset();

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
        rm = false;
        willRm = false;
        rmCnt = 0;
    }

    void Update()
    {

        DateTime old = DateTime.Now;


        switch (InputController.update())
        {
            case 4:
                if (-C.VEC_X == puyoPuyo.move(-C.VEC_X, puyoManager.getList()))
                {
                    cnt = 0;
                }
                break;
            case 6:
                if (C.VEC_X == puyoPuyo.move(C.VEC_X, puyoManager.getList()))
                {
                    cnt = 0;
                }
                break;
            case 2:
                if (C.VEC_0 == puyoPuyo.move(-C.VEC_Y / 2, puyoManager.getList()))
                {
                    cnt = C.FIX_CNT;
                }
                break;
            case 8:
                if (C.VEC_Y / 2 == puyoPuyo.move(C.VEC_Y / 2, puyoManager.getList()))
                {
                    cnt = 0;
                }
                break;
            case 16:
                puyoPuyo.rotate(1, puyoManager.getList());
                cnt = 0;
                break;
            case 14:
                puyoPuyo.rotate(-1, puyoManager.getList());
                cnt = 0;
                break;
        }

        if (C.VEC_DROP == puyoPuyo.move(C.VEC_DROP, puyoManager.getList())) cnt = 0;
        else cnt++;

        if (cnt > C.FIX_CNT)
        {

            cnt = 0;
            List<Puyo> puyo = puyoPuyo.getPuyo();
            puyoManager.addPuyo(puyo[0]);
            puyoManager.addPuyo(puyo[1]);
            rm = true;
            puyoPuyo = null;
            newPuyoPuyo();
        }


        if (!puyoManager.update(field, puyoPuyo.getPuyo()) && rm)
        {
            rm = false;
            if (field.rmCheck(puyoManager.getList()))
            {
                willRm = true;
            }
        }

        if (willRm)
        {
            rmCnt++;
            if (rmCnt == C.EFFECT_REMOVE_CNT)
            {
                willRm = false;
                rmCnt = 0;

                field.rm();
                puyoManager.rm();
                GameObject[] gO = GameObject.FindGameObjectsWithTag("REMOVE");
                for (int i = 0; i < gO.Length; i++)
                {
                    Destroy(gO[i]);
                }
                rm = true;


                Debug.Log("rm check again");
                // // puyoManager.update(field, puyoPuyo.getPuyo());

                // if (field.rmCheck(puyoManager.getList()))
                // {
                //     // Debug.Log("ok");
                //     willRm = true;
                // }
                // else
                // {
                //     // Debug.Log("ng");
                // }
            }
        }






        // render
        puyoPuyo.render();
        puyoManager.render();

        if (DateTime.Now.Millisecond - old.Millisecond > 0)
        {
            // Debug.Log(DateTime.Now.Millisecond - old.Millisecond);
        }

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
