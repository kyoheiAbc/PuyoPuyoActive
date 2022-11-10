using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoPuyo
{
    Puyo p0, p1;

    public PuyoPuyo(Puyo p0_, Puyo p1_)
    {
        p0 = p0_;
        p1 = p1_;
    }

    public Vector2 move(Vector2 vec)
    {
        p0.move(vec);
        return p1.move(vec);
    }

}
