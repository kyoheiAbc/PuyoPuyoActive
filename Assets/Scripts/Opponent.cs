using UnityEngine;
using TMPro;

public class Opponent
{
    int hp;
    int energy;
    int combo;
    int cnt;
    int atk;
    int max;
    TextMeshPro debugE;
    TextMeshPro debugH;

    private Opponent()
    {
        debugH = GameObject.Find("OpponentH").GetComponent<TextMeshPro>();
        init();
    }
    public void init()
    {
        hp = 100;
        combo = 0;
        energy = 0;
        cnt = 0;
        atk = 3;
        max = 1000 * 5;
        debugE = GameObject.Find("OpponentE").GetComponent<TextMeshPro>();

        // float tmp = (max - atk * 1000) / C.OPPONENT_SPEED;
        // p = (int)((tmp / 100) * (101 - C.OPPONENT_ACT_RATE));
    }

    public void incHp(int h)
    {
        hp += h;
        if (hp < 0) hp = 0;

    }
    public void incCombo()
    {
        if (energy < 1000)
        {
            combo = 0;
            return;
        }

        combo++;
        energy -= 1000;
        OjamaSystem.I().incDmgTmp(C.COMBO_TO_OJAMA(combo));
    }

    public void update()
    {
        // if (combo == 0 && energy / 1000 >= atk && UnityEngine.Random.Range(0, (int)(p * 1.1f)) == p)

        if (energy >= max)
        {
            incCombo();
        }

        if (combo == 0)
        {
            energy += C.OPPONENT_SPEED;
            if (energy >= max)
            {
                energy = max;
            }

            cnt = 0;

            OjamaSystem.I().setDmg();
            OjamaSystem.I().fixAtk();
        }
        else
        {
            cnt++;
            if (cnt > C.EFFECT_REMOVE_CNT + (6 / -(10 * C.VEC_DROP.y)) + C.EFFECT_FIX_CNT)
            {
                cnt = 0;
                if (combo >= atk)
                {
                    combo = 0;
                }
                else
                {
                    incCombo();
                }
            }

        }

        debugE.text = energy.ToString();

        debugH.text = hp.ToString();
    }


    private static Opponent i = new Opponent();
    public static Opponent I() { return i; }

}
