using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class OjamaManager
{
    // GameObject[,] ary;
    int ojama;
    TextMeshPro text;

    public OjamaManager(GameObject[,] g)
    {
        // ary = g;
        text = GameObject.Find("OjamaUI").GetComponent<TextMeshPro>();
        init();
    }

    public void init()
    {
        // for (int y = 0; y < 12; y++)
        // {
        //     for (int x = 0; x < 6; x++)
        //     {
        //         ary[y, x].SetActive(a);
        //     }
        // }
        text.text = "";
        ojama = 0;
    }
    public void setOjama(int o)
    {
        ojama = o;
        text.text = ojama.ToString();
    }

    public int getOjama()
    {
        return ojama;
    }


    // public void setOjmField(int num)
    // {
    //     init(false);
    //     for (int n = 0; n < num; n++)
    //     {
    //         GameObject gO = ary[UnityEngine.Random.Range(0, 12), UnityEngine.Random.Range(0, 6)];
    //         gO.SetActive(true);
    //     }
    // }


}
