using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MegaAdapter : MonoBehaviour
{
    private static MegaAdapter instance;
    public static MegaAdapter Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    [SerializeField]
    private GachaItemDatabase gachaDatabase;
    [SerializeField]
    private MonsterDatabase monsterDatabase;

    [Button]
    public void Synchronize ()
    {
        int gachaIndex = 1;
        //Synchronize Monster
        List<MonsterData> listMon = monsterDatabase.GetListDatas();
        gachaDatabase.listItems.Clear();
        foreach (MonsterData mon in listMon)
        {
            GachaItemData newItem = new GachaItemData();
            if (gachaIndex < 10)
                newItem.gachaID = "GAIT_00" + gachaIndex.ToString();
            else if (gachaIndex < 100)
                newItem.gachaID = "GAIT_0" + gachaIndex.ToString();
            else
                newItem.gachaID = "GAIT_" + gachaIndex.ToString();

            newItem.itemID = mon.id;
            newItem.itemName = mon.monName;
            newItem.itemRarity = mon.rarity;

            gachaDatabase.listItems.Add(newItem);
            gachaIndex++;
        }
        
    }
}

[System.Serializable]
public class AdapterData
{
    public string gachaID;
    public string itemID;
}

