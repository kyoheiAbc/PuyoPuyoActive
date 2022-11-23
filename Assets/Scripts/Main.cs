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
    ComboManager comboManager;
    EffectManager effectManager;
    int cnt;

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
        puyoPuyoNext = new PuyoPuyo[2];
        colorBag = new ColorBag();
        comboManager = new ComboManager();
        effectManager = new EffectManager();

        reset();
    }

    public void reset()
    {
        GameObject[] gOary;
        gOary = GameObject.FindGameObjectsWithTag("PUYO");
        for (int i = 0; i < gOary.Length; i++) Destroy(gOary[i]);

        gOary = GameObject.FindGameObjectsWithTag("REMOVE");
        for (int i = 0; i < gOary.Length; i++) Destroy(gOary[i]);

        gOary = GameObject.FindGameObjectsWithTag("EFFECT");
        for (int i = 0; i < gOary.Length; i++) Destroy(gOary[i]);

        inputController.init();
        field.init();
        puyoManager.init();
        colorBag.init();

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

        puyoPuyo = newPuyoPuyo(new Vector2(3.5f, 12.5f));
        puyoPuyoNext[0] = newPuyoPuyo(new Vector2(3.5f, 15.5f));
        puyoPuyoNext[1] = newPuyoPuyo(new Vector2(6.5f, 15.5f));

        cnt = 0;
    }

    void Update()
    {
        // debug
        int oldTime = DateTime.Now.Millisecond;

        // game over
        if (cnt >= 900)
        {
            cnt++;
            if (cnt - 900 - 1 != (int)C.NEXT_GAME_CNT) return;
            reset();
        }
        if (field.getPuyo(new Vector2(3.5f, 12.5f)) != null || field.getPuyo(new Vector2(4.5f, 12.5f)) != null)
        {
            cnt = 900;
            return;
        }

        // generate
        if (puyoPuyo == null)
        {
            nextPuyo();
            inputController.init();
        }

        // move puyoPuyo
        int input = inputController.update();
        switch (input)
        {
            case 4:
            case 6:
                puyoPuyo.move(C.VEC_X * Mathf.Sign(input - 5), puyoManager.getList());
                break;
            case 2:
                if (puyoPuyo.move(-C.VEC_Y / 2, puyoManager.getList()) == C.VEC_0)
                {
                    // ready fix
                    puyoPuyo.setCnt(C.FIX_CNT);
                    puyoPuyo.getPuyo()[0].setCnt(0);
                    puyoPuyo.getPuyo()[1].setCnt(0);
                }
                break;
            case 8:
                for (int n = 0; n < C.FIELD_SIZE_Y - 1; n++)
                {
                    puyoPuyo.move(-C.VEC_Y, puyoManager.getList());
                }
                // ready fix
                puyoPuyo.setCnt(C.FIX_CNT);
                puyoPuyo.getPuyo()[0].setCnt(0);
                puyoPuyo.getPuyo()[1].setCnt(0);

                break;
            case 14:
            case 16:
                puyoPuyo.rotate((int)Mathf.Sign(input - 15), puyoManager.getList());
                break;
        }


        // drop
        puyoPuyo.move(C.VEC_DROP, puyoManager.getList());
        puyoManager.move(C.VEC_DROP_QUICK, puyoPuyo.getPuyo());


        // field set
        puyoPuyo.setCnt(puyoPuyo.getCnt() + 1);
        if (puyoPuyo.getCnt() > C.FIX_CNT)
        {
            List<Puyo> puyo = puyoPuyo.getPuyo();
            if (field.getPuyo(puyo[0].getPos() + C.UNDER) == null && field.getPuyo(puyo[1].getPos() + C.UNDER) == null)
            {
                // ready fix cancel
                puyoPuyo.setCnt(0);
                puyoPuyo.getPuyo()[0].setCnt(C.EFFECT_FIX_CNT);
                puyoPuyo.getPuyo()[1].setCnt(C.EFFECT_FIX_CNT);
            }
            else
            {
                puyoPuyo = null;
                for (int i = 0; i < puyo.Count; i++) puyoManager.addPuyo(puyo[i]);
                if (cnt < 200) cnt = 100;
            }
        }
        puyoManager.setPuyo(field);



        // fix effect
        field.effect();



        // rm check
        List<Puyo> puyoPuyoList;
        if (puyoPuyo != null) puyoPuyoList = puyoPuyo.getPuyo();
        else puyoPuyoList = new List<Puyo>();

        if (cnt == 100 &&
            !field.effectIng() &&
            !puyoManager.canDrop(puyoPuyoList)
        )
        {
            int rmCnt = field.rmCheck();
            if (rmCnt > 0)
            {
                cnt = 200;
                comboManager.setPlus(rmCnt);
            }
            else
            {
                cnt = 0;
                comboManager.setCnt(1);
            }
        }


        // rm
        if (cnt >= 200)
        {
            cnt++;
            if (cnt == 200 + C.EFFECT_REMOVE_CNT)
            {
                cnt = 100;

                field.rm();
                puyoManager.rm();
                GameObject[] gO = GameObject.FindGameObjectsWithTag("REMOVE");

                Vector2 p = C.VEC_0;
                for (int i = 0; i < gO.Length; i++)
                {
                    p = p + (Vector2)gO[i].transform.position / gO.Length;
                    effectManager.add(gO[i], Instantiate(C.EFFECT_EXPLOSION));
                    Destroy(gO[i]);
                }

                comboManager.setCombo(p);
            }
        }

        // combo
        comboManager.update();


        // render
        if (puyoPuyo != null) puyoPuyo.render();
        puyoManager.render();
        effectManager.render();
        GameObject[] gOe = GameObject.FindGameObjectsWithTag("REMOVE");
        for (int i = 0; i < gOe.Length; i++) Destroy(gOe[i]);


        int nowTime = DateTime.Now.Millisecond;
        // if (nowTime - oldTime >= 0) Debug.Log("- " + (nowTime - oldTime) + " -");

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
            newPuyo(colorBag.getColor(), pos + C.VEC_X)
        );
    }

    private Puyo newPuyo(int color, Vector2 pos)
    {
        return new Puyo(Instantiate(C.PUYO[color], pos, Quaternion.identity));
    }
}
