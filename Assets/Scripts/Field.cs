using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field
{
    Puyo[,] ary;

    public Field()
    {
        ary = new Puyo[16, 8];
        for (int y = 0; y < ary.GetLength(0); y++)
        {
            for (int x = 0; x < ary.GetLength(1); x++)
            {
                ary[y, x] = null;
            }
        }
    }
    public int[] getSize()
    {
        return new int[2] { ary.GetLength(0), ary.GetLength(1) };
    }

    public void setPuyo(Puyo p)
    {
        Vector2 pos = p.getPos();
        ary[(int)pos.y, (int)pos.x] = p;
    }
}
