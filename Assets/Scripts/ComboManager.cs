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
        // for ScoreSystem
        if (tmp == 0) return;

        combo += tmp;
        ScoreSystem.I().incTmp(0, C.COMBO_TO_OJAMA(combo));
        tmp = 0;
        text.text = combo + " combo";
    }
    public int getCombo()
    {
        return combo;
    }
    public void startCnt()
    {
        if (cnt != 0) return;
        if (combo == 0) return;
        cnt++;
    }
    public void update()
    {

        if (cnt == 0) return;
        cnt++;
        if (cnt > C.COMBO_CNT)
        {
            if (Opponent.I().getCombo() == 0)
            {
                Opponent.I().setAtk(combo);
                Opponent.I().incCombo();
            }
            else Opponent.I().setAtk(Opponent.I().getAtk() + combo);

            ScoreSystem.I().setScore(0);
            init();
        }
    }
    private static ComboManager i = new ComboManager();
    public static ComboManager I() { return i; }
}