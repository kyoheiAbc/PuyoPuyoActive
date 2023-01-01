using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

public class Field
{
    Puyo[][] ary;
    Explosion[][] aryE;

    private Field()
    {
        ary = new Puyo[8][];
        for (int i = 0; i < 8; i++) ary[i] = new Puyo[15];

        aryE = new Explosion[8][];
        for (int x = 0; x < 8; x++)
        {
            aryE[x] = new Explosion[15];
            for (int y = 0; y < 15; y++)
            {
                if (x == 0 || x == 7) continue;
                if (y == 0 || y == 13 || y == 14) continue;
                aryE[x][y] = new Explosion(new Vector2(x + 0.5f, y + 0.5f));
            }
        }

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

    public void setExp(int color, Vector2 pos)
    {
        aryE[(int)pos.x][(int)pos.y].start(color);
    }

    public Puyo getPuyo(Vector2 pos)
    {
        return ary[(int)pos.x][(int)pos.y];
    }

    public bool tryRm()
    {
        bool rm = false;
        for (int y = 1; y < 14; y++)
        {
            for (int x = 1; x < 7; x++)
            {
                if (ary[x][y] == null) continue;
                int cnt = 0;
                List<int> donePos = new List<int>();
                cntGroup(x, y, ref cnt, ref donePos);
                if (cnt >= C.REMOVE_NUMBER)
                {
                    rmGroup(x, y);
                    rm = true;
                    ComboManager.I().incTmp();
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
            int color_ = ary[x_][y_].getColor();
            if (color_ == 5) continue;
            if (color_ == color) cntGroup(x_, y_, ref cnt, ref donePos);
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
            int color_ = ary[x_][y_].getColor();
            if (color_ == 5) ary[x_][y_].rm();
            if (color_ == color) rmGroup(x_, y_);
        }
    }

    public void update()
    {
        for (int y = 0; y < 15; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                if (aryE[x][y] == null) continue;
                aryE[x][y].update();
            }
        }
    }

    private static Field i = new Field();
    public static Field I() { return i; }
}

public class Explosion
{
    readonly GameObject gameObject;
    readonly Transform transform;
    readonly Transform[] children;
    int cnt = 0;
    public Explosion(Vector2 p)
    {
        gameObject = Addressables.InstantiateAsync("Assets/Sources/Explosion.prefab").WaitForCompletion();

        transform = gameObject.transform;
        transform.position = p;

        children = new Transform[4];
        for (int i = 0; i < 4; i++)
        {
            children[i] = transform.GetChild(i).transform;
        }

        init();
    }

    public void init()
    {
        cnt = 0;
        gameObject.SetActive(false);
    }

    public void start(int color)
    {
        cnt = 1;

        gameObject.SetActive(true);

        transform.Rotate(0, 0, UnityEngine.Random.Range(0, 5) * 15);

        for (int i = 0; i < 4; i++)
        {
            children[i].GetComponent<SpriteRenderer>().color = C.PUYO_COLORS[color];
            children[i].localPosition = -new Vector2(0.5f, 0.5f)
            + new Vector2(0.5f, 0) * UnityEngine.Random.Range(0, 3)
            + new Vector2(0, 0.5f) * UnityEngine.Random.Range(0, 3);
        }
    }

    public void update()
    {
        if (cnt == 0) return;

        cnt++;

        for (int i = 0; i < 4; i++)
        {
            children[i].localPosition = (Vector2)children[i].localPosition
                                        + new Vector2(0.25f, 0) * (1 - i) * ((i + 1) % 2)
                                        + new Vector2(0, 0.25f) * (2 - i) * (i % 2);
        }

        if (cnt > 20)
        {
            cnt = 0;
            gameObject.SetActive(false);
        }
    }

}