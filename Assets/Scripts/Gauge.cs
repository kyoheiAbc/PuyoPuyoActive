using UnityEngine;

public class Gauge
{
    Transform[] children;
    float max;
    float point;
    public Gauge(float m, Vector2 s, GameObject gO, Color c)
    {
        Transform t = gO.transform;
        t.localScale = s;
        children = new Transform[2];
        children[0] = t.GetChild(0).gameObject.transform;
        children[1] = t.GetChild(1).gameObject.transform;
        children[1].GetComponent<SpriteRenderer>().color = c;

        max = m;
        init();
    }

    public void init()
    {
        point = 0;
        setUi();
    }

    public void setPoint(float p)
    {
        point = p;
        if (point < 0) point = 0;
        setUi();
    }
    public float getPoint()
    {
        return point;
    }

    public void setUi()
    {
        children[1].transform.localScale = new Vector2(point / max, 1);
        children[1].transform.localPosition = new Vector2(-(max - point) / (2 * max), 0);
    }

}
