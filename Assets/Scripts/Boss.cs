using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss
{
    int cnt;
    int combo;
    Gauge[] gauge;

    public Boss(GameObject[] gO)
    {
        gauge = new Gauge[3]{
            new Gauge(C.BOSS_HP, new Vector2(5, 0.25f), gO[0], Color.green),
            new Gauge(C.BOSS_SPEED, new Vector2(5, 0.25f), gO[1], Color.yellow),
            new Gauge(C.BOSS_SPEED, new Vector2(0, 0), gO[2], Color.white),
        };
    }

    public void init()
    {
        cnt = 0;
        combo = 0;
        for (int i = 0; i < 2; i++)
        {
            gauge[i].init();
        }
        gauge[0].setPoint(C.BOSS_HP);
    }

    public int update()
    {
        cnt++;
        if (cnt > C.BOSS_SPEED)
        {
            if (cnt > C.BOSS_SPEED + C.EFFECT_FIX_CNT + C.EFFECT_REMOVE_CNT + (C.FIELD_SIZE_Y * 0.5f) / -C.VEC_DROP_QUICK.y)
            {
                if (gauge[1].getPoint() < 1)
                {
                    combo = 0;
                    cnt = 0;
                }
                else
                {
                    combo++;
                    gauge[1].setPoint(gauge[1].getPoint() - C.BOSS_SPEED / C.BOSS_ATTACK);
                    cnt = (int)C.BOSS_SPEED;
                    return combo;
                }
            }
            return 0;
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
