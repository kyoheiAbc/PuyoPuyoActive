using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss
{
    float hp;
    float hpMax;

    Gauge gauge;

    public Boss(float hp_, GameObject gO)
    {
        hpMax = hp_;
        hp = hpMax;
        gauge = new Gauge(hp, new Vector2(4, 0.25f), gO);
    }
    public void init()
    {
        hp = hpMax;
        gauge.init();
    }

    public void setHp(float h)
    {
        hp = h;
        if (hp < 0) hp = 0;
        gauge.setPoint(hp);
    }
    public float getHp()
    {
        return hp;
    }



}
