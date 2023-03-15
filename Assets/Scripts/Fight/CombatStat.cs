using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CombatStatType 
{
    MaxHP,
    HP,
    ATK,
    CRITRATE,
    CRITDMG,
    DMGOUTPUT,
    HEALINGBONUS,
    DMGGAIN
}

[System.Serializable]
public class CombatStat : ICloneable
{
    //Basic Stat
    public float maxHP;
    public float baseMaxHP;
    public float currentHP;
    public float atk;
    public float criticalRate;
    public float criticalDmg;
    public float attackSpeed;
    

    //Advance Stat
    public float dmgOutput;
    public float healingBonus;
    public float dmgGain;
    private float shield;

    //Function Stat
    public float timeCounter = 0;

    public List<Buff> listBuff;

    public void Initialize(Stat stat)
    {
        maxHP = stat.maxHP;
        baseMaxHP = maxHP;
        currentHP = maxHP;
        atk = stat.atk;
        criticalRate = stat.criticalRate;
        criticalDmg = stat.criticalDmg;
        attackSpeed = stat.attackSpeed;

        dmgOutput = 0;
        healingBonus = 0;
        dmgGain = 0;

        listBuff = new List<Buff>();
    }

    public void InscreaseShield(float delta)
    {
        shield += delta;
        if (shield > maxHP)
            shield = maxHP;
    }

    public float DmgShield(float idmg)
    {
        if (shield == 0)
            return idmg;
        float dmg = idmg;
        if (shield > dmg)
        {
            shield -= dmg;
            return 0;
        }
        dmg -= shield;
        shield = 0;
        return dmg;
    }

    public CombatStat Capture()
    {
        CombatStat newStat = this.CloneCombatStat();

        foreach (Buff buff in newStat.listBuff)
        {
            switch (buff.statType)
            {
                case CombatStatType.MaxHP:
                    break;
                case CombatStatType.HP:
                    break;
                case CombatStatType.ATK:
                    newStat.atk += buff.buffValue;
                    break;
                case CombatStatType.CRITRATE:
                    newStat.criticalRate += buff.buffValue;
                    break;
                case CombatStatType.CRITDMG:
                    newStat.criticalDmg += buff.buffValue;
                    break;
                case CombatStatType.DMGOUTPUT:
                    newStat.dmgOutput += buff.buffValue;
                    break;
                case CombatStatType.HEALINGBONUS:
                    newStat.healingBonus += buff.buffValue;
                    break;
                case CombatStatType.DMGGAIN:
                    newStat.dmgGain += buff.buffValue;
                    break;
            }
        }

        return newStat;
    }

    public CombatStat CloneCombatStat()
    {
        return (CombatStat)this.Clone();
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public void AddBuff(Buff buff)
    {
        Buff temp = listBuff.Find(x => x.id == buff.id);
        if (temp == null)
        {
            listBuff.Add(buff);
            return;
        }

        temp.buffStack += buff.buffStack;
    }

    public void AddBuff(string id, int istack, float ivalue)
    {
        Buff buff = DatabaseInstanceAccess.Instance.buffDatabase.CreateBuff(id, istack, ivalue);
        Buff temp = listBuff.Find(x => x.id == buff.id);
        if (temp == null)
        {
            listBuff.Add(buff);
            return;
        }

        temp.buffStack += buff.buffStack;
    }
}
