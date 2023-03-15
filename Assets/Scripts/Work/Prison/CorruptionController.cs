using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptionController : MonoBehaviour
{
    private static CorruptionController instance;
    public static CorruptionController Instance => instance;

    
    private void Awake()
    {
        if (instance == null)
            instance = this;

        if (listCorruptor == null)
            listCorruptor = new List<MonsterData>();
        if (listTransfer == null)
            listTransfer = new List<InvaderToCorrupted>();
    }
    [SerializeField]
    private MonsterDatabase database;
    [SerializeField]
    private PrisonUI prisonUI;

    public int maxCorruptor = 0;
    public int currentCorruptor = 0;

    [SerializeField]
    private List<MonsterData> listCorruptor;
    [SerializeField]
    private List<InvaderToCorrupted> listTransfer;

    public void AddCorruptor(MonsterData invader)
    {
        listCorruptor.Add(invader);
        invader.isCorrupting = true;
        if (invader.rarity == Rarity.Tier1)
            invader.corruptionTime = 5;
        currentCorruptor++;
    }

    public void UpdateCorruptor()
    {
        foreach(MonsterData data in listCorruptor)
        {
            if (data.corruptionTime > 0)
                data.corruptionTime -= 1;
            if (data.corruptionTime == 0)
            {
                data.isCorrupting = false;
                data.isCorruptDone = true;
            }
        }
    }

    public void Clear()
    {
        listCorruptor.Clear();
    }

    public void GetCorrupted(MonsterData data)
    {
        InvaderToCorrupted temp = listTransfer.Find(x => x.invader == data.id);
        //MonsterData newMons = database.GetDataByID(temp.corrupted);
        MonsterInventory.Instance.AddMons(temp.corrupted);
        PrisonerInventory.Instance.RemoveMonster(data);
        prisonUI.LoadInventory();
        currentCorruptor--;
        currentCorruptor = 0;
    }
}

[System.Serializable]
public class InvaderToCorrupted
{
    public string invader;
    public string corrupted;
}
