using UnityEngine;

public class Field
{
    Puyo[][] ary;
    Puyo[][] tmp;

    private Field()
    {
        ary = new Puyo[8][];
        for (int i = 0; i < 8; i++) ary[i] = new Puyo[15];

        tmp = new Puyo[8][];
        for (int i = 0; i < 8; i++) tmp[i] = new Puyo[15];
        init();
    }

    public void init()
    {
        for (int y = 0; y < 15; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                ary[x][y] = null;
                tmp[x][y] = null;
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
        for (int y = 0; y < 15; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                tmp[x][y] = ary[x][y];
            }
        }

        bool rm = false;
        for (int y = 0; y < 15; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                if (tmp[x][y] == null) continue;
                int cnt = 0;
                cntSameColor(ref cnt, x, y);
                if (cnt >= C.REMOVE_NUMBER)
                {
                    rmSameColor(x, y);
                    rm = true;
                }
            }
        }
        return rm;
    }

    private void cntSameColor(ref int cnt, int x, int y)
    {
        cnt++;
        int color = tmp[x][y].getColor();
        tmp[x][y] = null;

        for (int i = 0; i < 4; i++)
        {
            if (tmp[x + (1 - i) * ((i + 1) % 2)][y + (2 - i) * (i % 2)] == null) continue;
            if (tmp[x + (1 - i) * ((i + 1) % 2)][y + (2 - i) * (i % 2)].getColor() == color)
            {
                cntSameColor(ref cnt, x + (1 - i) * ((i + 1) % 2), y + (2 - i) * (i % 2));
            }
        }
    }
    private void rmSameColor(int x, int y)
    {
        int color = ary[x][y].getColor();
        ary[x][y].rm();

        for (int i = 0; i < 4; i++)
        {
            if (ary[x + (1 - i) * ((i + 1) % 2)][y + (2 - i) * (i % 2)] == null) continue;
            if (ary[x + (1 - i) * ((i + 1) % 2)][y + (2 - i) * (i % 2)].getColor() == color)
            {
                rmSameColor(x + (1 - i) * ((i + 1) % 2), y + (2 - i) * (i % 2));
            }
        }
    }

    private static Field i = new Field();
    public static Field I() { return i; }
}