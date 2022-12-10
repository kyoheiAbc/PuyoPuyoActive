using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss
{
    int cnt;
    int combo;
    int saisoku;
    Gauge[] gauge;

    public Boss(GameObject[] gO)
    {
        gauge = new Gauge[3]{
            new Gauge(C.BOSS_HP, new Vector2(5, 0.25f), gO[0], Color.green),
            new Gauge(C.BOSS_SPEED*C.BOSS_ATTACK_GAUGE_MAX, new Vector2(5, 0.25f), gO[1], Color.yellow),
            new Gauge(C.BOSS_SPEED, new Vector2(0, 0), gO[2], Color.white),
        };
    }

    public void init()
    {
        cnt = 0;
        combo = 0;
        saisoku = 0;
        for (int i = 0; i < 3; i++)
        {
            gauge[i].init();
        }
        gauge[0].setPoint(C.BOSS_HP);
        gauge[1].cover((C.BOSS_ATTACK_GAUGE_MAX - C.BOSS_ATTACK) * C.BOSS_SPEED);
    }

    public int update()
    {
        cnt++;
        if (cnt > C.BOSS_SPEED * C.BOSS_ATTACK)
        {
            if (gauge[1].getPoint() == C.BOSS_SPEED * C.BOSS_ATTACK)
            {
                gauge[1].setUiTmp(0);
                saisoku = C.BOSS_ATTACK;
            }

            if (cnt > C.BOSS_SPEED * C.BOSS_ATTACK + C.EFFECT_FIX_CNT + C.EFFECT_REMOVE_CNT + (C.FIELD_SIZE_Y * 0.5f) / -C.VEC_DROP_QUICK.y)
            {
                if (gauge[1].getPoint() < 1 || saisoku == 0)
                {
                    combo = 0;
                    cnt = (int)gauge[1].getPoint();
                }
                else
                {
                    saisoku--;
                    combo++;
                    gauge[1].setPoint(gauge[1].getPoint() - C.BOSS_SPEED);
                    cnt = (int)C.BOSS_SPEED * C.BOSS_ATTACK;
                    return combo;
                }
            }
            return 0;
        }


        if (UnityEngine.Random.Range(0, (int)C.BOSS_SPEED * C.BOSS_ATTACK) == 0)
        {
            int s = UnityEngine.Random.Range((int)C.BOSS_ATTACK / 3, (int)C.BOSS_ATTACK / 2 + 1);
            if (cnt / C.BOSS_SPEED > s)
            {
                gauge[1].setUiTmp(gauge[1].getPoint() - C.BOSS_SPEED * s);
                saisoku = s;
                cnt = (int)C.BOSS_SPEED * C.BOSS_ATTACK;
                return 0;
            }
        }


        gauge[1].setPoint(cnt);
        return 0;
    }

    public int getCombo()
    {
        return combo;
    }

    public void setHp(float h)
    {
        gauge[0].setPoint(h > 0 ? h : 0);
    }
    public float getHp()
    {
        return gauge[0].getPoint();
    }

}
