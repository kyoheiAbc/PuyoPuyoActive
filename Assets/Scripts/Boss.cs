using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss
{
    float hp;
    float hpMax;

    float sp;
    float spMax;

    int ak;

    Gauge[] gauge;

    public Boss(float hp_, float sp_, int a, GameObject[] gO)
    {
        hpMax = hp_;
        hp = hpMax;

        spMax = sp_;
        sp = spMax;

        ak = a;

        gauge = new Gauge[2]{
            new Gauge(hp, new Vector2(4, 0.25f), gO[0], Color.green),
            new Gauge(sp, new Vector2(4, 0.25f), gO[1], Color.yellow)
        };
    }
    public void init()
    {
        hp = hpMax;
        sp = 0;
        for (int i = 0; i < 2; i++)
        {
            gauge[i].init();
        }
        gauge[1].setPoint(0);
    }

    public int update()
    {
        sp++;
        if (sp > spMax)
        {
            sp = -1;
            return 1;
        }

        gauge[1].setPoint(sp);
        return 0;
    }

    public void setHp(float h)
    {
        hp = h;
        if (hp < 0) hp = 0;
        gauge[0].setPoint(hp);
    }
    public float getHp()
    {
        return hp;
    }

    public int getAtk()
    {
        return ak;
    }

}
