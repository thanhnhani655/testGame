using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parameter : MonoBehaviour
{
    private static Parameter instance;
    public static Parameter Instance => instance;

    public int DailyGachaTimes { get => (dailyGachaTimes + BonusNewgame.Instance.dailySummonInscrease); set => dailyGachaTimes = value; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public float SPEED = 1;
    public float SceneChangeSpeed = 1;
    public float BuffDayChangeSceneSpeed = 0.2f;
    public bool debugCombatController = false;
    private int dailyGachaTimes = 1;
    public int DailyGachaRemain = 1;

    public void CombatDebug(string text)
    {
        if (!debugCombatController)
            return;
        Debug.Log(text);
    }


}
