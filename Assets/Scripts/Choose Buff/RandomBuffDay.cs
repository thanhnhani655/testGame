using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBuffDay : MonoBehaviour
{
    public event System.Action OnEndPhase = delegate { };

    public BuffDayController buffDayController;

    public BuffDayUIController buffDayUI;

    public List<BuffDay> listBuffDay;

    public List<BuffDay> chooseableBuff;

    public List<BuffCard> listBuffCard;

    public WaveGenerator waveGenerator;

    private void Start()
    {
        chooseableBuff = new List<BuffDay>();
    }

    public void RandomBuff()
    {
        chooseableBuff.Clear();

        List<BuffDay> pool;
        pool = GetPool();



        //Buff #1
        int index = Random.Range(0, pool.Count);
        chooseableBuff.Add(pool[index]);
        pool.Remove(pool[index]);
        listBuffCard[0].SetBuffCard(chooseableBuff[0]);

        //Buff #2
        index = Random.Range(0, pool.Count);
        chooseableBuff.Add(pool[index]);
        pool.Remove(pool[index]);
        listBuffCard[1].SetBuffCard(chooseableBuff[1]);

        //Buff #3
        index = Random.Range(0, pool.Count);
        chooseableBuff.Add(pool[index]);
        pool.Remove(pool[index]);
        listBuffCard[2].SetBuffCard(chooseableBuff[2]);

        buffDayUI.ShowBuffDayUI_Begin();
    }

    public void ChooseBuff(int index)
    {
        buffDayController.SetActiveBuff(chooseableBuff[index]);
        buffDayUI.BuffChoosen(index);

        
    }

    private List<BuffDay> GetPool()
    {
        List<BuffDay> pool = new List<BuffDay>();
        int levelinRow = waveGenerator.levelInRow;
        int day = DayCounter.Instance.Day;

        if (day < 10)
        {
            foreach (BuffDay buff in listBuffDay)
            {
                if (buff.tier == Rarity.Tier1)
                    this.AddPool(pool, buff, 6);
                if (buff.tier == Rarity.Tier2)
                    this.AddPool(pool, buff, 3);
            }
        }
        else if (day == 10)
        {
            foreach (BuffDay buff in listBuffDay)
            {
                if (buff.tier == Rarity.Tier2)
                    this.AddPool(pool, buff, 3);
            }
        }
        else if (day < 20)
        {
            foreach (BuffDay buff in listBuffDay)
            {
                if (buff.tier == Rarity.Tier1)
                    this.AddPool(pool, buff, 3);
                if (buff.tier == Rarity.Tier2)
                    this.AddPool(pool, buff, 6);
                if (buff.tier == Rarity.Tier3)
                    this.AddPool(pool, buff, 3);
            }
        }
        else if (day == 20)
        {
            foreach (BuffDay buff in listBuffDay)
            {
                if (buff.tier == Rarity.Tier2)
                    this.AddPool(pool, buff, 3);
                if (buff.tier == Rarity.Tier3)
                    this.AddPool(pool, buff, 6);
            }
        }
        else if (day < 30)
        {
            foreach (BuffDay buff in listBuffDay)
            {
                if (buff.tier == Rarity.Tier2)
                    this.AddPool(pool, buff, 3);
                if (buff.tier == Rarity.Tier3)
                    this.AddPool(pool, buff, 3);
            }
        }
        else if (day == 30)
        {
            foreach (BuffDay buff in listBuffDay)
            {
                if (buff.tier == Rarity.Tier3)
                    this.AddPool(pool, buff, 3);
            }
        }
        else if (day < 40)
        {
            foreach (BuffDay buff in listBuffDay)
            {
                if (buff.tier == Rarity.Tier2)
                    this.AddPool(pool, buff, 3);
                if (buff.tier == Rarity.Tier3)
                    this.AddPool(pool, buff, 3);
                if (buff.tier == Rarity.Tier4)
                    this.AddPool(pool, buff, 3);
            }
        }
        else if (day == 40)
        {
            foreach (BuffDay buff in listBuffDay)
            {
                if (buff.tier == Rarity.Tier3)
                    this.AddPool(pool, buff, 3);
                if (buff.tier == Rarity.Tier4)
                    this.AddPool(pool, buff, 3);
            }
        }
        else if (day < 50)
        {
            foreach (BuffDay buff in listBuffDay)
            {
                if (buff.tier == Rarity.Tier3)
                    this.AddPool(pool, buff, 3);
                if (buff.tier == Rarity.Tier4)
                    this.AddPool(pool, buff, 3);
            }
        }
        else
        {
            foreach (BuffDay buff in listBuffDay)
            {
                if (buff.tier == Rarity.Tier4)
                    this.AddPool(pool, buff, 3);
            }
        }


        return pool;
    }

    private void AddPool(List<BuffDay> pool, BuffDay buff, int count)
    {
        for (int i = 0; i < count; i++)
        {
            pool.Add(buff);
        }
    }

    public void EndPhase()
    {
        OnEndPhase();
    }
}
