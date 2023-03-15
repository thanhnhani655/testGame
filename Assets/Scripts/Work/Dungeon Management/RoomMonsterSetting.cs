using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMonsterSetting : MonoBehaviour
{
    [SerializeField]
    private Room roomSetting;
    [SerializeField]
    private RoomPlanUI roomPlanUISetting;
    [SerializeField]
    private GameObject ForMonsterUI;
    [SerializeField]
    private GameObject ForEffectUI;
    [SerializeField]
    private GameObject MonCardArea;


    [SerializeField]
    private List<GameObject> listMonCard;

    [SerializeField]
    private MonsterCard monCardPrefaps;
    [SerializeField]
    private GameObject AddableCardPrefaps;

    private void Start()
    {
        if (listMonCard == null)
            listMonCard = new List<GameObject>();
    }

    public void SettingUIActive(Room iroom)
    {
        this.gameObject.SetActive(true);
        ForMonsterUI.SetActive(true);
        ForEffectUI.SetActive(false);

        
        this.SetRoom(iroom);
        LoadMonster();
        
    }

    public void SettingUIActive(RoomPlanUI iroom)
    {
        this.gameObject.SetActive(true);
        ForMonsterUI.SetActive(true);
        ForEffectUI.SetActive(false);


        this.SetRoom(iroom);
        LoadMonster();

    }

    public void SetRoom(Room iRoom)
    {
        roomSetting = iRoom;   
    }

    public void SetRoom(RoomPlanUI iRoom)
    {
        roomPlanUISetting = iRoom;
    }

    public void LoadMonster()
    {
        if (listMonCard == null)
            listMonCard = new List<GameObject>();
        ClearCard();

        if (roomSetting.isBossRoom)
        {
            foreach (MonsterData data in roomSetting.ListMonInRoom.FindAll(x=>x.rank == MonsterRank.Boss))
            {
                MonsterCard newCard = Instantiate<MonsterCard>(monCardPrefaps, MonCardArea.transform);
                newCard.SetMonster(data);
                newCard.manager = DungeonManagementUI.Instance;
                listMonCard.Add(newCard.gameObject);
            }

            if (roomSetting.ListMonInRoom.FindAll(x=>x.rank == MonsterRank.Boss).Count < roomSetting.maxBossInRoom)
            {
                GameObject newCard = Instantiate<GameObject>(AddableCardPrefaps, MonCardArea.transform);
                listMonCard.Add(newCard.gameObject);
            }
        }
        
        foreach (MonsterData data in roomSetting.ListMonInRoom.FindAll(x=>x.rank == MonsterRank.Minion))
        {
            MonsterCard newCard = Instantiate<MonsterCard>(monCardPrefaps, MonCardArea.transform);
            newCard.SetMonster(data);
            newCard.manager = DungeonManagementUI.Instance;
            listMonCard.Add(newCard.gameObject);
        }

        if (roomSetting.ListMonInRoom.FindAll(x => x.rank == MonsterRank.Minion).Count < roomSetting.maxMonsterInRoom)
        {
            GameObject newCard = Instantiate<GameObject>(AddableCardPrefaps, MonCardArea.transform);
            listMonCard.Add(newCard.gameObject);
        }
    }

    public void ClearCard()
    {
        foreach (GameObject card in listMonCard)
        {
            Debug.Log("Destroy " + card.name);
            Destroy(card.gameObject);
        }

        listMonCard.Clear();
    }

    public void AddMonster(MonsterData data)
    {
        if (data.rank == MonsterRank.Boss)
        {
            if (!roomSetting.isBossRoom)
                return;
            if (roomSetting.ListMonInRoom.FindAll(x => x.rank == MonsterRank.Minion).Count <= roomSetting.maxBossInRoom)
            {
                roomSetting.AddMonster(data);
                data.address = ItemAddress.Dungeon;
                data.dungeonAddress = this.roomSetting.RoomID;
                DungeonCore.Instance.BossFloor1 = data;

                LoadMonster();
            }

            return;
        }

        if (roomSetting.ListMonInRoom.FindAll(x=>x.rank == MonsterRank.Minion).Count < roomSetting.maxMonsterInRoom)
        {
            roomSetting.AddMonster(data);
            data.address = ItemAddress.Dungeon;
            data.dungeonAddress = this.roomSetting.RoomID;


            LoadMonster();
        }
    }

    public void RemoveMonster(MonsterData data)
    {
        roomSetting.RemoveMonster(data);
        LoadMonster();
    }

    public void CloseScene()
    {
        this.gameObject.SetActive(false);
    }
}
