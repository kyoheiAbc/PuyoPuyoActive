using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field
{
    Puyo[,] ary;

    public Field()
    {
        ary = new Puyo[15, 8];
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

    private void rmPuyo(Puyo p)
    {
        Vector2 pos = p.getPos();
        ary[(int)pos.y, (int)pos.x] = null;
    }
    public bool rmCheck()
    {
        bool remove = false;
        for (int y = 1; y < 15 - 1; y++)
        {
            for (int x = 1; x < 8 - 1; x++)
            {
                int cnt = 0;
                if (cntSameColor(ary[y, x], cnt) >= C.REMOVE_NUMBER)
                {
                    remove = true;
                    rmSameColor(ary[y, x]);
                }
            }
        }
        for (int y = 1; y < 15 - 1; y++)
        {
            for (int x = 1; x < 8 - 1; x++)
            {
                if (ary[y, x] == null) continue;
                int color = ary[y, x].getColor();
                if (color != 255 && color >= 100)
                {
                    ary[y, x].setColor(color - 100);
                }
            }
        }
        return remove;
    }


    public void rm()
    {
        for (int y = 1; y < 15 - 1; y++)
        {
            for (int x = 1; x < 8 - 1; x++)
            {
                if (ary[y, x] == null) continue;

                if (ary[y, x].getColor() == 255)
                {
                    ary[y, x] = null;
                    continue;
                }


                Vector2 p = ary[y, x].getPos() - new Vector2(0, 0.5f + C.RESOLUTION);
                if (getPuyo(p) == null) rmPuyo(ary[y, x]);

            }
        }
    }

    private int cntSameColor(Puyo p, int cnt)
    {
        if (p == null) return cnt;
        int color = p.getColor();
        if (color >= 100) return cnt;

        cnt++;

        Puyo[] rtlb = getRtlb(p);
        p.setColor(color + 100);

        for (int i = 0; i < 4; i++)
        {
            if (rtlb[i] == null) continue;
            if (color == rtlb[i].getColor())
            {
                cnt = cntSameColor(rtlb[i], cnt);
            }
        }
        return cnt;
    }

    private void rmSameColor(Puyo p)
    {
        if (p == null) return;
        int color = p.getColor();
        if (color == 255) return;
        p.setColor(255);

        Puyo[] rtlb = getRtlb(p);
        for (int i = 0; i < 4; i++)
        {
            if (rtlb[i] == null) continue;
            if (color == rtlb[i].getColor()) rmSameColor(rtlb[i]);
        }
    }

    private Puyo[] getRtlb(Puyo puyo)
    {
        Puyo[] rtlb = new Puyo[4];
        for (int i = 0; i < 4; i++)
        {
            rtlb[i] = getPuyo(
                puyo.getPos() + new Vector2(1, 0) * (1 - i) * ((i + 1) % 2) + new Vector2(0, 1) * (2 - i) * (i % 2)
            );
        }
        return rtlb;
    }

    public void init()
    {
        for (int y = 0; y < 15; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                ary[y, x] = null;
            }
        }
    }
}
