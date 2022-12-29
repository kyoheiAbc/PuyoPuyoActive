using UnityEngine;
using TMPro;

public class ComboManager
{
    int combo;
    int tmp;
    int cnt;
    TextMeshPro text;

    private ComboManager()
    {
        text = GameObject.Find("Combo").GetComponent<TextMeshPro>();
        init();
    }
    public void init()
    {
        combo = 0;
        tmp = 0;
        cnt = 0;
        text.text = "";
    }
    public void incTmp()
    {
        tmp++;
        cnt = 0;
    }
    public void setCombo()
    {
        if (tmp == 0) return;
        OjamaSystem.I().incAtkTmp(C.COMBO_TO_OJAMA(combo));

        combo += tmp;
        tmp = 0;
        text.text = combo + " combo";
    }
    public void startCnt()
    {
        if (cnt != 0) return;
        if (combo == 0) return;
        cnt++;
    }
    public void update()
    {
        if (combo == 0)
        {
            OjamaSystem.I().fixDmg();
        }

        if (cnt == 0) return;
        cnt++;
        if (cnt > C.COMBO_CNT)
        {
            OjamaSystem.I().setAtk();
            init();
        }
    }
    private static ComboManager i = new ComboManager();
    public static ComboManager I() { return i; }
}