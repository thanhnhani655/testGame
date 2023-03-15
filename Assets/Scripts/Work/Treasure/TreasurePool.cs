using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TreasurePool : MonoBehaviour
{
    public TreasureDatabase treasureDatabase;
    public List<Treasure> listTreasure;

    public Treasure PullOutItem(Rarity rarity)
    {
        Treasure newTreasure;
        List<Treasure> listTreasurePullAble = listTreasure.FindAll(x => x.rarity == rarity);
        newTreasure = listTreasurePullAble[Random.Range(0, listTreasurePullAble.Count)];

        newTreasure.maxduplicate--;
        if (newTreasure.maxduplicate == 0)
        {
            listTreasure.Remove(newTreasure);
        }

        return newTreasure.CloneTreasure();
    }

    [Button]
    public void UpdateFromDatabase()
    {
        foreach (Treasure item in treasureDatabase.GetListData())
        {
            listTreasure.Add(item.CloneTreasure());
        }
    }
}
