using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss
{
    float hp;
    float hpMax;

    float sp;
    float spMax;

    float maskSp;
    float maskSpMax;

    int ak;

    Gauge[] gauge;

    OjamaManager ojamaManager;

    public Boss(float hp_, float sp_, int a, float maskS, GameObject[] gO, OjamaManager oM)
    {
        hpMax = hp_;
        hp = hpMax;

        spMax = sp_;
        sp = spMax;

        maskSpMax = maskS;
        maskSp = maskSpMax;

        ak = a;

        gauge = new Gauge[3]{
            new Gauge(hp, new Vector2(4, 0.25f), gO[0], Color.green),
            new Gauge(sp, new Vector2(4, 0.25f), gO[1], Color.yellow),
            new Gauge(maskSp, new Vector2(4, 0.25f), gO[2], Color.white),
        };

        ojamaManager = oM;
    }
    public void init()
    {
        hp = hpMax;
        sp = 0;
        maskSp = 0;
        for (int i = 0; i < 2; i++)
        {
            gauge[i].init();
        }
        gauge[1].setPoint(0);
    }

    public int update()
    {
        maskSp++;
        if (maskSp < maskSpMax)
        {
            gauge[2].setPoint(maskSp);
        }
        else if (maskSp == maskSpMax)
        {
            gauge[2].setPoint(maskSp);
            ojamaManager.setOjmField(C.BOSS_MASK_NUM);
        }
        else if (maskSp > maskSpMax)
        {
            gauge[2].setPoint(0);
            if (maskSp == C.BOSS_MASK_TIME + maskSpMax)
            {
                maskSp = 0;
                ojamaManager.init(false);
            }
        }

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
