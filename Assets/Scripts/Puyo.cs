using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puyo
{
    int color;
    GameObject gO;

    public Puyo(int c, GameObject gameObject)
    {
        color = c;
        gO = gameObject;
    }

    ~Puyo()
    {
        Debug.Log("~Puyo()");
    }

    public Vector2 move(Vector2 vec)
    {
        Vector2 p = gO.transform.position;
        gO.transform.position = (Vector2)gO.transform.position + vec;
        return (Vector2)gO.transform.position - p;
    }

    public Vector2 getPos()
    {
        return gO.transform.position;
    }

}
