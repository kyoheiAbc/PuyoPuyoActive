using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauge
{
    GameObject gameObject;
    Transform transform;
    Transform[] children;
    float max;
    float point;
    public Gauge(float m, Vector2 t, GameObject gO)
    {
        gameObject = gO;
        transform = gO.transform;
        transform.localScale = t;
        children = new Transform[2];
        children[0] = transform.GetChild(0).gameObject.transform;
        children[1] = transform.GetChild(1).gameObject.transform;

        max = m;
        init();
    }

    public void init()
    {
        point = max;
        setUi();
    }

    public void setUi()
    {
        children[1].transform.localScale = new Vector2(point / max, 1);
        children[1].transform.localPosition = new Vector2(-(max - point) / (2 * max), 0);
    }

}
