using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComboManager
{

    int combo;
    int cnt;
    TextMeshPro text;
    Transform t;

    public ComboManager()
    {
        t = GameObject.Find("ComboUI").transform;
        text = GameObject.Find("ComboUI").GetComponent<TextMeshPro>();
        t.gameObject.GetComponent<MeshRenderer>().sortingOrder = 99;
        init();

    }

    public void init()
    {
        combo = 0;
        cnt = C.COMBO_CNT;
        text.text = "";

    }

    public void update()
    {
        if (0 < cnt && cnt < C.COMBO_CNT)
        {
            text.color = text.color - new Color(0, 0, 0, (1f / (float)(C.COMBO_CNT)));
            cnt++;
        }
        if (cnt == C.COMBO_CNT)
        {
            combo = 0;
            text.text = "";
        }
    }

    public void comboPlus(Vector2 pos)
    {
        combo++;
        text.text = combo + " COMBO";
        t.position = pos;
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
    }

    public void setCnt(int c)
    {
        cnt = c;
    }
    public int getCnt()
    {
        return cnt;
    }

}
