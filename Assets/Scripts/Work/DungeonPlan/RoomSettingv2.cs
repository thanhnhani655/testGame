using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSettingv2 : MonoBehaviour
{
    [SerializeField]
    private DungeonManagementUI dungeonManager;
    public List<Vector3> listPosCard;

    public List<GameObject> listCard;

    public GameObject AddableCard;
    public MonsterCard prefapMonsterCard;

    public Transform cardHolder;

    public Room room;

    public void LoadRoom(Room iroom)
    {
        room = iroom;
        Clear();

        int maxMonsinRoom = 3;
        if (room.isBossRoom)
            maxMonsinRoom = 4;

        for (int i = 0; i < maxMonsinRoom; i++)
        {
            if (i < room.ListMonInRoom.Count)
            {
                MonsterCard newCard = Instantiate<MonsterCard>(prefapMonsterCard, cardHolder);
                newCard.SetMonster(room.ListMonInRoom[i]);
                newCard.transform.localPosition = listPosCard[i];
                newCard.manager = dungeonManager;

                listCard.Add(newCard.gameObject);
            }
            else
            {
                GameObject newCard = Instantiate<GameObject>(AddableCard, cardHolder);
                newCard.transform.localPosition = listPosCard[i];

                listCard.Add(newCard.gameObject);
            }
        }
    }

    public void Clear()
    {
        foreach (GameObject card in listCard)
        {
            Destroy(card);
        }

        listCard.Clear();
    }


    public void Show(Room iroom)
    {
        this.gameObject.SetActive(true);
        room = iroom;
        this.LoadRoom(room);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void RemoveMonster(MonsterData data)
    {
        room.RemoveMonster(data);
        LoadRoom(room);
    }

    public void AddMonster(MonsterData data)
    {
        if (data.rank == MonsterRank.Boss)
        {
            if (!room.isBossRoom)
                return;
            if (room.ListMonInRoom.FindAll(x => x.rank == MonsterRank.Minion).Count <= room.maxBossInRoom)
            {
                room.AddMonster(data);
                data.address = ItemAddress.Dungeon;
                data.dungeonAddress = this.room.RoomID;
                DungeonCore.Instance.BossFloor1 = data;

                LoadRoom(room);
            }

            return;
        }

        if (room.ListMonInRoom.FindAll(x => x.rank == MonsterRank.Minion).Count < room.maxMonsterInRoom)
        {
            room.AddMonster(data);
            data.address = ItemAddress.Dungeon;
            data.dungeonAddress = this.room.RoomID;


            LoadRoom(room);
        }
    }
}
