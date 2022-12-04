using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OjamaManager
{
    GameObject[,] ary;

    public OjamaManager(GameObject[,] g)
    {
        ary = g;
    }

    public void init(bool a)
    {
        for (int y = 0; y < 12; y++)
        {
            for (int x = 0; x < 6; x++)
            {
                ary[y, x].SetActive(a);
            }
        }
    }

    public void update()
    {

    }


    public void setOjmField(int num)
    {
        init(false);
        for (int n = 0; n < num; n++)
        {
            GameObject gO = ary[UnityEngine.Random.Range(0, 12), UnityEngine.Random.Range(0, 6)];
            gO.SetActive(true);
        }
    }


}
