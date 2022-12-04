using UnityEngine;
using System.IO;

[System.Serializable]
public class GameParameters
{
    public int FPS = 30;
    public int COLOR_NUMBER = 4;
    public int REMOVE_NUMBER = 4;
    public int FIX_CNT = 30;
    public int EFFECT_FIX_CNT = 10;
    public int EFFECT_REMOVE_CNT = 30;
    public int COMBO_CNT = 30;
    public float BOSS_HP = C.COMBO_TO_OJAMA(7) * 2;
    public int BOSS_ATTACK = 6;
    public int BOSS_MASK_NUM = 18;
    public int BOSS_MASK_TIME = 90;
    public float BOSS_MASK_SPEED = 270;
    public float BOSS_SPEED = 180;
    public int GAME_TIME_SEC = 120;
    public int NEXT_GAME_CNT = 90;
    public float VEC_DROP_Y = 0.03f;
    public float VEC_DROP_QUICK_Y = 0.4f;
}

public class Options
{
    public Options()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "gameParameters.json");

        GameParameters gameParams = new GameParameters();
        if (System.IO.File.Exists(filePath))
        {
            string readStr = File.ReadAllText(filePath);
            try
            {
                gameParams = JsonUtility.FromJson<GameParameters>(readStr);
            }
            catch { }
        }
        else
        {
            File.WriteAllText(filePath, JsonUtility.ToJson(gameParams));
        }

        C.FPS = gameParams.FPS;
        C.COLOR_NUMBER = gameParams.COLOR_NUMBER;
        C.REMOVE_NUMBER = gameParams.REMOVE_NUMBER;
        C.FIX_CNT = gameParams.FIX_CNT;
        C.EFFECT_FIX_CNT = gameParams.EFFECT_FIX_CNT;
        C.EFFECT_REMOVE_CNT = gameParams.EFFECT_REMOVE_CNT;
        C.COMBO_CNT = gameParams.COMBO_CNT;
        C.BOSS_HP = gameParams.BOSS_HP;
        C.BOSS_ATTACK = gameParams.BOSS_ATTACK;
        C.BOSS_SPEED = gameParams.BOSS_SPEED;
        C.BOSS_MASK_NUM = gameParams.BOSS_MASK_NUM;
        C.BOSS_MASK_TIME = gameParams.BOSS_MASK_TIME;
        C.BOSS_MASK_SPEED = gameParams.BOSS_MASK_SPEED;
        C.GAME_TIME_SEC = gameParams.GAME_TIME_SEC;
        C.NEXT_GAME_CNT = gameParams.NEXT_GAME_CNT;
        C.VEC_DROP = new Vector2(0, -gameParams.VEC_DROP_Y);
        C.VEC_DROP_QUICK = new Vector2(0, -gameParams.VEC_DROP_QUICK_Y);
    }

}
