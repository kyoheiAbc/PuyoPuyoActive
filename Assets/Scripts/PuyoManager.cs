using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuyoManager
{
    List<Puyo> puyoList;

    public PuyoManager()
    {
        puyoList = new List<Puyo>();
    }

    public bool update()
    {
        bool update = false;
        for (int i = 0; i < puyoList.Count; i++)
        {
            if (C.VEC_DROP_QUICK == puyoList[i].move(C.VEC_DROP_QUICK)) update = true;
            else
            {
                puyoList[i].setToField();
                puyoList.Remove(puyoList[i]);
                i--;
            }
        }
        return update;
    }

    public void addPuyo(Puyo puyo)
    {
        puyoList.Add(puyo);
    }

    public void reset()
    {
        puyoList.Clear();
    }

    public void render()
    {
        for (int i = 0; i < puyoList.Count; i++) puyoList[i].render();
    }
}
