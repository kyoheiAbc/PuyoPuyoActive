using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Puyo
{
    Vector2 pos;
    GameObject gO;
    Field field;

    public Puyo(Field f, GameObject g)
    {
        field = f;
        gO = g;
        if (gO != null) pos = gO.transform.position;
    }

    ~Puyo()
    {
        // Debug.Log("---");
        // Debug.Log(pos);
        // Debug.Log("~Puyo()");
        gO = null;
        field = null;
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
            if (field.getPuyo(pos + new Vector2(0, C.PUYO_R) + vec) == null &&
                field.getPuyo(pos + new Vector2(0, -C.PUYO_R) + vec) == null)
            {
                pos += vec;
            }
            else if (field.getPuyo(pos + new Vector2(0, C.PUYO_R / 2) + vec) == null)
            {
                pos += new Vector2(0, C.PUYO_R / 2) + vec;
                pos = new Vector2(pos.x, (int)pos.y + 0.5f);
            }
            else if (field.getPuyo(pos - new Vector2(0, C.PUYO_R / 2) + vec) == null)
            {
                pos += -new Vector2(0, C.PUYO_R / 2) + vec;
                pos = new Vector2(pos.x, (int)pos.y + 0.5f);
            }
        }
        else
        {
            pos += vec;
            if (field.getPuyo(pos + new Vector2(0, C.PUYO_R) * Mathf.Sign(vec.y)) != null)
            {
                pos = new Vector2(pos.x, (int)pos.y + 0.5f);
                if (field.getPuyo(pos) != null)
                {
                    pos -= new Vector2(0, 1) * Mathf.Sign(vec.y);
                }
            }
        }
        return pos - initPos;
    }

    public Vector2 getPos()
    {
        return pos;
    }

    public void setToField()
    {
        field.setPuyo(this);
    }

    public bool error()
    {
        return field.getPuyo(pos + new Vector2(0, C.PUYO_R)) != null
            || field.getPuyo(pos + new Vector2(0, -C.PUYO_R)) != null;
    }

    public void render()
    {
        gO.transform.position = pos;
    }

    public GameObject getGameObject()
    {
        return gO;
    }


}
