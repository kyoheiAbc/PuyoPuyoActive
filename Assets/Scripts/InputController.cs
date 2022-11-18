using UnityEngine;

public class InputController
{
    Camera cam;
    Vector2 pos;
    int ctrl;

    public InputController()
    {
        cam = Camera.main;
    }

    public void init()
    {
        pos = C.VEC_0;
        ctrl = 0;
    }

    public int update()
    {
        if (Input.GetMouseButtonDown(0) && ctrl == 0)
        {
            pos = cam.ScreenToWorldPoint((Vector2)Input.mousePosition);
            ctrl = 1;
        }
        else if (Input.GetMouseButton(0) && ctrl > 0)
        {

            Vector2 p = (Vector2)cam.ScreenToWorldPoint((Vector2)Input.mousePosition);
            Vector2 d = p - pos;

            if (d.x * d.x < 1 && d.y * d.y < 0.25) return 0;

            ctrl = 2;
            pos = p;

            if (Mathf.Abs(d.x) > Mathf.Abs(d.y))
            {
                // pos.x = pos.x + (int)Mathf.Sign(d.x);
                return 5 + (int)Mathf.Sign(d.x);
            }
            else
            {
                // pos.y = pos.y + (int)Mathf.Sign(d.y) * 0.5f;
                return 5 + (int)Mathf.Sign(d.y) * 3;
            }
        }
        else if (Input.GetMouseButtonUp(0) && ctrl > 0)
        {
            if (ctrl == 2)
            {
                init();
                return 0;
            }
            init();
            return 15 + (int)Mathf.Sign(cam.ScreenToWorldPoint((Vector2)Input.mousePosition).x - 4);
        }

        return 0;
    }
}
