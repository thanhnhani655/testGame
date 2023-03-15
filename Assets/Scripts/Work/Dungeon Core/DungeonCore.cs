using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class DungeonCore : MonoBehaviour
{
    private static DungeonCore instance;
    public static DungeonCore Instance => instance;
    
    public float SoulGain 
    {
        get
        {
            float totalSoulGain = soulGain;
            foreach (Facility soulExtractor in BuildingController.Instance.GetListSoulGainAble())
            {
                totalSoulGain += soulExtractor.value;
            }
            totalSoulGain += T_SoulGain;
            if (T_BetterPrison)
                totalSoulGain += PrisonerInventory.Instance.GetListData().Count;
            return totalSoulGain;
        }
        set => soulGain = value; 
    }

    public float Atk 
    { 
        get
        {
            float tempatk = atk;
            tempatk += T_Atk;
            return tempatk;
        }
        set => atk = value; 
    }

    public float Hp 
    {
        get
        {
            float tempHP = hp;
            tempHP += T_HP;
            return tempHP;
        }
        set => hp = value; 
    }

    public float CriticalRate {
        get
        {
            float tempCrit = criticalRate;
            tempCrit += T_CriticalRate;
            return tempCrit;
        }
        set => criticalRate = value; 
    }

    public float HealingBonus {
        get
        {
            float tempHealBonus = healingBonus;
            tempHealBonus += T_HealingBonus;
            return tempHealBonus;
        }
        set => healingBonus = value; 
    }
    
    public float DmgBonus {
        get
        {
            float temp = dmgBonus;
            temp += T_DmgBonus;
            return temp;
        }
        set => dmgBonus = value; 
    }

    public float Atkspd {
        get
        {
            float temp = atkspd;
            temp += T_ATKSPD;
            return temp;
        }
        set => atkspd = value; 
    }

    public float CorruptedSoulGain {
        get
        {
            float temp = corruptedSoulGain;
            temp += T_corruptedSoulGain;
            return temp;
        }
        set => corruptedSoulGain = value; 
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    [SerializeField]
    private DungeonCoreUI dungeonCoreUI;
    [SerializeField]
    public int coreLevel = 1;
    [Header("Stat")]
    public float monsterlevel;
    [SerializeField]
    private float hp;
    [SerializeField]
    private float atk;
    [SerializeField]
    private float criticalRate;
    [SerializeField]
    public float criticalDmg;
    [SerializeField]
    private float soulGain;
    [SerializeField]
    private float healingBonus;
    [SerializeField]
    private float dmgBonus;
    [SerializeField]
    private float atkspd;
    [SerializeField]
    private float corruptedSoulGain;

    public int pointRemain;

    [Header("Stat Inscrease Per Level")]
    public float levelIns;
    public float hpIns;
    public float atkIns;
    public float criticalRateIns;
    public float criticalDmgIns;
    public float soulGainIns;

    [Header("Stat From Treasure")]
    [SerializeField]
    private float T_SoulGain;
    [SerializeField]
    public float T_corruptedSoulGain;
    [SerializeField]
    private float T_Atk;
    [SerializeField]
    private float T_HP;
    [SerializeField]
    public float T_DiamondBuff;
    [SerializeField]
    private float T_CriticalRate;
    [SerializeField]
    private float T_HealingBonus;
    [SerializeField]
    private float T_DmgBonus;
    [SerializeField]
    private float T_ATKSPD;
    public float T_BossAtk;
    public float T_BossHP;
    public float T_BossMinionAtk;
    public float T_BossMinionHP;
    public float T_Shield;
    public float T_PrisonCapaciy;
    [SerializeField]
    private bool T_BetterPrison = false;
    public bool T_LegionUpgrade1 = false;
    public bool T_LegionUpgrade2 = false;

    private float totalExp = 0f;
    private float soulNeedToLevelUp = 100f;

    //private bool isSoulConcentratedIsLock = true;
    [Header("Legion Bonus")]
    public List<LegionCore> listLegionCore;

    public MonsterData BossFloor1;

    public void UpdateAfterLevelUp()
    {
        dungeonCoreUI.UpdateDungeonCoreUI();
    }

    public void LevelUp(bool direct = false)
    {
        if (direct)
        {
            coreLevel++;
            pointRemain++;
            //level++;
            CalculateExpNeedToLevelUP();
            LevelMonsterUp();

            UpdateAfterLevelUp();
            return;
        }

        if (PlayerCurrency.Instance.Soul < soulNeedToLevelUp)
            return;

        PlayerCurrency.Instance.Soul -= soulNeedToLevelUp;
        coreLevel++;
        pointRemain++;
        //level++;
        CalculateExpNeedToLevelUP();
        LevelMonsterUp();

        UpdateAfterLevelUp();
    }

    public void ResetLevel()
    {
        coreLevel = 1;
        soulNeedToLevelUp = 100;
        hp = 0;
        atk = 0;
        criticalRate = 0;
        criticalDmg = 0;
        LoadTreasureStat();
    }

    public void CalculateExpNeedToLevelUP()
    {
        int t = 100;
        totalExp = 0;
        for (int i = 1; i < coreLevel + 1; i++)
        {
            t += (i + 1) * 10;
            totalExp += t;
        }
        soulNeedToLevelUp = t;
        totalExp -= t;
        if (totalExp == 0)
            totalExp = 100;
    }

    public void LevelMonsterUp()
    {
        //pointRemain--;
        //level += levelIns;
        foreach(MonsterData data in MonsterInventory.Instance.GetListData())
        {
            if (data.rank == MonsterRank.Minion)
                data.SetLevelUpTo((int)coreLevel);
        }
        UpdateAfterLevelUp();
    }

    public void HPUp()
    {
        pointRemain--;
        hp += hpIns;
        UpdateAfterLevelUp();
    }

    public void ATKUp()
    {
        pointRemain--;
        atk += atkIns;
        UpdateAfterLevelUp();
    }

    public void CriticalRateUp()
    {
        pointRemain--;
        criticalRate += criticalRateIns;
        UpdateAfterLevelUp();
    }

    public void CriticalDamageUp()
    {
        pointRemain--;
        criticalDmg += criticalDmgIns;
        UpdateAfterLevelUp();
    }

    public void SoulgainUp()
    {
        pointRemain--;
        soulGain += soulGainIns;
        UpdateAfterLevelUp();
    }

    public float GetSoulNeedToLevelUp()
    {
        return soulNeedToLevelUp;
    }

    [Button]
    public void UnlockSoulConcentrated()
    {
        //isSoulConcentratedIsLock = false;
        dungeonCoreUI.UnlockSoulConcentrated();
    }

    public void CreateSoulConcentrated()
    {
        if (PlayerCurrency.Instance.Soul < 100)
            return;
        PlayerCurrency.Instance.DeltaSoul(-100);
        PlayerCurrency.Instance.DeltaSoulConcentrated(1);
    }

    public void GetLegionBasicBonus()
    {
        foreach (LegionCore legion in listLegionCore)
        {
            float bonus = 0;
            legion.criticalRateBonus = 0;
            foreach (MonsterData data in MonsterInventory.Instance.GetListData())
            {
                if (data.legion == legion.legionID)
                {
                    switch (data.rarity)
                    {
                        case Rarity.Tier1:
                            bonus += 0.1f;
                            break;
                        case Rarity.Tier2:
                            bonus += 1f;
                            break;
                        case Rarity.Tier3:
                            bonus += 10f;
                            break;
                        case Rarity.Tier4:
                            bonus += 100f;
                            break;
                    }
                }
            }

            legion.attackFlatBonus = bonus;
            legion.hpFlatBonus = bonus * 10;
            if (T_LegionUpgrade1)
                legion.criticalRateBonus = bonus * 0.1f;
            if (T_LegionUpgrade2)
                legion.dmgOutputBonus = bonus;

        }
    }

    public void LoadTreasureStat()
    {
        Debug.Log("Update Treasure Stat!");
        TreasureInventory treasureInventory = TreasureInventory.Instance;
        T_SoulGain = 0;
        T_corruptedSoulGain = 0;
        T_Atk = 0;
        T_HP = 0;
        T_DiamondBuff = 0;
        T_CriticalRate = 0;
        T_HealingBonus = 0;
        T_DmgBonus = 0;
        T_ATKSPD = 0;
        T_BossAtk = 0;
        T_BossHP = 0;
        T_BossMinionAtk = 0;
        T_BossMinionHP = 0;
        T_Shield = 0;
        T_PrisonCapaciy = 0;
        T_BetterPrison = false;
        T_LegionUpgrade1 = false;
        T_LegionUpgrade2 = false;
        foreach (Treasure item in treasureInventory.GetListData())
        {
            Debug.Log("Treasure ID: " +     item.id);
            switch (item.id)
            {
                case "TREA_001":
                    T_SoulGain += item.value1 * item.stack;
                    break;
                case "TREA_002":
                    T_SoulGain += item.value1;
                    break;
                case "TREA_003":
                    T_SoulGain += item.value1;
                    break;
                case "TREA_004":
                    T_SoulGain += 2 * PrisonerInventory.Instance.GetListData().Count;
                    break;
                case "TREA_005":
                    T_corruptedSoulGain += item.value1;
                    break;
                case "TREA_006":
                    T_Atk += item.value1 * item.stack;
                    break;
                case "TREA_007":
                    T_Atk += item.value1 * item.stack;
                    break;
                case "TREA_008":
                    T_Atk += item.value1;
                    break;
                case "TREA_009":
                    T_Atk += item.value1;
                    break;
                case "TREA_010":
                    T_HP += item.value1 * item.stack;
                    break;
                case "TREA_011":
                    T_HP += item.value1 * item.stack;
                    break;
                case "TREA_012":
                    T_DiamondBuff += item.value1;
                    break;
                case "TREA_013":
                    T_DiamondBuff += item.value1;
                    break;
                case "TREA_014":
                    T_HP += item.value1;
                    break;
                case "TREA_015":
                    T_CriticalRate += item.value1;
                    break;
                case "TREA_016":
                    T_CriticalRate += item.value1;
                    break;
                case "TREA_017":
                    T_HealingBonus += item.value1;
                    break;
                case "TREA_018":
                    T_DmgBonus += item.value1;
                    break;
                case "TREA_019":
                    T_ATKSPD += item.value1;
                    break;
                case "TREA_020":
                    T_BossAtk += item.value1;
                    break;
                case "TREA_021":
                    T_BossMinionHP += item.value1;
                    break;
                case "TREA_022":
                    T_BossMinionHP += item.value1;
                    break;
                case "TREA_023":
                    T_BossMinionHP += item.value1;
                    break;
                case "TREA_024":
                    T_BossHP += item.value1;
                    break;
                case "TREA_025":
                    T_BossHP += item.value1;
                    break;
                case "TREA_026":
                    T_BossHP += item.value1;
                    break;
                case "TREA_027":
                    T_BossHP += item.value1;
                    break;
                case "TREA_029":
                    T_BossAtk += item.value1;
                    break;
                case "TREA_030":
                    T_Shield += item.value1;
                    break;
                case "TREA_031":
                    T_Shield += item.value1;
                    break;
                case "TREA_032":
                    T_Shield += item.value1;
                    break;
                case "TREA_033":
                    T_Shield += item.value1;
                    break;
                case "TREA_034":
                    BuildingController.Instance.UnlockWeakTrap();
                    break;
                case "TREA_035":
                    BuildingController.Instance.UnlockGumTrap();
                    break;
                case "TREA_036":
                    BuildingController.Instance.UnlockGumTrapLevel10();
                    break;
                case "TREA_037":
                    BuildingController.Instance.UnlockGumTrapLevel20();
                    break;
                case "TREA_038":
                    BuildingController.Instance.UnlockSoulExtractorLevel10();
                    break;
                case "TREA_039":
                    T_PrisonCapaciy = item.value1 * item.stack;
                    break;
                case "TREA_040":
                    BuildingController.Instance.UnlockGiantBuild();
                    break;
                case "TREA_041":
                    BuildingController.Instance.UnlockSoulSucker();
                    break;
                case "TREA_042":
                    BuildingController.Instance.UnlockSoulExtractorLevel20();
                    break;
                case "TREA_043":
                    
                    break;
                case "TREA_044":
                    T_BetterPrison = true;
                    break;
                case "TREA_045":
                    T_LegionUpgrade1 = true;
                    break;
                case "TREA_046":
                    T_LegionUpgrade2 = true;
                    break;
            }
        }
        
    }
}
