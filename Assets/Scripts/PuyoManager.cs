using System.Collections.Generic;
using UnityEngine;

public class PuyoManager
{
    PuyoPuyo[] puyoPuyos;
    List<Puyo> puyoList;
    ColorBag colorBag;


    private PuyoManager()
    {
        puyoPuyos = new PuyoPuyo[3];
        puyoList = new List<Puyo>();
        colorBag = new ColorBag();
        init();
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
                    puyoList.Add(new Puyo(255, new Vector2(x + 0.5f, y + 0.5f)));
                }
            }
        }

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
        puyoList.Add(new Puyo(colorBag.getColor(), new Vector2(0, 0)));
        puyoList.Add(new Puyo(colorBag.getColor(), new Vector2(0, 0)));

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
            if (Field.I().tryRm())
            {
                Field.I().init();
            }
        }

    }

    public List<Puyo> getList()
    {
        return puyoList;
    }
    private static int sortPosY(Puyo pA, Puyo pB)
    {
        return (int)(pA.getPos().y - pB.getPos().y);
    }

    public void render()
    {
        for (int i = 0; i < puyoList.Count; i++) puyoList[i].render();
    }
    private static PuyoManager i = new PuyoManager();
    public static PuyoManager I() { return i; }

    private class ColorBag
    {
        int[] bag;
        int cnt;
        public ColorBag()
        {
            bag = new int[C.COLOR_NUMBER * 64];
            init();
        }
        public void init()
        {
            int[] colors = new int[5] { 0, 1, 2, 3, 4 };
            shuffle(ref colors);
            for (int i = 0; i < bag.Length; i++)
            {
                bag[i] = colors[i % C.COLOR_NUMBER];
            }
            shuffle(ref bag);
            cnt = 0;
        }
        public int getColor()
        {
            if (cnt >= bag.Length)
            {
                shuffle(ref bag);
                cnt = 0;
            }
            int ret = bag[cnt];
            cnt++;
            return ret;
        }
        private void shuffle(ref int[] ary)
        {
            for (int i = ary.Length - 1; i > 0; i--)
            {
                int r = UnityEngine.Random.Range(0, i + 1);
                int tmp = ary[i];
                ary[i] = ary[r];
                ary[r] = tmp;
            }
        }
    }
}




