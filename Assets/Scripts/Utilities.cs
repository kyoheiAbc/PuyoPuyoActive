using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class C
{
    public static int FPS;
    public static int COLOR_NUMBER;
    public static int REMOVE_NUMBER;
    public static int FIX_CNT;
    public static int EFFECT_FIX_CNT;
    public static int EFFECT_REMOVE_CNT;
    public static int COMBO_CNT;
    public static float BOSS_HP;
    public static int BOSS_ATTACK;
    public static float BOSS_SPEED;
    public static int GAME_TIME_SEC;
    public static int NEXT_GAME_CNT;
    public static Vector2 VEC_DROP;
    public static Vector2 VEC_DROP_QUICK;
    public static readonly int FIELD_SIZE_X = 8;
    public static readonly int FIELD_SIZE_Y = 17;
    public static readonly int COLOR_ADJUST = 8;
    public static readonly Vector2 VEC_0 = new Vector2(0, 0);
    public static readonly Vector2 VEC_X = new Vector2(1, 0);
    public static readonly Vector2 VEC_Y = new Vector2(0, 1);
    public static readonly Vector2 UNDER = new Vector2(0, -0.501f);

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
    public static readonly TextMeshProUGUI GAME_TIME_TEXT = GameObject.Find("Time").GetComponent<TextMeshProUGUI>();

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