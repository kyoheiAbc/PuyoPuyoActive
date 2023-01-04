using UnityEngine;
using TMPro;

public class Opponent
{
    int hp;
    int energy;
    int combo;
    int cnt;
    int atk;
    readonly int energyMax;
    Gauge hpGauge;
    Gauge energyGauge;
    PopUp hp_;
    Transform icon;
    int iconCnt;
    GameObject dmg;
    int dmgCnt;



    private Opponent()
    {
        energyMax = C.OPPONENT_ATTACK[C.OPPONENT_ATTACK.Length - 1] * 1000;

        hpGauge = new Gauge(C.OPPONENT_HP, GameObject.Find("Opponent").transform.Find("HpGauge").gameObject);
        energyGauge = new Gauge(energyMax, GameObject.Find("Opponent").transform.Find("EnergyGauge").gameObject);

        hp_ = new PopUp(GameObject.Find("Hp_").GetComponent<TextMeshPro>());

        icon = GameObject.Find("Opponent").transform.Find("Icon").transform;

        dmg = GameObject.Find("Opponent").transform.Find("Damage").gameObject;


        init();
    }
    public void init()
    {
        hp = C.OPPONENT_HP;
        energy = 0;
        combo = 0;
        cnt = 0;
        hpGauge.init();
        energyGauge.init();
        hp_.init();

        iconCnt = 0;
        dmgCnt = 0;
        dmg.SetActive(false);

        resetAtk();

    }

    public void resetAtk()
    {
        int[] atkAry = new int[C.OPPONENT_ATTACK.Length - 1];
        for (int i = 0; i < atkAry.Length; i++)
        {
            atkAry[i] = C.OPPONENT_ATTACK[i];
        }
        C.SHUFFLE(ref atkAry);
        atk = atkAry[0];
    }

    public void decHp(int h)
    {
        hp -= h;

        hp_.set(-h);


        if (hp < 0) hp = 0;

        hpGauge.setPoint(hp);


    }
    public int getCombo()
    {
        return combo;
    }
    public int getAtk()
    {
        return atk;
    }
    public void setAtk(int a)
    {
        atk = a;
    }
    public void incCombo()
    {

        if (energy < 1000)
        {

            combo = 0;
            return;
        }

        combo++;
        energy -= 1000;

        ScoreSystem.I().incTmp(1, C.COMBO_TO_OJAMA(combo));

        if (iconCnt == 0) iconCnt = 1;


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
            if (dmgCnt == 30)
            {
                dmgCnt = 0;
                dmg.SetActive(false);
            }
        }

        if (iconCnt != 0)
        {
            iconCnt++;
            icon.localPosition = new Vector2(-C.QUADRATIC((float)iconCnt / C.EFFECT_ICON_CNT, C.EFFECT_ICON), 0);
            if (iconCnt == C.EFFECT_ICON_CNT) iconCnt = 0;
        }



        hp_.update();


        energyGauge.setPoint(energy);


        if (combo == 0 && energy / 1000 >= atk)
        {
            float tmp = ((float)energyMax - (float)energy) / (float)C.OPPONENT_SPEED;
            float p = (int)((tmp / 100) * (101 - C.OPPONENT_ACT_RATE));
            if (UnityEngine.Random.Range(0, (int)p) == 0)
            {
                incCombo();

            }
        }
        if (energy >= energyMax)
        {
            atk = energyMax / 1000;
            incCombo();
        }

        if (combo == 0)
        {
            energy += C.OPPONENT_SPEED;
            if (energy >= energyMax)
            {
                energy = energyMax;
            }
            cnt = 0;
        }
        else
        {
            cnt++;
            if (cnt > C.EFFECT_REMOVE_CNT + (6 / -(10 * C.VEC_DROP.y)) + C.EFFECT_FIX_CNT)
            {
                cnt = 0;
                if (combo >= atk || energy < 1000)
                {
                    ScoreSystem.I().setScore(1);
                    resetAtk();
                    combo = 0;
                    if (ScoreSystem.I().getOjmCtl() == 0)
                    {
                        ScoreSystem.I().setOjmCtl(1);
                    }
                }
                else incCombo();
            }
        }




    }


    private static Opponent i = new Opponent();
    public static Opponent I() { return i; }

}
