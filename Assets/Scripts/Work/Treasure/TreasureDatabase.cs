using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TreasureDatabase", menuName = "CustomAsset/Treasure Database")]
public class TreasureDatabase : ScriptableObject
{
    [SerializeField]
    private List<Treasure> listTreasureData;

    public Treasure GetTreasureData(string id)
    {
        return listTreasureData.Find(x => x.id == id);
    }

    public List<Treasure> GetListData()
    {
        return listTreasureData;
    }
}
