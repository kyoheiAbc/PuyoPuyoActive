using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Puyo
{
    int color;
    Vector2 pos;
    GameObject gO;
    Field field;
    PuyoManager pM;

    public Puyo()
    {
    }

    ~Puyo()
    {
        // Debug.Log("~Puyo()");
    }

    public void init(GameObject gO_, Field f, PuyoManager pM_)
    {
        gO = gO_;
        pos = gO.transform.position;
        field = f;
        pM = pM_;
    }


    public void setPos(Vector2 p)
    {
        pos = p;
    }

    public Vector2 move(Vector2 vec)
    {
        Vector2 initPos = pos;

        // if collision puyoList return 0
        //
        //
        //

        if (vec.x != 0)
        {
            if (field.getPuyo(pos + new Vector2(0, 0.49f) + vec) == null ||
                field.getPuyo(pos + new Vector2(0, -0.49f) + vec) == null)
            {
                pos += vec;
            }
        }
        else
        {
            pos += vec;
            int sign = (int)Mathf.Sign(vec.y);
            if (field.getPuyo(pos + new Vector2(0, 0.49f) * sign) != null)
            {
                pos = new Vector2(pos.x, (int)pos.y + 0.49f);
                if (field.getPuyo(pos + new Vector2(0, 0.49f) * sign) != null)
                {
                    pos -= vec;
                }
            }
        }
        return pos - initPos;
    }

    public Vector2 getPos()
    {
        return pos;
    }
    public void render()
    {
        gO.transform.position = pos;
    }


}
