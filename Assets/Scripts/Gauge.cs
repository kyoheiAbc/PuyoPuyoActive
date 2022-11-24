using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauge
{
    GameObject gameObject;
    Transform transform;
    Transform[] children;
    float point;
    public Gauge(GameObject gO, Vector2 t)
    {
        gameObject = gO;
        transform = gO.transform;
        transform.localScale = t;
        children = new Transform[2];
        children[0] = transform.GetChild(0).gameObject.transform;
        children[1] = transform.GetChild(1).gameObject.transform;

        point = 100;
    }

    public void addPoint(float p)
    {
        point = point + p;
        if (point > 100) point = 100;
        if (point < 0) point = 0;
    }

    public void setUi()
    {
        Debug.Log(point);
        children[1].transform.localScale = new Vector2(point / 100f, 1);
        children[1].transform.localPosition = new Vector2(-(100f - point) / 200f, 0);
    }

}
