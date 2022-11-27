using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauge
{
    GameObject gameObject;
    Transform transform;
    Transform[] children;
    float max;
    float hp;
    public Gauge(GameObject gO, Vector2 t)
    {
        gameObject = gO;
        transform = gO.transform;
        transform.localScale = t;
        children = new Transform[2];
        children[0] = transform.GetChild(0).gameObject.transform;
        children[1] = transform.GetChild(1).gameObject.transform;

        max = (float)C.COMBO_TO_OJAMA(C.BOSS_HP) * 2f; // + 1f;
    }

    public void init()
    {
        hp = max;
        setUi();
    }

    public void addPoint(float h)
    {
        hp = hp + h;
        if (hp > max) hp = max;
        if (hp < 0) hp = 0;
    }

    public int getHp()
    {
        return (int)hp;
    }

    public void setUi()
    {
        children[1].transform.localScale = new Vector2(hp / max, 1);
        children[1].transform.localPosition = new Vector2(-(max - hp) / (2 * max), 0);
    }

}
