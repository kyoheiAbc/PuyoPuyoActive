using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OjamaSystem
{
    int atkTmp;
    int point;
    int dmgTmp;
    TextMeshPro text;

    private OjamaSystem()
    {
        init();
        text = GameObject.Find("Ojama").GetComponent<TextMeshPro>();
    }

    public void init()
    {
        atkTmp = 0;
        point = 0;
        dmgTmp = 0;
    }

    public void incAtkTmp(int a)
    {
        atkTmp += a;
    }

    public void setAtk()
    {
        point += atkTmp;
        atkTmp = 0;
    }

    public void fixAtk()
    {
        if (point > 0)
        {
            Opponent.I().incHp(-point);
            point = 0;
        }
    }

    public void incDmgTmp(int d)
    {
        dmgTmp += d;
    }

    public void setDmg()
    {
        point -= dmgTmp;
        dmgTmp = 0;
    }

    public void fixDmg()
    {
        if (point < 0)
        {
            PuyoManager.I().newOjama(-point);
            point = 0;
        }
    }

    public void update()
    {
        text.text = (point + atkTmp - dmgTmp).ToString();
    }

    private static OjamaSystem i = new OjamaSystem();
    public static OjamaSystem I() { return i; }
}
