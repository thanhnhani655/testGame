using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TreasureInventory : MonoBehaviour
{
    private static TreasureInventory instance;
    public static TreasureInventory Instance => instance;
    [SerializeField]
    TreasureDatabase database;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    [SerializeField]
    private List<Treasure> listTreasure;

    public List<Treasure> GetListData()
    {
        return listTreasure;
    }

    public void AddTreasure(Treasure treasure)
    {
        Treasure temp = listTreasure.Find(x => x.id == treasure.id);
        if (temp == null)
            listTreasure.Add(treasure);
        else
            temp.stack++;

        DungeonCore.Instance.LoadTreasureStat();
    }
    [Button]
    public void AddTreasure(string id)
    {
        Treasure treasure = database.GetTreasureData(id);
        Treasure temp = listTreasure.Find(x => x.id == treasure.id);
        if (temp == null)
            listTreasure.Add(treasure);
        else
            temp.stack++;

        DungeonCore.Instance.LoadTreasureStat();
    }

    public void Clear()
    {
        listTreasure.Clear();
    }

}
