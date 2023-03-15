using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusNewgame : MonoBehaviour
{
    private static BonusNewgame instance;
    public static BonusNewgame Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }


    private void Start()
    {
        gachaBanner.OnGachaResult += GetBonus;
    }

    [SerializeField]
    private List<BonusData> listData;
    [SerializeField]
    private List<BonusData> poolData;
    [SerializeField]
    private GachaBanner gachaBanner;

    public int dailySummonInscrease = 0;
    public int StartWithMoreSoul = 0;
    public int StartWithChanceGetBetterMonster;
    public int StartWithChanceGetBetterBoss;
    public int StartWithDungeonCoreLevel = 1;
    public int StartWith1RandomTreasureTier1 = 0;
    //public int InscreaseSummonRate;
    
    public void GachaBonus()
    {
        gachaBanner.Gacha3Choose1();
    }

    public void GetBonus(GachaItemData data)
    {
        Debug.Log("Get Bonus");
        BuyBonus(data.itemID);
    }

    public void BuyBonus(string id)
    {

        switch(id)
        {
            case "NGBN_001":
                dailySummonInscrease++;
                break;
            case "NGBN_002":
                dailySummonInscrease++;
                break;
            case "NGBN_003":
                StartWithMoreSoul += 50;
                break;
            case "NGBN_004":
                StartWithChanceGetBetterMonster++;
                break;
            case "NGBN_005":
                StartWithChanceGetBetterBoss++;
                break;
            case "NGBN_006":
                StartWithDungeonCoreLevel++;
                break;
            case "NGBN_007":
                StartWith1RandomTreasureTier1++;
                break;
            case "NGBN_008":
                break;
            case "NGBN_009":
                break;
            case "NGBN_010":
                break;
            case "NGBN_011":
                break;
            case "NGBN_012":
                break;
            case "NGBN_013":
                break;
            case "NGBN_014":
                break;
            case "NGBN_015":
                break;
        }
    }

    public bool CheckPrice(BonusData data)
    {
        if (PlayerCurrency.Instance.SoulStone < data.price)
            return false;
        return true;
    }

    public bool CheckPriceAndPay(BonusData data)
    {
        if (CheckPrice(data))
        {
            PlayerCurrency.Instance.DeltaSoulStone(-data.price);
            return true;
        }

        return false;
    }
}

[System.Serializable]
public class BonusData
{
    public string id;
    public int price;
    public float value;
    public int maxCopy;
    public int priceInscreasePerCopy;
}
