using UnityEngine;

public class InputController
{
    readonly Camera cam;
    byte ctl;
    Vector2 pos;

    public InputController()
    {
        cam = Camera.main;
    }

    public void init()
    {
        ctl = 0;
        pos = new Vector2(0, 0);
    }

    public int update()
    {
        if (Input.GetMouseButtonDown(0) && ctl == 0)
        {
            ctl = 1;
            pos = cam.ScreenToWorldPoint((Vector2)Input.mousePosition);
        }
        else if (Input.GetMouseButton(0) && ctl > 0)
        {
            Vector2 p = (Vector2)cam.ScreenToWorldPoint((Vector2)Input.mousePosition);
            Vector2 d = p - pos;

            if (d.x * d.x < 1 && d.y * d.y < 1) return 0;

            ctl = 2;
            pos = p;

            if (d.x > 1) return 6;
            if (d.x < -1) return 4;
            if (d.y > 1) return 8;
            if (d.y < -1) return 2;
        }
        else if (Input.GetMouseButtonUp(0) && ctl > 0)
        {
            if (ctl == 2)
            {
                init();
                return 0;
            }
            init();
            return 16;
        }
        return 0;
    }
}
