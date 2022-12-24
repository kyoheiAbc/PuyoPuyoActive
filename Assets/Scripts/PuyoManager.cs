using System.Collections.Generic;
using UnityEngine;

public class PuyoManager
{
    PuyoPuyo[] puyoPuyos;
    List<Puyo> puyoList;
    Field field;
    ColorBag colorBag;


    public PuyoManager()
    {
        puyoPuyos = new PuyoPuyo[3];
        puyoList = new List<Puyo>();
        field = new Field();
        colorBag = new ColorBag();
    }

    public void init()
    {
        puyoPuyos = new PuyoPuyo[3];

        puyoList.Clear();
        for (int y = 0; y < 15; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                if (y == 0 || x == 0 || x == 8 - 1)
                {
                    puyoList.Add(new Puyo(255, new Vector2(x + 0.5f, y + 0.5f), puyoList, field));
                }
            }
        }

        field.init(null);

        colorBag.init();

        for (int n = 0; n < 3; n++) nextPuyoPuyo();

    }

    public PuyoPuyo getPuyoPuyo()
    {
        return puyoPuyos[0];
    }

    public void nextPuyoPuyo()
    {
        puyoPuyos[0] = puyoPuyos[1];
        puyoPuyos[1] = puyoPuyos[2];
        puyoPuyos[2] = newPuyoPuyo();

        if (puyoPuyos[1] == null) return;
        puyoPuyos[1].setPos(new Vector2(8.5f, 11.5f));

        if (puyoPuyos[0] == null) return;
        puyoPuyos[0].setPos(new Vector2(3.5f, 12.5f));
    }

    private PuyoPuyo newPuyoPuyo()
    {
        puyoList.Add(new Puyo(colorBag.getColor(), new Vector2(0, 0), puyoList, field));
        puyoList.Add(new Puyo(colorBag.getColor(), new Vector2(0, 0), puyoList, field));

        PuyoPuyo puyoPuyo = new PuyoPuyo(puyoList[puyoList.Count - 2], puyoList[puyoList.Count - 1]);
        puyoPuyo.setPos(new Vector2(8.5f, 8.5f));

        return puyoPuyo;
    }

    public void update()
    {
        if (!puyoPuyos[0].update())
        {
            puyoPuyos[0] = null;
            puyoList.Sort(sortPosY);
        }

        bool update = false;
        for (int i = 0; i < puyoList.Count; i++)
        {
            if (puyoList[i].getParent() != null) continue;
            if (puyoList[i].getPos().x == 0.5f) continue;
            if (puyoList[i].getPos().x == 7.5f) continue;
            if (puyoList[i].getPos().y == 0.5f) continue;

            Puyo tmp = puyoList[i];
            if (puyoList[i].update())
            {
                update = true;
            }

            if (tmp != puyoList[i])
            {
                i--;
            }

        }
        if (!update)
        {
            Debug.Log("TRY RM");
            if (field.tryRm())
            {
                field.init(null);
            }
        }

    }
    private static int sortPosY(Puyo pA, Puyo pB)
    {
        return (int)(pA.getPos().y - pB.getPos().y);
    }

    public void render()
    {
        for (int i = 0; i < puyoList.Count; i++) puyoList[i].render();
    }


}

public class ColorBag
{
    int[] bag;
    int cnt;
    public ColorBag()
    {
        bag = new int[C.COLOR_NUMBER * 64];
        for (int i = 0; i < bag.Length; i++)
        {
            bag[i] = i % C.COLOR_NUMBER;
        }
    }
    public void init()
    {
        for (int i = bag.Length - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            int tmp = bag[i];
            bag[i] = bag[j];
            bag[j] = tmp;
        }
        cnt = 0;
    }
    public int getColor()
    {
        int ret;
        if (cnt < bag.Length)
        {
            ret = bag[cnt];
        }
        else
        {
            init();
            ret = bag[cnt];
        }
        cnt++;
        return ret;
    }
}