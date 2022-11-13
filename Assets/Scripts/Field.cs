using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field
{
    Puyo[,] ary;

    public Field()
    {
        ary = new Puyo[17, 8];
        for (int y = 0; y < ary.GetLength(0); y++)
        {
            for (int x = 0; x < ary.GetLength(1); x++)
            {
                if (y == 0 || y == ary.GetLength(0) - 1 ||
                    x == 0 || x == ary.GetLength(1) - 1)
                {
                    ary[y, x] = new Puyo(null, null);
                }
                else
                {
                    ary[y, x] = null;
                }
            }
        }
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
        for (int y = 1; y < ary.GetLength(0) - 1; y++)
        {
            for (int x = 1; x < ary.GetLength(1) - 1; x++)
            {
                ary[y, x] = null;
            }
        }
    }

    public void render()
    {
        for (int y = 1; y < ary.GetLength(0) - 1; y++)
        {
            for (int x = 1; x < ary.GetLength(1) - 1; x++)
            {
                if (ary[y, x] != null) ary[y, x].render();
            }
        }
    }
}
