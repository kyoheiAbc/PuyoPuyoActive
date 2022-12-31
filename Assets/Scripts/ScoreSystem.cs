using UnityEngine;
using TMPro;

public class ScoreSystem
{
    int[] score;
    TextMeshPro debug;

    private ScoreSystem()
    {
        init();
        debug = GameObject.Find("Score").GetComponent<TextMeshPro>();
    }

    public void init()
    {
        score = new int[3] { 0, 0, 0 };
    }

    public void incTmp(int index, int tmp)
    {
        Debug.Log("incTmp, index: " + index + ", tmp: " + tmp);

        score[index] += tmp;
    }

    public void setScore(int index)
    {

        score[2] = score[2] + (-2 * index + 1) * score[index];
        Debug.Log("setScore, index: " + index + ", score[index]: " + score[index] + ", score[2]: " + score[2]);

        score[index] = 0;
    }

    public void update()
    {
        if (score[2] > 0)
        {
            if (Opponent.I().getCombo() == 0)
            {
                Opponent.I().decHp(score[2]);
                score[2] = 0;
            }
        }
        else
        {
            if (ComboManager.I().getCombo() == 0)
            {
                PuyoManager.I().newOjama(-score[2]);
                score[2] = 0;
            }
        }

        debug.text = (score[2] + score[0] - score[1]).ToString();
    }

    private static ScoreSystem i = new ScoreSystem();
    public static ScoreSystem I() { return i; }
}
