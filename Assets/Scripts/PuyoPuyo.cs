using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoPuyo
{
    Puyo[] puyo;
    int rot;
    Field field;
    PuyoManager puyoManager;

    public PuyoPuyo()
    {
        puyo = new Puyo[2] { new Puyo(), new Puyo() };
        rot = 0;
    }

    public void init(GameObject[] g, Field f, PuyoManager pM)
    {
        field = f;
        puyoManager = pM;
        for (int i = 0; i < 2; i++) puyo[i].init(g[i], field, puyoManager);
    }

    public Vector2 move(Vector2 vec)
    {
        Vector2 p = puyo[0].getPos();
        for (int i = 0; i < 2; i++)
        {
            if (puyo[i].move(vec) != vec)
            {
                sync(i);
                return puyo[0].getPos() - p;
            }
        }
        return puyo[0].getPos() - p;
    }

    public void sync(int index)
    {
        puyo[1 - index].setPos(
            puyo[index].getPos() +
            new Vector2(1 - rot, 0) * (1 - 2 * index)
        );
    }
}
