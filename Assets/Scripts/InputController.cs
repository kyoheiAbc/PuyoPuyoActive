using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController
{
    Vector2 firstPosition;

    public int update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            firstPosition = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
            return 5;
        }

        else if (Input.GetMouseButton(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);

            if ((firstPosition.x - pos.x) * (firstPosition.x - pos.x)
             >= (firstPosition.y - pos.y) * (firstPosition.y - pos.y))
            {
                if (firstPosition.x - pos.x > 1)
                {
                    firstPosition = pos;
                    return 4;
                }
                else if (firstPosition.x - pos.x < -1)
                {
                    firstPosition = pos;
                    return 6;
                }
            }
            else
            {
                if (firstPosition.y - pos.y > 1)
                {
                    firstPosition = pos;
                    return 2;
                }
                else if (firstPosition.y - pos.y < -1)
                {
                    firstPosition = pos;
                    return 8;
                }
            }
            return 5;
        }
        return 0;
    }
}