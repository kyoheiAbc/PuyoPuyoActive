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

    public void rmPuyo(Puyo p)
    {
        Vector2 pos = p.getPos();
        ary[(int)pos.y, (int)pos.x] = null;
        p.rm();
    }
    public bool rmCheck(List<Puyo> pList)
    {
        bool remove = false;
        for (int y = 1; y < C.FIELD_SIZE_Y - 1; y++)
        {
            for (int x = 1; x < C.FIELD_SIZE_X - 1; x++)
            {
                int cnt = 0;
                if (cntSameColor(ary[y, x], cnt) >= C.REMOVE_NUMBER)
                {
                    remove = true;
                    Debug.Log("remove true");

                    rmSameColor(ary[y, x], pList);
                }
            }
        }
        for (int y = 1; y < C.FIELD_SIZE_Y - 1; y++)
        {
            for (int x = 1; x < C.FIELD_SIZE_X - 1; x++)
            {
                if (ary[y, x] == null) continue;
                ary[y, x].setCtrl(0);
                if (getPuyo(ary[y, x].getPos() - C.VEC_Y) != null) continue;
                ary[y, x] = null;
            }
        }
        return remove;
    }


    public void rm()
    {
        for (int y = 1; y < C.FIELD_SIZE_Y - 1; y++)
        {
            for (int x = 1; x < C.FIELD_SIZE_X - 1; x++)
            {
                if (ary[y, x] == null) continue;

                if (ary[y, x].getColor() == 255)
                {
                    ary[y, x] = null;
                }
            }
        }
    }

    private int cntSameColor(Puyo p, int cnt)
    {
        if (p == null) return cnt;
        int color = p.getColor();
        Debug.Log(color);
        if (color == 255) return cnt;
        if (p.getCtrl() == 255) return cnt;

        cnt++;

        Puyo[] rtlb = getRtlb(p);
        p.setCtrl(255);

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

    private void rmSameColor(Puyo p, List<Puyo> pList)
    {
        if (p == null) return;
        int color = p.getColor();
        if (color == 255) return;
        p.setColor(255);


        // rmPuyo(p);
        // pList.Remove(p);
        // p.rm();
        Puyo[] rtlb = getRtlb(p);
        for (int i = 0; i < 4; i++)
        {
            if (rtlb[i] == null) continue;
            if (color == rtlb[i].getColor()) rmSameColor(rtlb[i], pList);
        }
    }

    public Puyo[] getRtlb(Puyo puyo)
    {
        Puyo[] rtlb = new Puyo[4];
        for (int i = 0; i < 4; i++)
        {
            rtlb[i] = getPuyo(
                puyo.getPos() + C.VEC_X * (1 - i) * ((i + 1) % 2) + C.VEC_Y * (2 - i) * (i % 2)
            );
        }
        return rtlb;
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
