using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController
{
    Vector2 stdPos = new Vector2(0, 0);
    int cnt = 0;

    public int update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            cnt++;
            stdPos = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);

            cnt++;
            if (cnt > C.TOUCH_CNT) cnt = 255;


            if ((stdPos.x - pos.x) * (stdPos.x - pos.x) >= (stdPos.y - pos.y) * (stdPos.y - pos.y))
            {
                if (stdPos.x - pos.x > 1)
                {
                    stdPos = pos;
                    cnt = 255;
                    return 4;
                }
                else if (stdPos.x - pos.x < -1)
                {
                    stdPos = pos;
                    cnt = 255;
                    return 6;
                }
            }
            else
            {
                if (stdPos.y - pos.y > 0.5f)
                {
                    stdPos = pos;
                    cnt = 255;
                    return 2;
                }
                else if (stdPos.y - pos.y < -0.5f)
                {
                    stdPos = pos;
                    cnt = 255;
                    return 8;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (cnt <= C.TOUCH_CNT)
            {
                stdPos = new Vector2(0, 0);
                cnt = 0;
                if (Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition).x >= 4) return 16;
                else return 14;
            }
        }
        else
        {
            stdPos = new Vector2(0, 0);
            cnt = 0;
        }
        return 0;
    }
}
