using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Puyo
{
    private Vector2 pos;
    GameObject gO;
    Transform t;
    int color;
    int ctrl;
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
        ctrl = 0;
        cnt = 0;
    }

    ~Puyo()
    {
        gO = null;
        t = null;
    }

    public Vector2 update(List<Puyo> pList)
    {


        Vector2 retVec = move(C.VEC_DROP_QUICK, pList);
        if (retVec != C.VEC_0)
        {
            cnt = 0;
            return retVec;
        };

        if (cnt == C.EFFECT_FIX_CNT + 1)
        {
            return C.VEC_0;
        }


        float x = cnt / C.EFFECT_FIX_CNT;
        float b = 0.5f;
        t.localScale = new Vector2(1, 1) * (-2 * ((x - b) * (x - b)) + 1.5f);

        cnt++;
        return C.VEC_255;
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

    public int getCtrl()
    {
        return ctrl;
    }
    public void setCtrl(int c)
    {
        ctrl = c;
    }

    public int getColor()
    {
        return color;
    }
    public void setColor(int c)
    {
        t.localScale = new Vector2(1.2f, 1.5f);
        color = c;
    }

    public Vector2 getPos()
    {
        return pos;
    }

    public void rm()
    {
        gO.tag = "REMOVE";
    }

    public GameObject getGameObject()
    {
        return gO;
    }

    public void render()
    {
        t.position = pos;
    }

}
