using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCounter : MonoBehaviour
{
    private static DayCounter instance;
    public static DayCounter Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public event System.Action OnEndScene = delegate { };
    public int Day = 0;
    public DayDisplay dayDisplay;
    public WaveGenerator invaderGenerator;
    public CorruptionController corruptionController;

    public void NextDay()
    {
        if (Day > 0)
        {
            invaderGenerator.RecordPowerLevel();
            invaderGenerator.LevelUP();
            corruptionController.UpdateCorruptor();
        }

        Day++;
        dayDisplay.NextDay();
        Parameter.Instance.DailyGachaRemain = Parameter.Instance.DailyGachaTimes;
    }

    public void EndScene()
    {
        OnEndScene();
    }
}
