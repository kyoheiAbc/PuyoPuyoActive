using UnityEngine;
using TMPro;

public class ScoreSystem
{
    int[] score;

    Gauge scoreGauge;


    PopUp tmp_;
    PopUp tmpO_;

    TextMeshPro scoreText;



    private ScoreSystem()
    {
        init();

        scoreGauge = new Gauge(72 * 2, GameObject.Find("Score").gameObject);

        tmp_ = new PopUp(GameObject.Find("Tmp_").GetComponent<TextMeshPro>());
        tmpO_ = new PopUp(GameObject.Find("TmpO_").GetComponent<TextMeshPro>());

        scoreText = GameObject.Find("ScoreT").transform.GetComponent<TextMeshPro>();
        scoreText.gameObject.GetComponent<MeshRenderer>().sortingOrder = 99;


    }

    public void init()
    {
        score = new int[3] { 0, 0, 0 };
    }

    public void incTmp(int index, int tmp)
    {
        if (index == 0)
        {
            tmp_.set(tmp);
        }
        if (index == 1)
        {
            tmpO_.set(-tmp);

        }
        score[index] += tmp;
    }

    public void setScore(int index)
    {
        if (index == 0)
        {
        }
        if (index == 1)
        {
        }
        score[2] = score[2] + (-2 * index + 1) * score[index];

        score[index] = 0;
    }

    public int getScore()
    {
        return score[2];
    }

    public void update()
    {
        tmp_.update();
        tmpO_.update();


        if (score[2] > 0)
        {
            if (Opponent.I().getCombo() == 0)
            {
                Opponent.I().decHp(score[2]);
                Opponent.I().setDmgEffect();
                score[2] = 0;
            }
        }
        else if (score[2] < 0)
        {
            if (ComboManager.I().getCombo() == 0)
            {
                if (PuyoManager.I().getPuyoPuyo() == null)
                {
                    if (PuyoManager.I().updateDone())
                    {
                        if (ComboManager.I().getTmp() == 0)
                        {
                            score[2] += PuyoManager.I().newOjama(-score[2]);
                            ComboManager.I().setDmgEffect();
                            if (score[2] <= 0)
                            {
                                PuyoManager.I().next = true;
                            }
                        }
                        else
                        {
                            PuyoManager.I().next = true;
                        }
                    }
                }
            }
        }


        scoreText.text = (score[2] + score[0] - score[1]).ToString();

        scoreGauge.setPoint(score[2] + score[0] - score[1] + 72);
    }

    private static ScoreSystem i = new ScoreSystem();
    public static ScoreSystem I() { return i; }
}

public class PopUp
{
    TextMeshPro t;
    int cnt;
    public PopUp(TextMeshPro t_)
    {
        t_.gameObject.GetComponent<MeshRenderer>().sortingOrder = 99;

        t = t_;
        init();
    }
    public void init()
    {
        cnt = 0;
        t.text = "";
    }
    public void set(int s)
    {
        t.text = s >= 0 ? "+" : "-";
        t.text += Mathf.Abs(s);
        cnt = 1;
    }
    public void update()
    {
        if (cnt == 0) return;
        cnt++;
        if (cnt > 60) init();
    }
}

public class Gauge
{
    int max;
    int point;
    Transform t;
    public Gauge(int max_, GameObject gO)
    {
        max = max_;
        t = gO.transform.GetChild(0).transform;
        init();
    }
    public void init()
    {
        point = max;
        setUi();
    }
    public void setPoint(int p)
    {
        point = p;
        if (point < 0) point = 0;
        if (max < point) point = max;
        setUi();
    }
    public void incPoint(int p)
    {
        point += p;
        if (point < 0) point = 0;
        if (max < point) point = max;
        setUi();
    }
    public void setUi()
    {
        t.localScale = new Vector2((float)point / (float)max, 1);
        t.localPosition = new Vector2(-((float)max - (float)point) / (2 * (float)max), 0);
    }
}