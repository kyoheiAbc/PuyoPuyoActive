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
    List<EffectExplosion> eElist;
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
        eElist = new List<EffectExplosion>();

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
        int oldTime = DateTime.Now.Millisecond;

        if (cnt >= 300)
        {
            cnt++;
            if (cnt == 300 + C.NEXT_GAME_CNT) reset();
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

        comboManager.update();


        int input = inputController.update();
        switch (input)
        {
            case 4:
            case 6:
                puyoPuyo.move(C.VEC_X * Mathf.Sign(input - 5), puyoManager.getList());
                break;
            case 2:
                if (C.VEC_0 == puyoPuyo.move(-C.VEC_Y / 2, puyoManager.getList()))
                {
                    puyoPuyo.setCnt((int)C.FIX_CNT);
                    List<Puyo> puyo = puyoPuyo.getPuyo();
                    for (int i = 0; i < puyo.Count; i++) puyo[i].setCnt(0);
                }
                break;
            case 8:
                for (int n = 0; n < C.FIELD_SIZE_Y; n++)
                {
                    if (C.VEC_0 == puyoPuyo.move(-C.VEC_Y, puyoManager.getList())) break;
                }
                puyoPuyo.setCnt((int)C.FIX_CNT);
                List<Puyo> pL = puyoPuyo.getPuyo();
                for (int i = 0; i < pL.Count; i++) pL[i].setCnt(0);
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

        if (!puyoManager.update(field, puyoPuyoList))
        {
            if (cnt == 100)
            {
                {
                    if (field.rmCheck(false))
                    {
                        cnt = 200;
                    }
                    else cnt = 0;
                }
            }
        }

        if (comboManager.getCnt() == 0 && cnt < 200)
        {
            if (!puyoManager.canDrop(field, puyoPuyoList))
            {
                if (!field.rmCheck(true))
                {
                    comboManager.setCnt(1);
                }
                else
                {
                }
            }
            else
            {
            }
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
                Vector2 pos = C.VEC_0;
                for (int i = 0; i < gO.Length; i++)
                {
                    pos = pos + (Vector2)gO[i].transform.position / gO.Length;
                    eElist.Add(new EffectExplosion(gO[i], Instantiate(C.EFFECT_EXPLOSION)));
                    Destroy(gO[i]);
                }
                comboManager.comboPlus(pos);
                comboManager.setCnt(0);
            }
        }

        // render
        if (puyoPuyo != null) puyoPuyo.render();
        puyoManager.render();

        for (int i = 0; i < eElist.Count; i++)
        {
            if (!eElist[i].update())
            {
                Destroy(eElist[i].getGameObject());
                eElist.Remove(eElist[i]);
                i--;
            }
        }



        int nowTime = DateTime.Now.Millisecond;
        if (nowTime - oldTime > 0)
        {
            Debug.Log(nowTime - oldTime);
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
            newPuyo(colorBag.getColor(), pos + C.VEC_X)
        );
    }

    private Puyo newPuyo(int color, Vector2 pos)
    {
        return new Puyo(Instantiate(C.PUYO[color], pos, Quaternion.identity));
    }


}
