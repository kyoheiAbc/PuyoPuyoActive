using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Puyo
{
    private Vector2 pos;
    GameObject gO;
    Transform t;
    int color;
    float cnt;

    public Puyo(GameObject g)
    {
        gO = g;
        t = gO.transform;
        pos = t.position;
        switch (gO.name)
        {
            case "puyoA(Clone)": color = 0; break;
            case "puyoB(Clone)": color = 1; break;
            case "puyoC(Clone)": color = 2; break;
            case "puyoD(Clone)": color = 3; break;
            default: color = 255; break;
        }
        cnt = C.EFFECT_FIX_CNT;
    }

    public Vector2 update(List<Puyo> pList)
    {
        Vector2 retVec = move(C.VEC_DROP_QUICK, pList);
        if (retVec != C.VEC_0)
        {
            cnt = 0;
            return retVec;
        }

        if (cnt >= C.EFFECT_FIX_CNT)
        {
            if (color == 255)
            {
                cnt++;
            }
            return C.VEC_0;
        }

        cnt++;
        return C.VEC_0;
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

    public int getColor()
    {
        return color;
    }
    public void setColor(int c)
    {
        color = c;
    }

    public Vector2 getPos()
    {
        return pos;
    }

    public void setCnt(int c)
    {
        cnt = c;
    }
    public int getCnt()
    {
        return (int)cnt;
    }

    public void rm()
    {
        gO.tag = "REMOVE";
    }

    public void render()
    {
        if (color == 255)
        {
            if (cnt == C.EFFECT_FIX_CNT)
            {
                t.localScale = new Vector2(0.9f, 1.1f);
            }
            if (cnt == C.EFFECT_FIX_CNT + (int)(C.EFFECT_REMOVE_CNT / 3f))
            {
                t.localScale = C.VEC_0;
            }
            if (cnt == C.EFFECT_FIX_CNT + (int)(C.EFFECT_REMOVE_CNT * 2f / 3f))
            {
                t.localScale = new Vector2(0.9f, 1.1f);
            }
            return;
        }

        t.position = new Vector2(pos.x, pos.y - C.QuadraticF(cnt / C.EFFECT_FIX_CNT, 0.1f));
        t.localScale = new Vector2(1 + C.QuadraticF(cnt / C.EFFECT_FIX_CNT, 0.1f), 1);
    }

}
