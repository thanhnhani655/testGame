using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PrisonerInventory : MonoBehaviour
{
    private static PrisonerInventory instance;
    public static PrisonerInventory Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    [SerializeField]
    private MonsterDatabase database;
    [SerializeField]
    private List<MonsterData> listPrisoner;

    [SerializeField]
    private int prisonCapacity = 0;

    public bool IsPrisonFull()
    {
        if (listPrisoner.Count >= BuildingController.Instance.GetPrisonCapacity())
        {
            return true;
        }
        return false;
    }

    public void AddPrisoner(MonsterData data)
    {
        if (listPrisoner.Count >= prisonCapacity)
            return;
        Debug.Log("Add Prisoner");
        listPrisoner.Add(data);

        data.address = ItemAddress.Prison;
    }

    public void RemoveMonster(MonsterData data)
    {
        listPrisoner.Remove(data);
    }

    public void Clear()
    {
        listPrisoner.Clear();
    }

    public List<MonsterData> GetListData()
    {
        return listPrisoner;
    }


    [Button]
    public void AddPrisonerByID(string id)
    {
        listPrisoner.Add(database.GetDataByID(id));
    }

    public void SetCapacity(int icapacity)
    {
        prisonCapacity = icapacity;
    }
}
