using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field
{
    Puyo[,] ary;

    public Field()
    {
        ary = new Puyo[C.FIELD_SIZE_Y, C.FIELD_SIZE_X];
    }

    public void setPuyo(Puyo puyo)
    {
        Vector2 pos = puyo.getPos();
        ary[(int)pos.y, (int)pos.x] = puyo;
    }

    public Puyo getPuyo(Vector2 pos)
    {
        return ary[(int)pos.y, (int)pos.x];
    }

    public void reset()
    {
        for (int y = 0; y < C.FIELD_SIZE_Y; y++)
        {
            for (int x = 0; x < C.FIELD_SIZE_X; x++)
            {
                ary[y, x] = null;
            }
        }
    }
}
