using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectExplosion
{
    GameObject gameObject;
    Transform transform;
    Transform[] children;
    int cnt;

    public EffectExplosion(GameObject origin, GameObject gO)
    {
        gameObject = gO;
        transform = gameObject.transform;
        transform.position = (Vector2)origin.transform.position - new Vector2(0.5f, 0.5f)
                            + new Vector2(0.5f, 0) * UnityEngine.Random.Range(0, 3)
                            + new Vector2(0, 0.5f) * UnityEngine.Random.Range(0, 3);


        transform.Rotate(0, 0, UnityEngine.Random.Range(0, 7) * 15);

        Color color = origin.GetComponent<SpriteRenderer>().color;

        cnt = 0;

        children = new Transform[4];
        for (int i = 0; i < 4; i++)
        {
            children[i] = this.transform.GetChild(i).gameObject.transform;
            children[i].GetComponent<SpriteRenderer>().color = color;

            children[i].localPosition = (Vector2)children[i].localPosition - new Vector2(0.5f, 0.5f)
                                        + new Vector2(0.5f, 0) * UnityEngine.Random.Range(0, 3)
                                        + new Vector2(0, 0.5f) * UnityEngine.Random.Range(0, 3);


        }
    }

    public bool update()
    {
        cnt++;

        for (int i = 0; i < 4; i++)
        {
            children[i].localPosition = (Vector2)children[i].localPosition
                                        + new Vector2(0.2f, 0) * (1 - i) * ((i + 1) % 2)
                                        + new Vector2(0, 0.2f) * (2 - i) * (i % 2);
        }

        if (cnt == 15)
        {
            return false;
        }
        return true;
    }
    public GameObject getGameObject()
    {
        return gameObject;
    }
}
