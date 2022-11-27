using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class C
{
    public static readonly int FPS = 30;
    public static readonly int FIELD_SIZE_X = 8;
    public static readonly int FIELD_SIZE_Y = 17;
    public static readonly int COLOR_NUMBER = 4;
    public static readonly int COLOR_ADJUST = 8;
    public static readonly int REMOVE_NUMBER = 4;
    public static readonly Vector2 VEC_0 = new Vector2(0, 0);
    public static readonly Vector2 VEC_X = new Vector2(1, 0);
    public static readonly Vector2 VEC_Y = new Vector2(0, 1);
    public static readonly Vector2 UNDER = new Vector2(0, -0.501f);
    public static readonly Vector2 VEC_DROP = new Vector2(0, -0.03f);
    public static readonly Vector2 VEC_DROP_QUICK = new Vector2(0, -0.4f);
    public static readonly int NEXT_GAME_CNT = 30;
    public static readonly int FIX_CNT = 30;
    public static readonly int EFFECT_REMOVE_CNT = 30;
    public static readonly int EFFECT_FIX_CNT = 10;
    public static readonly int COMBO_CNT = 45;

    public static readonly int BOSS_HP = 7;

    public static readonly GameObject[] PUYO = new GameObject[10] {
        Resources.Load<GameObject>("puyoA"),
        Resources.Load<GameObject>("puyoB"),
        Resources.Load<GameObject>("puyoC"),
        Resources.Load<GameObject>("puyoD"),
        Resources.Load<GameObject>(""),
        Resources.Load<GameObject>(""),
        Resources.Load<GameObject>(""),
        Resources.Load<GameObject>(""),
        Resources.Load<GameObject>(""),
        Resources.Load<GameObject>("puyoZ")
    };
    public static readonly GameObject EFFECT_EXPLOSION = Resources.Load<GameObject>("EffectExplosion");
    public static readonly GameObject GAUGE = Resources.Load<GameObject>("Gauge");

    public static float QuadraticF(float x, float max)
    {
        return -4f * max * (x - 0.5f) * (x - 0.5f) + max;
    }

    public static int COMBO_TO_OJAMA(int c)
    {
        switch (c)
        {
            case 0:
            case 1: return c;
            case 2: return 3;
            case 3: return 6;
        }
        return (int)(9 * Mathf.Pow(2, c - 4));
    }

}

public class ColorBag
{
    int[] bag;
    int cnt;
    public ColorBag()
    {
        bag = new int[C.COLOR_NUMBER * C.COLOR_ADJUST];
        for (int i = 0; i < bag.Length; i++)
        {
            bag[i] = i % C.COLOR_NUMBER;
        }
    }
    public void init()
    {
        cnt = -1;
        for (int i = bag.Length - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            int tmp = bag[i];
            bag[i] = bag[j];
            bag[j] = tmp;
        }
    }

    public int getColor()
    {
        if (cnt == bag.Length - 1)
        {
            init();
        }
        cnt++;
        return bag[cnt];
    }
}