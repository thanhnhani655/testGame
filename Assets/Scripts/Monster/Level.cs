using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Level 
{
    [SerializeField]
    private int level = 1;
    [SerializeField]
    private float currentExp = 0;
    [SerializeField]
    private float totalExp = 0;
    [SerializeField]
    private float expNeedToLevelUp = 10;

    public float CurrentExp { get => currentExp;}
    public float ExpNeedToLevelUp { get => expNeedToLevelUp;}
    public int LV { get => level; }
    public float TotalExp { get => totalExp;}

    public bool LevelUp(bool isGodCommand = false)
    {
        if (!isGodCommand)
        {
            if (PlayerCurrency.Instance.Soul < expNeedToLevelUp)
                return false;
        }


        level++;
        CalculateExpNeedToLevelUP();
        InscreaseExp(0);
        return true;
    }

    public void InscreaseExp(float exp)
    {
        currentExp += exp;
        if (currentExp > ExpNeedToLevelUp)
        {
            currentExp -= expNeedToLevelUp;
            expNeedToLevelUp = 0;
        }
        else
        {
            expNeedToLevelUp -= currentExp;
            currentExp = 0;
        }
    }

    public bool IsEnoughExpToLevelUp()
    {
        if (ExpNeedToLevelUp <= 0)
            return true;
        return false;
    }

    public void CalculateExpNeedToLevelUP()
    {
        int t = 10;
        totalExp = 0;
        for (int i = 1; i < level+1; i++)
        {
            totalExp += t;
            t += (i + 1) * 10;
        }
        expNeedToLevelUp = t;

        if (totalExp == 0)
            totalExp = 10;
    }

    public Level CloneLevel()
    {
        return (Level)this.Clone();
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}
