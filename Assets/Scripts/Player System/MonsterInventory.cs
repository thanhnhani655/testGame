using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MonsterInventory : MonoBehaviour
{
    private static MonsterInventory instance;
    public static MonsterInventory Instance => instance;

    public MonsterDatabase database;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    [SerializeField]
    private List<MonsterData> monsterDatas;
    [SerializeField]
    private List<string> listStarterMonster;
    [SerializeField]
    private RoomManager roomManager;

    public void SetStarterInventory()
    {
        Clear();
        int betterMonsterRate = BonusNewgame.Instance.StartWithChanceGetBetterMonster;
        foreach (string id in listStarterMonster)
        {
            if (betterMonsterRate == 0)
                this.AddMons(id);
        }
        
        int tier1Rate = 10000;
        int tier2Rate = 100;
        int tier3Rate = 10;
        int tier4Rate = 1;

        if (betterMonsterRate > 0)
        {
            tier2Rate += 100 * betterMonsterRate;
            tier3Rate += 10 * betterMonsterRate;
            tier4Rate += 1 * betterMonsterRate;

            int total = tier1Rate + tier2Rate + tier3Rate + tier4Rate;
            int rate = 0;
            List<MonsterData> pool = new List<MonsterData>();
            for (int i = 0; i <= 6; i++)
            {
                Rarity rarity;
                rate = Random.Range(0, total);
                if (rate <= tier4Rate)
                {
                    rarity = Rarity.Tier4;
                }
                else if (rate <= tier4Rate + tier3Rate)
                {
                    rarity = Rarity.Tier3;
                }
                else if (rate <= tier4Rate + tier3Rate + tier2Rate)
                {
                    rarity = Rarity.Tier2;
                }
                else
                {
                    rarity = Rarity.Tier1;
                }

                pool.AddRange(DatabaseInstanceAccess.Instance.monsterDatabase.GetListDatas().FindAll(x => x.rarity == rarity));
                this.AddMonster(pool[Random.Range(0, pool.Count)].CloneMon());
            }
        }
    }

    private void Clear()
    {
        monsterDatas.Clear();
        roomManager.ClearMonster();
    }

    public void AddMonster(MonsterData data)
    {
        monsterDatas.Add(data);
        if (data.rank == MonsterRank.Minion)
            if (data.stat.Level < DungeonCore.Instance.coreLevel)
                data.SetLevelUpTo((int)DungeonCore.Instance.coreLevel);
        data.address = ItemAddress.Inventory;
    }

    public void RemoveMonster(MonsterData data)
    {
        monsterDatas.Remove(data);
    }

    public List<MonsterData> GetListData()
    {
        return monsterDatas;
    }

    [Button]
    public void AddMons(string id)
    {
        MonsterData newData = database.GetDataByID(id);

        this.AddMonster(newData);
    }

    public void SortInventory()
    {
        List<MonsterData> tempListBoss = new List<MonsterData>();
        List<MonsterData> tempListMons = new List<MonsterData>();


        for (int i = 0; i < monsterDatas.Count; i++)
        {
            //Boss
            if (monsterDatas[i].rank == MonsterRank.Boss)
            {
                if (tempListBoss.Count == 0)
                {
                    tempListBoss.Add(monsterDatas[i]);
                    continue;
                }

                int index = tempListBoss.IndexOf(tempListBoss.Find(x => x.stat.Level <= monsterDatas[i].stat.Level));
                if (index != -1)
                {
                    tempListBoss.Insert(index, monsterDatas[i]);
                }
                else
                {
                    tempListBoss.Add(monsterDatas[i]);
                }
                tempListBoss.Reverse();
            }

            if (monsterDatas[i].rank == MonsterRank.Minion)
            {
                tempListMons.Add(monsterDatas[i]);
                /*
                if (tempListMons.Count == 0)
                {
                    tempListMons.Add(monsterDatas[i]);
                    continue;
                }

                int index = tempListMons.IndexOf(tempListMons.Find(x => x.stat.Level <= monsterDatas[i].stat.Level));
                if (index != -1)
                {
                    tempListMons.Insert(index, monsterDatas[i]);
                }
                else
                {
                    tempListMons.Add(monsterDatas[i]);
                }
                tempListMons.Reverse();
                */
            }
        }

        monsterDatas.Clear();
        monsterDatas.AddRange(tempListBoss);
        monsterDatas.AddRange(tempListMons);
    }
}
