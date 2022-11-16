using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Puyo
{
    Vector2 pos;
    GameObject gO;
    Transform t;

    public Puyo(GameObject g)
    {
        gO = g;
        t = gO.transform;
        pos = t.position;
    }

    ~Puyo()
    {
        gO = null;
        t = null;
    }

    public bool update(List<Puyo> pList)
    {
        return C.VEC_DROP_QUICK == move(C.VEC_DROP_QUICK, pList);
    }

    public void setPos(Vector2 p)
    {
        pos = p;
    }

    public Vector2 move(Vector2 vec, List<Puyo> pList)
    {
        Vector2 initPos = pos;

        pos += vec;

        for (int i = 0; i < pList.Count; i++)
        {
            Puyo p = pList[i];

            if (p == this) continue;
            if (Vector2.SqrMagnitude(pos - p.getPos()) >= 1) continue;


            if (vec.x != 0)
            {
                if (Mathf.Abs(pos.y - p.getPos().y) < 0.25)
                {
                    pos = initPos;
                    return pos - initPos;
                }
                pos.y = p.getPos().y + C.VEC_Y.y * Mathf.Sign(pos.y - p.getPos().y);
                if (!canPut(pList))
                {
                    pos = initPos;
                    return pos - initPos;
                }
            }
            else
            {
                pos.y = p.getPos().y - C.VEC_Y.y * Mathf.Sign(vec.y);
            }
        }

        return pos - initPos;
    }

    public bool canPut(List<Puyo> pList)
    {
        for (int i = 0; i < pList.Count; i++)
        {
            if (pList[i] == this) continue;
            if (Vector2.SqrMagnitude(pos - pList[i].getPos()) < 1) return false;
        }
        return true;
    }

    public Vector2 getPos()
    {
        return pos;
    }

    public void render()
    {
        t.position = pos;
    }

}
