using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("New Day Phase")]
    public DayDisplay newDayScene;

    public void RunNextDayPhase()
    {
        newDayScene.NextDay();
    }
}
