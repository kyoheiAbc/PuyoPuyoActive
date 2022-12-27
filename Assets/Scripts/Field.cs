using UnityEngine;
using System.Collections.Generic;

public class Field
{
    Puyo[][] ary;

    private Field()
    {
        ary = new Puyo[8][];
        for (int i = 0; i < 8; i++) ary[i] = new Puyo[15];
        init();
    }

    public void init()
    {
        for (int y = 0; y < 15; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                ary[x][y] = null;
            }
        }
    }

    public void set(Puyo puyo, Vector2 pos)
    {
        ary[(int)pos.x][(int)pos.y] = puyo;
    }

    public Puyo getPuyo(Vector2 pos)
    {
        return ary[(int)pos.x][(int)pos.y];
    }

    public bool tryRm()
    {
        bool rm = false;
        for (int y = 0; y < 15; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                if (ary[x][y] == null) continue;
                int cnt = 0;
                List<int> donePos = new List<int>();
                cntGroup(x, y, ref cnt, ref donePos);
                if (cnt >= C.REMOVE_NUMBER)
                {
                    rmGroup(x, y);
                    rm = true;
                }
            }
        }
        return rm;
    }

    private void cntGroup(int x, int y, ref int cnt, ref List<int> donePos)
    {
        cnt++;
        int color = ary[x][y].getColor();
        donePos.Add(x + 8 * y);
        for (int i = 0; i < 4; i++)
        {
            int x_ = x + (1 - i) * ((i + 1) % 2);
            int y_ = y + (2 - i) * (i % 2);
            if (ary[x_][y_] == null) continue;
            if (donePos.Contains(x_ + 8 * y_)) continue;
            if (ary[x_][y_].getColor() == color) cntGroup(x_, y_, ref cnt, ref donePos);
        }
    }
    private void rmGroup(int x, int y)
    {
        int color = ary[x][y].getColor();
        ary[x][y].rm();
        for (int i = 0; i < 4; i++)
        {
            int x_ = x + (1 - i) * ((i + 1) % 2);
            int y_ = y + (2 - i) * (i % 2);
            if (ary[x_][y_] == null) continue;
            if (ary[x_][y_].getColor() == color) rmGroup(x_, y_);
        }
    }

    private static Field i = new Field();
    public static Field I() { return i; }
}