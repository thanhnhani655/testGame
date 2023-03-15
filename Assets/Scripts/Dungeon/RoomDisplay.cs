using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomDisplay : MonoBehaviour
{
    [SerializeField]
    private Room room;
    [SerializeField]
    private SpriteRenderer roomSymbol;
    [SerializeField]
    private GameObject iconArea;
    [SerializeField]
    private MonsterIcon monsterIconPrefaps;
    [SerializeField]
    private GameObject BossIconArea;
    [SerializeField]
    private MonsterIcon BossIconPrefaps;

    [SerializeField]
    private List<MonsterIcon> listIcons;
    [SerializeField]
    private GameObject LockInsideHighlight;

    private void Start()
    {
        listIcons = new List<MonsterIcon>();
    }

    public void UpdateIconAmount(List<MonsterData> listDatas)
    {
        ClearIcon();

        foreach (MonsterData data in listDatas)
        {
            if (!data.isDead)
                this.AddIcon(data);
        }

    }

    public void AddIcon(MonsterData data)
    {
        if (data.rank == MonsterRank.Boss)
        {
            MonsterIcon newBossIcon = Instantiate<MonsterIcon>(BossIconPrefaps, BossIconArea.transform);
            newBossIcon.SetData(data);
            listIcons.Add(newBossIcon);
            return;
        }

        MonsterIcon newIcon = Instantiate<MonsterIcon>(monsterIconPrefaps, iconArea.transform);
        newIcon.SetData(data);
        listIcons.Add(newIcon);
    }

    public void ClearIcon()
    {
        foreach (MonsterIcon icon in listIcons)
        {
            Destroy(icon.gameObject);
        }

        listIcons.Clear();
    }

    public void UpdateFacilityDisplay()
    {
        if (room.facility == null)
        {
            roomSymbol.gameObject.SetActive(false);
            return;
        }
        roomSymbol.sprite = room.facility.sprite;
        roomSymbol.gameObject.SetActive(true);
    }

    public void LockInside()
    {
        LockInsideHighlight.SetActive(true);
    }

    public void UnLockInside()
    {
        LockInsideHighlight.SetActive(false);
    }
}
