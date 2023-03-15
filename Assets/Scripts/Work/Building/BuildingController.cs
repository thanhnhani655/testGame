using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    private static BuildingController instance;
    public static BuildingController Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        if (listFacilityInDungeon == null)
            listFacilityInDungeon = new List<Facility>();
    }

    [SerializeField]
    private List<Facility> listSample;

    [SerializeField]
    private List<Facility> listFacilityInDungeon;

    [SerializeField]
    private BuildingUI buildingUI;

    [SerializeField]
    private GameObject prison;

    [SerializeField]
    private GachaBanner BannerTier1;

    [SerializeField]
    private SummonSceneUI summonController;

    [SerializeField]
    private BuildingUI buildUI;

    [SerializeField]
    public bool isLockGumTraplevel10 = true;
    [SerializeField]
    public bool isLockGumTraplevel20 = true;
    [SerializeField]
    public bool isLockSoulExtractorTraplevel10 = true;
    [SerializeField]
    public bool isLockSoulExtractorTraplevel20 = true;


    public Facility GetFacility(string id)
    {
        return listSample.Find(x => x.id == id).CloneFacility();
    }

    public bool isBuildMode = false;
    public bool isBuilding = false;
    

    [SerializeField]
    private Room currentRoom;

    public void SelectRoom(Room room)
    {
        currentRoom = room;
        buildingUI.ActiveUI(this.currentRoom);
    }

    public void Build(string id)
    {
        id = buildingUI.currentBuildOption;

        if (!CheckPrice(id))
            return;

        currentRoom.AddFacility(this.GetFacility(id));
        buildingUI.ActiveUI(this.currentRoom);
        listFacilityInDungeon.Add(currentRoom.facility);
        if (this.currentRoom.facility.id == "ROSY_001")
            DungeonCore.Instance.UnlockSoulConcentrated();
        if (this.currentRoom.facility.id == "ROSY_003")
        {
            this.UnlockPrisonFeature();
            UpdatePrisonCapacity();
        }
        if (this.currentRoom.facility.id == "ROSY_004")
        {
            this.UpdateCorruptionCapacity();
        }
        if (this.currentRoom.facility.id == "ROSY_005")
        {
            summonController.UnlockBanner("BANN_002");
            //this.UpdateBannerTier2();
        }
    }

    public List<Facility> GetListSample()
    {
        return listSample;
    }

    public List<Facility> GetListSoulGainAble()
    {
        List<Facility> listSouGainable = listFacilityInDungeon.FindAll(x => x.id == "ROSY_002");

        return listSouGainable;
    }

    public int GetPrisonCapacity()
    {
        int totalCapacity = 0;
        foreach (Facility facility in listFacilityInDungeon)
        {
            if (facility.id == "ROSY_003")
                totalCapacity += (int)facility.value;
        }

        return totalCapacity;
    }

    public void UnlockPrisonFeature()
    {
        prison.gameObject.SetActive(true);
    }

    public void LockPrisonFreature()
    {
        prison.gameObject.SetActive(false);
    }

    public void UpdatePrisonCapacity()
    {
        int total = 0;
        foreach (Facility facility in listFacilityInDungeon)
        {
            if (facility.id == "ROSY_003")
            {
                total += (int)facility.value;
            }
        }

        PrisonerInventory.Instance.SetCapacity(total + (int)DungeonCore.Instance.T_PrisonCapaciy);
    }

    public void UpdateCorruptionCapacity()
    {
        int total = 0;
        foreach (Facility facility in listFacilityInDungeon)
        {
            if (facility.id == "ROSY_004")
            {
                total += (int)facility.value;
            }
        }

        CorruptionController.Instance.maxCorruptor = total;
    }

    public void UpdateBannerTier2()
    {
        int total = 0;
        foreach (Facility facility in listFacilityInDungeon)
        {
            if (facility.id == "ROSY_005")
            {
                total += (int)facility.value;
            }
        }

        BannerTier1.SetRatioByLevel(total);
    }

    public void UnlockWeakTrap()
    {
        buildUI.AddBuildAbleFacility("ROSY_006");
    }

    public void UnlockGumTrap()
    {
        buildUI.AddBuildAbleFacility("ROSY_007");
    }

    public void UnlockGumTrapLevel10() => isLockGumTraplevel10 = false;
    public void UnlockGumTrapLevel20() => isLockGumTraplevel20 = false;
    public void UnlockSoulExtractorLevel10() => isLockSoulExtractorTraplevel10 = true;
    public void UnlockSoulExtractorLevel20() => isLockSoulExtractorTraplevel20 = true;
    public void UnlockGiantBuild() => buildingUI.AddBuildAbleFacility("ROSY_008");
    public void UnlockSoulSucker() => buildingUI.AddBuildAbleFacility("ROSY_009");

    private bool CheckPrice(string id)
    {
        Facility checkFacility = listSample.Find(x => x.id == id);

        PlayerCurrency playerCurrency = PlayerCurrency.Instance;
        if (playerCurrency.Soul < checkFacility.soulPrice)
            return false;
        if (playerCurrency.SoulConcentrated < checkFacility.concentratedSoulPrice)
            return false;
        if (playerCurrency.CorruptedSoul < checkFacility.corruptedSoulPrice)
            return false;

        playerCurrency.DeltaSoul(-checkFacility.soulPrice);
        playerCurrency.DeltaSoulConcentrated(-checkFacility.concentratedSoulPrice);
        playerCurrency.DeltaCorruptedSoul(-checkFacility.corruptedSoulPrice);
        return true;
    }

    public void Clear()
    {
        foreach(Facility facility in listFacilityInDungeon)
        {
            facility.address.RemoveFacility();
        }

        listFacilityInDungeon.Clear();
        this.UpdateCorruptionCapacity();
        this.UpdatePrisonCapacity();
        this.LockPrisonFreature();
    }
}
