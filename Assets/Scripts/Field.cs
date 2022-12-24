using UnityEngine;

public class Field
{
    Puyo[,] ary;
    public Field()
    {
        ary = new Puyo[15, 8];
    }

    public void init(Puyo[,] pA)
    {
        for (int y = 0; y < ary.GetLength(0); y++)
        {
            for (int x = 0; x < ary.GetLength(1); x++)
            {
                ary[y, x] = pA == null ? null : pA[y, x];
            }
        }
    }

    public void set(Puyo puyo, Vector2 pos)
    {
        ary[(int)pos.y, (int)pos.x] = puyo;
    }
    public Puyo getPuyo(Vector2 pos)
    {
        return ary[(int)pos.y, (int)pos.x];
    }

    public bool tryRm()
    {
        Field f = new Field();
        f.init(ary);

        bool rm = false;
        for (int y = 0; y < ary.GetLength(0); y++)
        {
            for (int x = 0; x < ary.GetLength(1); x++)
            {
                if (ary[y, x] == null) continue;

                int cnt = 0;
                if (f.cntSameColor(cnt, ary[y, x]) >= C.REMOVE_NUMBER)
                {
                    rm = true;
                    rmSameColor(ary[y, x]);
                }
            }
        }
        return rm;
    }

    private int cntSameColor(int cnt, Puyo p)
    {
        cnt++;
        set(null, p.getPos());

        Puyo[] rtlb = getRtlb(p);
        for (int i = 0; i < 4; i++)
        {
            if (rtlb[i] == null) continue;
            if (rtlb[i].getColor() == p.getColor())
            {
                cnt = cntSameColor(cnt, rtlb[i]);
            }
        }
        return cnt;
    }

    private void rmSameColor(Puyo p)
    {
        p.rm();

        Puyo[] rtlb = getRtlb(p);
        for (int i = 0; i < 4; i++)
        {
            if (rtlb[i] == null) continue;
            if (rtlb[i].getColor() == p.getColor())
            {
                rmSameColor(rtlb[i]);
            }
        }
    }

    private Puyo[] getRtlb(Puyo puyo)
    {
        Puyo[] rtlb = new Puyo[4];
        for (int i = 0; i < 4; i++)
        {
            Vector2 pos = puyo.getPos() + new Vector2(1, 0) * (1 - i) * ((i + 1) % 2) + new Vector2(0, 1) * (2 - i) * (i % 2);
            rtlb[i] = ary[(int)pos.y, (int)pos.x];
        }
        return rtlb;
    }
}