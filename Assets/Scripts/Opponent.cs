using UnityEngine;
using TMPro;

public class Opponent
{
    int hp;
    int energy;
    int combo;
    int cnt;
    int atk;
    readonly int energyMax;
    TextMeshPro debugE;
    TextMeshPro debugH;

    private Opponent()
    {
        energyMax = C.OPPONENT_ATTACK[C.OPPONENT_ATTACK.Length - 1] * 1000;
        debugH = GameObject.Find("OpponentH").GetComponent<TextMeshPro>();
        debugE = GameObject.Find("OpponentE").GetComponent<TextMeshPro>();
        init();
    }
    public void init()
    {
        hp = C.OPPONENT_HP;
        energy = 0;
        combo = 0;
        cnt = 0;

        resetAtk();

    }

    public void resetAtk()
    {
        int[] atkAry = new int[C.OPPONENT_ATTACK.Length - 1];
        for (int i = 0; i < atkAry.Length; i++)
        {
            atkAry[i] = C.OPPONENT_ATTACK[i];
        }
        C.SHUFFLE(ref atkAry);
        atk = atkAry[0];
    }

    public void decHp(int h)
    {
        hp -= h;
        if (hp < 0) hp = 0;

        Debug.Log("dec hp: " + h);

    }
    public int getCombo()
    {
        return combo;
    }
    public int getAtk()
    {
        return atk;
    }
    public void setAtk(int a)
    {
        atk = a;
    }
    public void incCombo()
    {
        Debug.Log("incCombo");

        if (energy < 1000)
        {
            Debug.Log("incCombo cancel");

            combo = 0;
            return;
        }

        combo++;
        energy -= 1000;
        ScoreSystem.I().incTmp(1, C.COMBO_TO_OJAMA(combo));
        Debug.Log("combo: " + combo);

    }

    public void update()
    {
        if (combo == 0 && energy / 1000 >= atk)
        {
            float tmp = ((float)energyMax - (float)energy) / (float)C.OPPONENT_SPEED;
            float p = (int)((tmp / 100) * (101 - C.OPPONENT_ACT_RATE));
            if (UnityEngine.Random.Range(0, (int)p) == 0)
            {
                incCombo();

            }
        }
        if (energy >= energyMax)
        {
            atk = energyMax / 1000;
            incCombo();
        }

        if (combo == 0)
        {
            energy += C.OPPONENT_SPEED;
            if (energy >= energyMax)
            {
                energy = energyMax;
            }
            cnt = 0;
        }
        else
        {
            cnt++;
            if (cnt > C.EFFECT_REMOVE_CNT + (6 / -(10 * C.VEC_DROP.y)) + C.EFFECT_FIX_CNT)
            {
                cnt = 0;
                if (combo >= atk || energy < 1000)
                {
                    ScoreSystem.I().setScore(1);
                    resetAtk();
                    combo = 0;
                }
                else incCombo();
            }
        }




        debugE.text = energy.ToString();
        debugH.text = hp.ToString();
    }


    private static Opponent i = new Opponent();
    public static Opponent I() { return i; }

}
