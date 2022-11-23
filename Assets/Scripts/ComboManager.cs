using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComboManager
{

    int combo;
    int plus;
    int cnt;
    TextMeshPro text;
    Transform t;

    public ComboManager()
    {
        t = GameObject.Find("ComboUI").transform;
        text = GameObject.Find("ComboUI").GetComponent<TextMeshPro>();
        t.gameObject.GetComponent<MeshRenderer>().sortingOrder = 99;
        init();
        plus = 0;

    }

    public void init()
    {
        combo = 0;
        cnt = 0;
        text.text = "";
    }

    public void update()
    {
        if (cnt > 0)
        {
            cnt++;
        }
        if (cnt == C.COMBO_CNT) init();
    }

    public void setCombo(Vector2 pos)
    {
        combo = combo + plus;
        plus = 0;
        cnt = 0;
        text.text = combo + " COMBO";
        t.position = pos;
    }

    public void setPlus(int p)
    {
        plus = p;
    }
    public void setCnt(int c)
    {
        cnt = c;
    }

}
