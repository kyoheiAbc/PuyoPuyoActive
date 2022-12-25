using UnityEngine;

public class Boss
{
    private Boss()
    {

    }

    public void update()
    {
        if (UnityEngine.Random.Range(0, 90) == 0)
        {
            // Debug.Log("BOSS");
        }
    }


    private static Boss i = new Boss();
    public static Boss I() { return i; }

}
