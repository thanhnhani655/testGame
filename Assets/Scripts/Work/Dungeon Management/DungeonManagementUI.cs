using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonManagementUI : MonoBehaviour
{
    private static DungeonManagementUI instance;
    public static DungeonManagementUI Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    
    [SerializeField]
    private GameObject Dungeon;
    [SerializeField]
    private GameObject ManagementUI;
    [SerializeField]
    private RoomSettingv2 roomSettingv2;
    [SerializeField]
    private DungeonPlanUI dungeonPlanUI;
    [SerializeField]
    private bool isActive = false;

    public void OnEnable()
    {
        this.ActiveScene();
    }

    public void OnDisable()
    {
        this.DeactiveScene();
    }

    public void ActiveScene()
    {
        //Dungeon.SetActive(true);
        roomMonSetting.CloseScene();
        dungeonPlanUI.Show();
        roomSettingv2.Hide();
        LoadInventory();
        isActive = true;
    }

    public void DeactiveScene()
    {
        if (Dungeon != null)
            Dungeon.SetActive(false);
        isActive = false;
    }

    [SerializeField]
    private MonsterInventory inventory;
    [SerializeField]
    private List<MonsterCard> listCards;

    [SerializeField]
    private MonsterCard miniCardPrefap;
    [SerializeField]
    private Transform CardArea;
    [SerializeField]
    private GameObject ZoomArea;
    [SerializeField]
    private MonsterCard zoomedCard;

    [SerializeField]
    private RoomMonsterSetting roomMonSetting;

    public bool IsRoomInteracable { get => (isActive && !roomMonSetting.gameObject.activeSelf); }
    public bool IsActive { get => isActive; }

    #region Initialize
    private void Start()
    {
        if (listCards == null)
            listCards = new List<MonsterCard>();
    }

    public void LoadInventory()
    {
        Debug.Log("Load Inventory");
        this.ClearCard();
        inventory.SortInventory();
        List<MonsterData> listDatas = inventory.GetListData();


        foreach (MonsterData data in listDatas)
        {
            if (data.address != ItemAddress.Inventory)
                continue;
            MonsterCard newCard = Instantiate<MonsterCard>(miniCardPrefap, CardArea);
            newCard.SetMonster(data);
            newCard.manager = this;
            listCards.Add(newCard);
            Debug.Log(data.monName + " " + data.address.ToString());
        }
    }

    public void ClearCard()
    {
        foreach (MonsterCard card in listCards)
        {
            Destroy(card.gameObject);
        }

        listCards.Clear();
    }
    #endregion

    #region Function Zoom
    public void Zoom(MonsterData data)
    {
        ZoomArea.SetActive(true);
        zoomedCard.SetMonster(data);
        zoomedCard.ActiveMonsterDataUI();
    }

    public void DeactiveZoomCard()
    {
        ZoomArea.SetActive(false);
    }
   
    #endregion

    public void ActiveRoomSetting(Room room)
    {
        roomMonSetting.SettingUIActive(room);

    }

    public void ActiveRoomSettingV2(Room room)
    {
        dungeonPlanUI.Hide();
        roomSettingv2.Show(room);
    }

    public void ActiveDungeonPlanUI()
    {
        dungeonPlanUI.Show();
        roomSettingv2.Hide();
    }

    public void MoveMonster(MonsterData data)
    {

        if (!roomSettingv2.gameObject.activeSelf)
            return;

        switch (data.address)
        {
            case ItemAddress.Dungeon:
                //Dungeon To Inventory
                roomSettingv2.RemoveMonster(data);
                data.address = ItemAddress.Inventory;
                LoadInventory();
                break;
            case ItemAddress.Inventory:
                roomSettingv2.AddMonster(data);
                LoadInventory();
                break;
        }
    }
}
