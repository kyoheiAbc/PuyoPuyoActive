using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

public class Main : MonoBehaviour
{
    InputController inputController;
    PuyoManager puyoManager;
    int cnt;

    void Start()
    {
        Application.targetFrameRate = 30;

        inputController = InputController.I();
        puyoManager = PuyoManager.I();
    }

    public void init()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("PUYO");
        for (int i = 0; i < gameObjects.Length; i++)
        {
            Addressables.ReleaseInstance(gameObjects[i]);
        }

        inputController.init();
        puyoManager.init();
        Field.I().init();

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

        PuyoPuyo puyoPuyo = puyoManager.getPuyoPuyo();
        if (puyoPuyo == null)
        {
            puyoManager.nextPuyoPuyo();
            List<Puyo> puyoList = puyoManager.getList();
            for (int i = 0; i < puyoList.Count; i++)
            {
                if (!puyoList[i].canPut())
                {
                    cnt++;
                    goto render;
                }
            }
            puyoPuyo = puyoManager.getPuyoPuyo();
            inputController.init();
        }

        switch (inputController.update())
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

        puyoManager.update();

        Boss.I().update();

        ComboManager.I().update();


    render:
        puyoManager.render();

        // int nowTime = DateTime.Now.Millisecond;
        // if (nowTime - oldTime > 0) Debug.Log(nowTime - oldTime);
    }
}


public static class C
{
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
