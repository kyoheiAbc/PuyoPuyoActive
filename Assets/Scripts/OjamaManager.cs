using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class OjamaManager
{
    // GameObject[,] ary;
    int atk, atkTmp;
    int dmg, dmgTmp;
    TextMeshPro text;

    public OjamaManager(GameObject[,] g)
    {
        // ary = g;
        text = GameObject.Find("OjamaUI").GetComponent<TextMeshPro>();
        init();
    }

    public void init()
    {
        // for (int y = 0; y < 12; y++)
        // {
        //     for (int x = 0; x < 6; x++)
        //     {
        //         ary[y, x].SetActive(a);
        //     }
        // }
        text.text = "";
        atk = 0;
        atkTmp = 0;
        dmg = 0;
        dmgTmp = 0;
    }

    public void update()
    {
        text.text = ((atk + atkTmp) - (dmg + dmgTmp)).ToString();
    }
    public void addAtkTmp(int a)
    {
        atkTmp += a;
    }
    public void addDmgTmp(int d)
    {
        dmgTmp += d;
    }
    public void fixAtk()
    {
        atk += atkTmp;
        atkTmp = 0;
    }
    public void fixDmg()
    {
        dmg += dmgTmp;
        dmgTmp = 0;
    }
    public int getAtk()
    {
        return atk;
    }
    public int getDmg()
    {
        return dmg;
    }

    public void setAtk(int a)
    {
        atk = a;
    }
    public void setDmg(int d)
    {
        dmg = d;
    }

    // public void setOjmField(int num)
    // {
    //     init(false);
    //     for (int n = 0; n < num; n++)
    //     {
    //         GameObject gO = ary[UnityEngine.Random.Range(0, 12), UnityEngine.Random.Range(0, 6)];
    //         gO.SetActive(true);
    //     }
    // }


}