using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss
{
    float hp;
    Gauge gauge;

    public Boss(float hp_, GameObject gO)
    {
        hp = hp_;
        gauge = new Gauge(hp, new Vector2(4, 0.25f), gO);
    }
}
