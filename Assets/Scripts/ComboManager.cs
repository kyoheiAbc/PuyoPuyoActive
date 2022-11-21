using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager
{

    int combo;
    int cnt;

    public ComboManager()
    {
        init();
    }

    public void init()
    {
        combo = 0;
        cnt = C.COMBO_CNT;
    }

    public void update()
    {
        if (0 < cnt && cnt < C.COMBO_CNT) cnt++;
        if (cnt == C.COMBO_CNT) combo = 0;
    }

    public void comboPlus()
    {
        combo++;
    }

    public void setCnt(int c)
    {
        cnt = c;
    }
    public int getCnt()
    {
        return cnt;
    }

}
