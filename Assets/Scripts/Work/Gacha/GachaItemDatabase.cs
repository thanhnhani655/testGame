using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Gacha Item Database", menuName = "CustomAsset/Gacha Item Database", order = 1)]
public class GachaItemDatabase : ScriptableObject
{
    [SerializeField]
    public List<GachaItemData> listItems;

    public GachaItemData GetItem(string id)
    {
        return listItems.Find(x => x.itemID == id).CloneItem();
    }

    public List<GachaItemData> GetList()
    {
        return listItems;
    }
}
