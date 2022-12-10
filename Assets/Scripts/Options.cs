using UnityEngine;
using System.IO;

public sealed class D
{
    public float BOSS_HP = 72;
    public int BOSS_ATTACK_GAUGE_MAX = 7;
    public int BOSS_ATTACK = 7;
    public float BOSS_SPEED = 90;
    public int COMBO_CNT = 10;
    public int COLOR_NUMBER = 4;
    public int REMOVE_NUMBER = 4;


    public void cp(D d)
    {
        BOSS_HP = d.BOSS_HP;
        BOSS_ATTACK_GAUGE_MAX = d.BOSS_ATTACK_GAUGE_MAX;
        BOSS_ATTACK = d.BOSS_ATTACK;
        BOSS_SPEED = d.BOSS_SPEED;
        COMBO_CNT = d.COMBO_CNT;
        COLOR_NUMBER = d.COLOR_NUMBER;
        REMOVE_NUMBER = d.REMOVE_NUMBER;
    }


    public readonly int FPS = 30;
    public readonly int NEXT_GAME_CNT = 90;
    public readonly int FIX_CNT = 30;
    public readonly int EFFECT_FIX_CNT = 10;
    public readonly int EFFECT_REMOVE_CNT = 30;
    public readonly Vector2 VEC_DROP = new Vector2(0, -0.03f);
    public readonly Vector2 VEC_DROP_QUICK = new Vector2(0, -0.4f);
    public readonly int FIELD_SIZE_X = 8;
    public readonly int FIELD_SIZE_Y = 17;
    public readonly int COLOR_ADJUST = 8;
    public readonly Vector2 VEC_0 = new Vector2(0, 0);
    public readonly Vector2 VEC_X = new Vector2(1, 0);
    public readonly Vector2 VEC_Y = new Vector2(0, 1);
    public readonly Vector2 UNDER = new Vector2(0, -0.501f);
    public readonly GameObject[] PUYO = new GameObject[10] {
        Resources.Load<GameObject>("puyoA"),
        Resources.Load<GameObject>("puyoB"),
        Resources.Load<GameObject>("puyoC"),
        Resources.Load<GameObject>("puyoD"),
        Resources.Load<GameObject>("puyoE"),
        Resources.Load<GameObject>(""),
        Resources.Load<GameObject>(""),
        Resources.Load<GameObject>(""),
        Resources.Load<GameObject>(""),
        Resources.Load<GameObject>("puyoZ")
    };
    public readonly GameObject EFFECT_EXPLOSION = Resources.Load<GameObject>("EffectExplosion");
    public readonly GameObject GAUGE = Resources.Load<GameObject>("Gauge");
    // public static readonly TextMeshProUGUI GAME_TIME_TEXT = GameObject.Find("Time").GetComponent<TextMeshProUGUI>();


    private static D d = new D();
    private D() { }
    public static D I() { return d; }
}

public class Options
{
    public Options()
    {
        D D = D.I();
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "gameParameters.json"), JsonUtility.ToJson(D, true));
        if (System.IO.File.Exists(Path.Combine(Application.persistentDataPath, "gameParameters.json")))
        {
            string read = File.ReadAllText(Path.Combine(Application.persistentDataPath, "gameParameters.json"));
            try { D = JsonUtility.FromJson<D>(read); }
            catch { return; }
            D.I().cp(D);
        }
        else File.WriteAllText(Path.Combine(Application.persistentDataPath, "gameParameters.json"), JsonUtility.ToJson(D, true));
    }
}
