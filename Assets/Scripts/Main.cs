using System.Collections.Generic;
using UnityEngine;
using System;


public class Main : MonoBehaviour
{
    InputController inputController;
    Field field;
    PuyoManager puyoManager;
    PuyoPuyo puyoPuyo;
    PuyoPuyo[] puyoPuyoNext;
    ColorBag colorBag;
    int cnt;

    private void Awake()
    {
        Application.targetFrameRate = 30;

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
        puyoPuyoNext = new PuyoPuyo[2];
        colorBag = new ColorBag();


        reset();


    }

    public void reset()
    {
        GameObject[] gOary;
        gOary = GameObject.FindGameObjectsWithTag("PUYO");
        for (int i = 0; i < gOary.Length; i++) Destroy(gOary[i]);

        gOary = GameObject.FindGameObjectsWithTag("REMOVE");
        for (int i = 0; i < gOary.Length; i++) Destroy(gOary[i]);


        inputController.init();
        field.init();
        puyoManager.init();
        colorBag.init();

        GameObject gO = Resources.Load<GameObject>("puyo");
        for (int y = 0; y < 15; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                if (y == 0 ||
                    x == 0 || x == 8 - 1)
                {
                    gO.transform.position = new Vector2(x + 0.5f, y + 0.5f);
                    Puyo puyo = new Puyo(gO);
                    field.setPuyo(puyo);
                    puyoManager.addPuyo(puyo);
                }
            }
        }

        puyoPuyo = newPuyoPuyo(new Vector2(3.5f, 12.5f));
        puyoPuyoNext[0] = newPuyoPuyo(new Vector2(3.5f, 15.5f));
        puyoPuyoNext[1] = newPuyoPuyo(new Vector2(6.5f, 15.5f));

        cnt = 0;
    }

    void Update()
    {
        int oldTime = DateTime.Now.Millisecond;

        if (cnt >= 300)
        {
            cnt++;
            if (cnt == 300 + 90) reset();
            return;
        }

        if (field.getPuyo(new Vector2(3.5f, 12.5f)) != null ||
            field.getPuyo(new Vector2(4.5f, 12.5f)) != null)
        {
            cnt = 300;
            return;
        }

        if (puyoPuyo == null)
        {
            nextPuyo();
            inputController.init();
        }


        int input = inputController.update();
        switch (input)
        {
            case 4:
            case 6:
                puyoPuyo.move(new Vector2(1, 0) * Mathf.Sign(input - 5), puyoManager.getList());
                break;
            case 2:
                if (new Vector2(0, 0) == puyoPuyo.move(-new Vector2(0, 1) / 2, puyoManager.getList()))
                {
                    puyoPuyo.setCnt((int)C.FIX_CNT);
                    List<Puyo> puyo = puyoPuyo.getPuyo();
                    for (int i = 0; i < puyo.Count; i++) puyo[i].setCnt(0);
                }
                break;
            case 14:
            case 16:
                puyoPuyo.rotate((int)Mathf.Sign(input - 15), puyoManager.getList());
                break;
        }


        puyoPuyo.update(puyoManager.getList());


        if (puyoPuyo.getCnt() == C.FIX_CNT)
        {
            List<Puyo> puyo = puyoPuyo.getPuyo();
            if (field.getPuyo(puyo[0].getPos() - new Vector2(0, 0.5f + C.RESOLUTION)) == null &&
                field.getPuyo(puyo[1].getPos() - new Vector2(0, 0.5f + C.RESOLUTION)) == null)
            {
                puyoPuyo.setCnt(C.FIX_CNT - 1);
                for (int i = 0; i < puyo.Count; i++) puyo[i].setCnt((int)C.EFFECT_FIX_CNT);
            }
            else
            {
                puyoPuyo = null;
                for (int i = 0; i < puyo.Count; i++) puyoManager.addPuyo(puyo[i]);

                if (cnt == 0) cnt = 100;
            }
        }


        List<Puyo> puyoPuyoList;
        if (puyoPuyo != null) puyoPuyoList = puyoPuyo.getPuyo();
        else puyoPuyoList = new List<Puyo>();

        if (!puyoManager.update(field, puyoPuyoList) && cnt == 100)
        {
            if (field.rmCheck()) cnt = 200;
            else cnt = 0;
        }



        if (cnt >= 200)
        {
            cnt++;
            if (cnt == 200 + C.EFFECT_REMOVE_CNT)
            {
                cnt = 100;

                field.rm();
                puyoManager.rm();
                GameObject[] gO = GameObject.FindGameObjectsWithTag("REMOVE");
                for (int i = 0; i < gO.Length; i++) Destroy(gO[i]);
            }
        }


        // render
        if (puyoPuyo != null) puyoPuyo.render();
        puyoManager.render();

        int nowTime = DateTime.Now.Millisecond;
        if (nowTime - oldTime > 0)
        {
            // Debug.Log(nowTime - oldTime);
        }
    }

    private bool nextPuyo()
    {
        puyoPuyo = puyoPuyoNext[0];
        puyoPuyoNext[0] = puyoPuyoNext[1];
        puyoPuyoNext[1] = newPuyoPuyo(new Vector2(6.5f, 15.5f));

        puyoPuyo.setPos(new Vector2(3.5f, 12.5f));
        puyoPuyoNext[0].setPos(new Vector2(3.5f, 15.5f));
        puyoPuyoNext[1].setPos(new Vector2(6.5f, 15.5f));

        puyoPuyo.render();
        puyoPuyoNext[0].render();
        puyoPuyoNext[1].render();

        return true;
    }



    private PuyoPuyo newPuyoPuyo(Vector2 pos)
    {
        return new PuyoPuyo(
            newPuyo(colorBag.getColor(), pos),
            newPuyo(colorBag.getColor(), pos + new Vector2(1, 0))
        );
    }

    private Puyo newPuyo(int color, Vector2 pos)
    {
        return new Puyo(Instantiate(C.PUYO[color], pos, Quaternion.identity));
    }


}



public static class C
{
    public static readonly int COLOR_NUMBER = 4;
    public static readonly int REMOVE_NUMBER = 4;
    public static readonly Vector2 VEC_DROP = new Vector2(0, -0.03f);
    public static readonly float RESOLUTION = 0.001f;
    public static readonly int FIX_CNT = 30;
    public static readonly float EFFECT_REMOVE_CNT = 20;
    public static readonly float EFFECT_FIX_CNT = 10;
    public static readonly GameObject[] PUYO = new GameObject[4] {
        Resources.Load<GameObject>("puyoA"),
        Resources.Load<GameObject>("puyoB"),
        Resources.Load<GameObject>("puyoC"),
        Resources.Load<GameObject>("puyoD")
    };
}

public class ColorBag
{
    int[] bag;
    int cnt;
    public ColorBag()
    {
        bag = new int[C.COLOR_NUMBER * 64];
        for (int i = 0; i < bag.Length; i++)
        {
            bag[i] = i % C.COLOR_NUMBER;
        }
    }
    public void init()
    {
        for (int i = bag.Length - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            int tmp = bag[i];
            bag[i] = bag[j];
            bag[j] = tmp;
        }
        cnt = 0;
    }
    public int getColor()
    {
        int ret;
        try
        {
            ret = bag[cnt];
        }
        catch
        {
            init();
            ret = bag[cnt];
        }
        cnt++;
        return ret;
    }
}
