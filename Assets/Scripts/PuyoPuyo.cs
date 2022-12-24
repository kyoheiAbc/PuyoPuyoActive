using System.Collections.Generic;
using UnityEngine;

public class PuyoPuyo
{
    List<Puyo> puyos;
    int rot;
    int cnt;

    public PuyoPuyo(Puyo p0, Puyo p1)
    {
        puyos = new List<Puyo> { p0, p1 };
        for (int i = 0; i < 2; i++)
        {
            puyos[i].setParent(this);
        }
        rot = 3;
        cnt = 0;
    }

    public void setCnt(int c)
    {
        cnt = c;
    }

    public void setPos(Vector2 pos)
    {
        puyos[0].setPos(pos);
        sync(0);
    }

    private void sync(int i)
    {
        puyos[1 - i].setPos(
            puyos[i].getPos() +
            new Vector2(1 - rot, rot - 2) * new Vector2((rot + 1) % 2, rot % 2) * (1 - 2 * i)
        );
    }

    public Vector2 move(Vector2 vec)
    {
        Vector2 initPos = puyos[0].getPos();
        for (int i = 0; i < 2; i++)
        {
            if (puyos[i].move(vec) != vec)
            {
                sync(i);
                if (!puyos[1 - i].canPut())
                {
                    puyos[0].setPos(initPos);
                    sync(0);
                }
                break;
            }
        }

        return puyos[0].getPos() - initPos;
    }

    public void rotate(int r)
    {
        rot = (rot + r) % 4;
        if (rot < 0) rot = 3;

        Vector2 pos = puyos[0].getPos();
        puyos[1].setPos(pos);
        puyos[1].move(new Vector2(1 - rot, rot - 2) * new Vector2((rot + 1) % 2, rot % 2));
        sync(1);

        if (!puyos[0].canPut())
        {
            rot = (rot - r) % 4;
            if (rot < 0) rot = 3;

            rot = (rot + 2) % 4;
            puyos[1].setPos(pos);
            sync(1);
        }
    }

    public bool update()
    {
        if (move(C.VEC_DROP) == C.VEC_DROP)
        {
            cnt = 0;
            return true;
        }

        cnt++;
        if (cnt > C.FIX_CNT)
        {
            for (int i = 0; i < 2; i++)
            {
                puyos[i].setParent(null);
            }
            return false;
        }

        return true;
    }
}
