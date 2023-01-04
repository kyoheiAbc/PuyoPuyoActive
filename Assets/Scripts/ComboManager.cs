using UnityEngine;
using TMPro;

public class ComboManager
{
    int combo;
    int tmp;
    int cnt;
    TextMeshPro text;
    Transform t;

    Transform icon;
    int iconCnt;
    GameObject dmg;
    int dmgCnt;

    private ComboManager()
    {
        t = GameObject.Find("Combo").transform;
        t.gameObject.GetComponent<MeshRenderer>().sortingOrder = 99;
        text = GameObject.Find("Combo").transform.GetComponent<TextMeshPro>();

        icon = GameObject.Find("Player").transform.Find("Icon").transform;

        dmg = GameObject.Find("Player").transform.Find("Damage").gameObject;

        init();
    }
    public void init()
    {
        combo = 0;
        tmp = 0;
        cnt = 0;
        text.text = "";
        iconCnt = 0;
        dmgCnt = 0;
        dmg.SetActive(false);


    }
    public void incTmp()
    {
        tmp++;
        cnt = 0;
    }
    public void setPos(Vector2 p)
    {
        t.position = p;
    }
    public void setCombo()
    {
        // for ScoreSystem
        if (tmp == 0) return;

        combo += tmp;
        ScoreSystem.I().incTmp(0, C.COMBO_TO_OJAMA(combo));
        if (iconCnt == 0) iconCnt = 1;

        tmp = 0;
        text.text = combo.ToString();
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
    public void setDmgEffect()
    {
        dmgCnt = 1;
    }
    public void update()
    {


        if (dmgCnt != 0)
        {
            if (dmgCnt == 1) dmg.SetActive(true);
            dmgCnt++;
            if (dmgCnt > 30)
            {
                dmgCnt = 0;
                dmg.SetActive(false);
            }
        }

        if (iconCnt != 0)
        {
            iconCnt++;
            icon.localPosition = new Vector2(C.QUADRATIC((float)iconCnt / C.EFFECT_ICON_CNT, C.EFFECT_ICON), 0);
            if (iconCnt == C.EFFECT_ICON_CNT) iconCnt = 0;
        }


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

            ScoreSystem.I().setOjmCtl(1);

            ScoreSystem.I().setScore(0);
            init();
        }
    }
    private static ComboManager i = new ComboManager();
    public static ComboManager I() { return i; }
}