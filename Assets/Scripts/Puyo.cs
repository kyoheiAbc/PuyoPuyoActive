using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Puyo
{
    readonly int color;
    private Vector2 pos;
    GameObject gO;
    Transform t;
    PuyoPuyo parent;
    int cnt;

    public Puyo(int c, Vector2 p)
    {
        color = c;
        pos = p;

        if (color == 255)
        {
            gO = C.PUYO_GAME_OBJECT;
        }
        else
        {
            gO = Addressables.InstantiateAsync("Assets/Sources/puyo.prefab").WaitForCompletion();
            gO.GetComponent<SpriteRenderer>().color = C.PUYO_COLORS[color];
        }
        t = gO.transform;
        t.position = p;

        cnt = 0;
    }

    public bool update()
    {
        if (cnt >= 1000)
        {
            if (cnt > C.EFFECT_REMOVE_CNT + 1000)
            {
                PuyoManager.I().getList().Remove(this);
                Addressables.ReleaseInstance(gO);
            }
            cnt++;
            return true;
        }
        if (move(10 * C.VEC_DROP) != 10 * C.VEC_DROP)
        {
            if (cnt < C.EFFECT_FIX_CNT) cnt++;

            if (pos.y != 1.5f && Field.I().getPuyo(pos + new Vector2(0, -1)) == null)
            {
                return false;
            }

            Field.I().set(this, pos);

            return (cnt < C.EFFECT_FIX_CNT);
        }
        else
        {
            cnt = 0;
        }
        return true;
    }

    public void setPos(Vector2 p)
    {
        pos = p;
    }

    public Vector2 getPos()
    {
        return pos;
    }

    public void setParent(PuyoPuyo pP)
    {
        parent = pP;
    }
    public PuyoPuyo getParent()
    {
        return parent;
    }
    public int getColor()
    {
        return color;
    }

    public Vector2 move(Vector2 vec)
    {
        Vector2 initPos = pos;

        pos += vec;

        List<Puyo> puyoList = PuyoManager.I().getList();
        for (int i = 0; i < puyoList.Count; i++)
        {
            Puyo p = puyoList[i];

            if (p == this) continue;
            if (p.getParent() != null & p.getParent() == parent) continue;
            if (Vector2.SqrMagnitude(pos - p.getPos()) >= 1) continue;

            if (vec.x != 0)
            {
                if (Mathf.Abs(pos.y - p.getPos().y) < 0.25)
                {
                    pos = initPos;
                    break;
                }
                pos.y = p.getPos().y + new Vector2(0, 1).y * Mathf.Sign(pos.y - p.getPos().y);
                if (!canPut())
                {
                    pos = initPos;
                    break;
                }
            }
            else
            {
                pos.y = p.getPos().y - new Vector2(0, 1).y * Mathf.Sign(vec.y);
                break;
            }
        }

        return pos - initPos;
    }

    public bool canPut()
    {
        List<Puyo> puyoList = PuyoManager.I().getList();
        for (int i = 0; i < puyoList.Count; i++)
        {
            if (puyoList[i] == this) continue;
            if (puyoList[i].getParent() != null & puyoList[i].getParent() == parent) continue;

            if (Vector2.SqrMagnitude(pos - puyoList[i].getPos()) < 1) return false;
        }
        return true;
    }

    public void render()
    {
        if (cnt >= 1000)
        {
            if (cnt - 1000 == (int)((float)C.EFFECT_REMOVE_CNT / 3f))
            {
                t.localScale = new Vector2(0, 0);
            }
            if (cnt - 1000 == (int)((float)C.EFFECT_REMOVE_CNT * 2f / 3f))
            {
                t.localScale = new Vector2(0.9f, 1.1f);
            }
            return;
        }
        float x = (float)cnt / (float)C.EFFECT_FIX_CNT;
        t.position = new Vector2(pos.x, pos.y - quadratic(x, 0.1f));
        t.localScale = new Vector2(1 + quadratic(x, 0.1f), 1);
    }

    public void rm()
    {
        Field.I().set(null, pos);
        cnt = 1000;
        t.localScale = new Vector2(0.9f, 1.1f);
    }


    private static float quadratic(float x, float max)
    {
        return -4f * max * (x - 0.5f) * (x - 0.5f) + max;
    }

}
