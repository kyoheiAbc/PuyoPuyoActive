using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

public class Main : MonoBehaviour
{
    int cnt;

    void Start()
    {
        Application.targetFrameRate = 30;
    }

    public void init()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("PUYO");
        for (int i = 0; i < gameObjects.Length; i++)
        {
            Addressables.ReleaseInstance(gameObjects[i]);
        }

        ComboManager.I().init();
        Field.I().init();
        InputController.I().init();
        Opponent.I().init();
        PuyoManager.I().init();
        ScoreSystem.I().init();

        cnt = 0;
    }

    void Update()
    {
        // int oldTime = DateTime.Now.Millisecond;

        if (cnt > 0)
        {
            cnt++;
            if (cnt > 90)
            {
                init();
            }
            goto render;
        }

        PuyoPuyo puyoPuyo = PuyoManager.I().getPuyoPuyo();
        if (puyoPuyo == null)
        {
            PuyoManager.I().nextPuyoPuyo();
            // List<Puyo> puyoList = PuyoManager.I().getList();
            // for (int i = 0; i < puyoList.Count; i++)
            // {
            //     if (!puyoList[i].canPut())
            //     {
            //         cnt++;
            //         goto render;
            //     }
            // }
            puyoPuyo = PuyoManager.I().getPuyoPuyo();
            InputController.I().init();
        }

        switch (InputController.I().update())
        {
            case 4:
                if (puyoPuyo.move(new Vector2(-1, 0)) != new Vector2(0, 0))
                {
                    puyoPuyo.setCnt(0);
                }
                break;
            case 6:
                if (puyoPuyo.move(new Vector2(1, 0)) != new Vector2(0, 0))
                {
                    puyoPuyo.setCnt(0);
                }
                break;
            case 2:
                if (puyoPuyo.move(-new Vector2(0, 1)) != new Vector2(0, 0))
                {
                    puyoPuyo.setCnt(0);
                }
                else
                {
                    puyoPuyo.setCnt(C.FIX_CNT);
                }
                break;
            case 8:
                while (true)
                {
                    if (puyoPuyo.move(-new Vector2(0, 1)) == new Vector2(0, 0)) break;
                }
                puyoPuyo.setCnt(C.FIX_CNT);
                break;
            case 16:
                puyoPuyo.rotate(1);
                puyoPuyo.setCnt(0);
                break;
        }

        PuyoManager.I().update();

        ComboManager.I().update();

        Opponent.I().update();

        ScoreSystem.I().update();


    render:
        PuyoManager.I().render();

        // int nowTime = DateTime.Now.Millisecond;
        // if (nowTime - oldTime > 0) Debug.Log(nowTime - oldTime);
    }
}


public static class C
{
    public static readonly int OPPONENT_HP = 300;
    public static readonly int[] OPPONENT_ATTACK = new int[3] { 1, 3, 3 };
    public static readonly int OPPONENT_SPEED = 10;
    public static readonly int OPPONENT_ACT_RATE = 30;
    public static readonly int COMBO_CNT = 30;
    public static readonly int COLOR_NUMBER = 4;
    public static readonly int REMOVE_NUMBER = 4;
    public static readonly Vector2 VEC_DROP = new Vector2(0, -0.03f);
    public static readonly int FIX_CNT = 30;
    public static readonly int EFFECT_REMOVE_CNT = 20;
    public static readonly int EFFECT_FIX_CNT = 10;
    public static readonly Color[] PUYO_COLORS = new Color[6]{
        Color.HSVToRGB(0.0f, 0.5f, 1.0f),
        Color.HSVToRGB(0.2f, 0.5f, 1.0f),
        Color.HSVToRGB(0.4f, 0.5f, 1.0f),
        Color.HSVToRGB(0.6f, 0.5f, 1.0f),
        Color.HSVToRGB(0.8f, 0.5f, 1.0f),
        Color.HSVToRGB(0.0f, 0.0f, 0.8f)
    };
    public static readonly GameObject PUYO_GAME_OBJECT = Addressables.LoadAssetAsync<GameObject>("Assets/Sources/puyo.prefab").WaitForCompletion();

    public static int COMBO_TO_OJAMA(int c)
    {
        if (c < 4) return c;
        if (c == 4) return 3;
        return (int)(9 * Mathf.Pow(2, c - 5));
    }
    public static void SHUFFLE(ref int[] ary)
    {
        for (int i = ary.Length - 1; i > 0; i--)
        {
            int r = UnityEngine.Random.Range(0, i + 1);
            int tmp = ary[i];
            ary[i] = ary[r];
            ary[r] = tmp;
        }
    }
}
