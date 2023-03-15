using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GachaBanner : MonoBehaviour
{
    public event System.Action<GachaItemData> OnGachaResult = delegate { };

    public string id;
    public GachaItemDatabase database;
    public List<GachaItemData> listGachaAble;
    public List<GachaItemData> poolItem;
    public float soulPrice = 100;
    public float soulPriceInscrease = 50;
    public GachaTripleInterface gachaMethod;
    public GachaSingleInterface gachaSingle;
    public Gacha3Choose1 gacha3_1;
    public GameObject warning;

    [Header ("Rarity ratio")]
    public float tier1BaseRatio;
    public float tier2BaseRatio;
    public float tier3BaseRatio;
    public float tier4BaseRatio;

    public float tier1Ratio;
    public float tier2Ratio;
    public float tier3Ratio;
    public float tier4Ratio;

    public bool IsUsePool2PullItem = false;

    private void Start()
    {
        tier1Ratio = tier1BaseRatio;
        tier2Ratio = tier2BaseRatio;
        tier3Ratio = tier3BaseRatio;
        tier4Ratio = tier4BaseRatio;

        if (gacha3_1 != null)
            gacha3_1.OnGachaResult += GetItemGacha3_1;

        if (IsUsePool2PullItem)
        {
            poolItem = new List<GachaItemData>();
            foreach (GachaItemData data in listGachaAble)
            {
                poolItem.Add(data.CloneItem());
            }
        }
    }

    private void OnEnable()
    {
        warning.SetActive(false);
    }

    [Button]
    public void AddItemData(string id)
    {
        GachaItemData newItem = database.GetItem(id);
        listGachaAble.Add(newItem);
    }

    [Button]
    public void UpdateListItem()
    {
        for (int i = 0; i < listGachaAble.Count; i++)
        {
            listGachaAble[i] = database.GetItem(listGachaAble[i].itemID);
        }
    }

    [Button]
    public void AddAll()
    {
        List<GachaItemData> temp = database.GetList();

        foreach (GachaItemData item in temp)
        {
            listGachaAble.Add(database.GetItem(item.itemID));
        }
    }

    private Rarity RandomTier()
    {
        float rate = Random.value;
        if (rate <=  tier1Ratio)
        {
            return Rarity.Tier1;
        }
        if (rate <= tier1Ratio + tier2Ratio)
        {
            return Rarity.Tier2;
        }
        if (rate <= tier1Ratio + tier2Ratio + tier3Ratio)
        {
            return Rarity.Tier3;
        }
        else
        {
            return Rarity.Tier4;
        }
    }

    public GachaItemData RandomItem()
    {
        GachaItemData item;
        Rarity rarity = RandomTier();

        List<GachaItemData> listItemInRarity = new List<GachaItemData>();
        if (!IsUsePool2PullItem)
            listItemInRarity = listGachaAble.FindAll(x => x.itemRarity == rarity);
        else
            listItemInRarity = poolItem.FindAll(x => x.itemRarity == rarity);

        List<GachaItemData> listMinion = listItemInRarity.FindAll(x => x.rank == MonsterRank.Minion);
        List<GachaItemData> listBoss = listItemInRarity.FindAll(x => x.rank == MonsterRank.Boss);

        int Index = Random.Range(0,listItemInRarity.Count);
        item = listItemInRarity[Index].CloneItem();

        if (listBoss.Count > 0)
        {
            if (Random.value <= 0.05f)
            {
                Index = Random.Range(0, listBoss.Count);
                item = listBoss[Index].CloneItem();
            }
            else
            {
                Index = Random.Range(0, listMinion.Count);
                item = listMinion[Index].CloneItem();
            }
        }

        return item;
    }

    public void Gacha()
    {
        if (soulPrice > PlayerCurrency.Instance.Soul)
        {
            warning.SetActive(true);
            return;
        }
        warning.SetActive(false);

        PlayerCurrency.Instance.DeltaSoul(-soulPrice);
        soulPrice += soulPriceInscrease;

        gachaMethod.Gacha();
    }

    public void GachaSingle()
    {
        if (Parameter.Instance.DailyGachaRemain == 0)
        {
            warning.SetActive(true);
            return;
        }
        warning.SetActive(false);

        Parameter.Instance.DailyGachaRemain--;

        gachaSingle.Gacha();
    }

    public void SetRatioByLevel(int level)
    {
        int maxLevel = 50;
        if (level > maxLevel)
            level = maxLevel;

        float tier1RatioPerLevel = Mathf.Abs(tier1BaseRatio - 0.5f) / maxLevel;
        tier1Ratio = tier1BaseRatio - tier1RatioPerLevel * level;

        float tier2RatioPerLevel = Mathf.Abs(0.3f - tier2BaseRatio) / maxLevel;
        tier2Ratio = tier2BaseRatio + tier2RatioPerLevel * level;

        float tier3RatioPerLevel = Mathf.Abs(0.15f - tier3BaseRatio) / maxLevel;
        tier3Ratio = tier3BaseRatio + tier3RatioPerLevel * level;

        float tier4RatioPerLevel = Mathf.Abs(0.05f - tier4BaseRatio) / maxLevel;
        tier4Ratio = tier4BaseRatio + tier4RatioPerLevel * level;
    }

    public void Gacha3Choose1()
    {
        if (soulPrice > PlayerCurrency.Instance.Soul)
        {
            warning.SetActive(true);
            return;
        }
        warning.SetActive(false);

        PlayerCurrency.Instance.DeltaSoulStone(-soulPrice);
        soulPrice += soulPriceInscrease;

        gacha3_1.GachaItem(); 
    }

    public void GetItemGacha3_1(GachaItemData gachaItem)
    {
        if (IsUsePool2PullItem)
        {
            GachaItemData data = poolItem.Find(x => x.gachaID == gachaItem.gachaID);
            data.copy--;

            if (data.copy == 0)
                poolItem.Remove(data);
        }

        this.OnGachaResult(gachaItem);

    }
}


