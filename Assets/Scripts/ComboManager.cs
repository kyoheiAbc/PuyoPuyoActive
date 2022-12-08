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
    Gauge gauge;

    public ComboManager(GameObject gO)
    {
        t = GameObject.Find("ComboUI").transform;
        text = GameObject.Find("ComboUI").GetComponent<TextMeshPro>();
        t.gameObject.GetComponent<MeshRenderer>().sortingOrder = 99;

        gO.transform.Rotate(0, 0, 90);
        gauge = new Gauge(C.COMBO_CNT, new Vector2(3f, 1f), gO, Color.magenta);

        init();
    }

    public void init()
    {
        combo = 0;
        cnt = 0;
        gauge.setPoint(cnt);
        text.text = "";
    }

    public void update()
    {
        if (C.COMBO_CNT > cnt) cnt--;
        gauge.setPoint(cnt);
        if (cnt == 0) init();
    }

    public void setCombo(Vector2 pos)
    {
        combo = combo + plus;
        plus = 0;
        cnt = C.COMBO_CNT;
        gauge.setPoint(cnt);

        text.text = combo + " COMBO";
        t.position = pos;
    }

    public int getCombo()
    {
        return combo;
    }

    public void setPlus(int p)
    {
        plus = p;
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
