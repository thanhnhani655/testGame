using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    #region Room Connection
    // ID: XY 
    public string RoomID;
    public bool isLocked;
    public bool isLockedInside = false;
    public List<Room> listConnectRoom;

    public void AddConnection(Room connectRoom)
    {
        listConnectRoom.Add(connectRoom);
    }

    public void ClearConnection()
    {
        listConnectRoom.Clear();
    }

    public List<Room> GetAllConnectRoom()
    {
        List<Room> listConnectableRoom = new List<Room>();
        foreach (Room room in listConnectRoom)
        {
            if (room.isLocked)
                continue;
            listConnectableRoom.Add(room);
        }
        return listConnectableRoom;
    }
    #endregion

    public bool isBossRoom;
    public int maxMonsterInRoom = 2;
    public int maxBossInRoom = 1;
    [SerializeField]
    private List<MonsterData> listMonInRoom;
    [SerializeField]
    private RoomDisplay roomDisplay;
    [SerializeField]
    private List<Invader> listInvaderInRoom;
    public List<MonsterData> ListMonInRoom { get => listMonInRoom;}
    public List<Invader> ListInvaderInRoom { get => listInvaderInRoom;}

    [SerializeField]
    private CombatController combatController;

    [SerializeField]
    public Facility facility;

    public void AddFacility(Facility ifac)
    {
        facility = ifac;
        facility.address = this;
        roomDisplay.UpdateFacilityDisplay();
    }

    private void Start()
    {
        if (listInvaderInRoom == null)
            listInvaderInRoom = new List<Invader>();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (DungeonManagementUI.Instance != null)
                if (DungeonManagementUI.Instance.IsRoomInteracable)
                    DungeonManagementUI.Instance.ActiveRoomSetting(this);
            if (BuildingController.Instance.isBuildMode)
            {
                if (!BuildingController.Instance.isBuilding)
                {
                    BuildingController.Instance.SelectRoom(this);
                }
            }

            if (InvadeController.Instance.isInvadePhase)
            {
                BeInvadeUI.Instance.roomInformation.ActiveRoomInformation(this);
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            LockInside();
        }
    }

    public void AddMonster(MonsterData data)
    {
        listMonInRoom.Add(data);
        data.OnDead += UpdateDisplay;
        data.OnResetAlive += UpdateDisplay;
        this.UpdateDisplay();
    }

    public void RemoveMonster(MonsterData data)
    {
        listMonInRoom.Remove(data);
        data.OnDead -= UpdateDisplay;
        data.OnResetAlive -= UpdateDisplay;
        this.UpdateDisplay();
    }

    public void RemoveAllMonster()
    {
        while (true)
        {
            if (listMonInRoom.Count == 0)
                break;
            this.RemoveMonster(listMonInRoom[0]);
        }
    }

    public void UpdateDisplay()
    {
        roomDisplay.UpdateIconAmount(listMonInRoom);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Invader>() != null)
        {
            ListInvaderInRoom.Add(other.GetComponent<Invader>());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Invader>() != null)
        {
            other.GetComponent<Invader>().isJustFight = false;
            ListInvaderInRoom.Remove(other.GetComponent<Invader>());
        }
    }

    public void StartCombat()
    {
        if (Parameter.Instance.debugCombatController)
        {
            Debug.Log("Start a Combat!");
        }
        List<MonsterData> listCombatParticipate = new List<MonsterData>();

        foreach (Invader data in ListInvaderInRoom)
        {
            if (!data.isJustFight)
            {
                listCombatParticipate.Add(data.data);
            }
        }

        //foreach (MonsterData data in listMonInRoom)
        //{
        //    if (!data.isDead)
        //        listCombatParticipate.Add(data);
        //}

        foreach (MonsterData data in listCombatParticipate)
        {
            if (!data.isDead)
                combatController.GetAllTargetAble(data, this);
        }

        foreach (Invader data in ListInvaderInRoom)
        {
            if (!data.isJustFight)
            {
                data.isJustFight = true;
            }
            if (data.data.isTakeDmg)
            {
                data.ShowDmg();
                data.data.isTakeDmg = false;
                data.GetComponent<ShakeObject>().Shake();
                data.GetComponent<ShakeObject>().ShakeDone += data.GetComponent<MobInDungeon>().CombatDone;
                continue;
            }
            else
            {
                data.GetComponent<MobInDungeon>().CombatDone();
            }
        }
    }

    private void Update()
    {
        if (InvadeController.Instance.isInvadePhase)
        {
            MonsterAction();

            if (isLockedInside)
                if (listMonInRoom.Count == 0 || !listMonInRoom.Exists(x=>x.isDead == false))
                {
                    this.UnlockInside();
                }
        }
    }

    public void MonsterAction()
    {
        foreach (MonsterData data in listMonInRoom)
        {
            if (data.isDead)
                continue;

            data.baseStat.timeCounter += Time.deltaTime * Parameter.Instance.SPEED;
            if (data.baseStat.timeCounter < data.baseStat.attackSpeed)
                continue;

            data.baseStat.timeCounter -= data.baseStat.attackSpeed;
            
            combatController.GetAllTargetAble(data, this);
        }
    }

    public void LockInside()
    {
        isLockedInside = true;
        roomDisplay.LockInside();
    }

    public void UnlockInside()
    {
        isLockedInside = false;
        roomDisplay.UnLockInside();
    }

    public void RemoveFacility()
    {
        facility = null;
        roomDisplay.UpdateFacilityDisplay();
    }
}

    
