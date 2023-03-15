using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class BuildingUI : MonoBehaviour
{
    public GameObject dungeon;
    public GameObject EmptyRoom;
    public GameObject ChooseBuilding;
    public GameObject DeactiveButton;

    public GameObject FacilitySetting;
    public BuildingController buildingController;

    public FacilityCard card;
    public BuildCard buildCard;

    public List<Facility> listBuildable;
    public int currentIndex = 0;
    public string currentBuildOption;

    private void OnEnable()
    {
        ActiveScene();
    }

    private void OnDisable()
    {
        DeActiveScene();
    }

    public void ActiveScene()
    {
        dungeon.SetActive(true);
        buildingController.isBuildMode = true;
        
    }

    public void DeActiveScene()
    {
        dungeon.SetActive(false);
        buildingController.isBuildMode = false;
    }

    public void ActiveUI(Room room)
    {
        DeactiveUI();
        DeactiveButton.SetActive(true);
        buildingController.isBuilding = true;
        if (room.facility == null)
        {
            EmptyRoom.SetActive(true);
            return;
        }
        else if (!buildingController.GetListSample().Exists(x => x.id == room.facility.id))
        {
            EmptyRoom.SetActive(true);
            return;
        }
        FacilitySetting.SetActive(true);
        card.LoadFacility(room.facility);
    }

    public void DeactiveUI()
    {
        DeactiveButton.SetActive(false);
        buildingController.isBuilding = false;
        ChooseBuilding.SetActive(false);
        FacilitySetting.SetActive(false);
        EmptyRoom.SetActive(false);
    }

    public void ChooseSymbol()
    {
        EmptyRoom.SetActive(false);
        ChooseBuilding.SetActive(true);
        currentIndex = 0;
        LoadSymbol(currentIndex);
    }

    public void LoadSymbol(int index)
    {
        buildCard.Load(listBuildable[index]);
        currentBuildOption = buildCard.facility.id;
    }

    public void Next()
    {
        currentIndex++;
        if (currentIndex >= listBuildable.Count)
            currentIndex = 0;
        LoadSymbol(currentIndex);
    }

    public void Back()
    {
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = listBuildable.Count - 1;
        LoadSymbol(currentIndex);
    }

    [Button]
    public void AddBuildAbleFacility(string id)
    {
        listBuildable.Add(buildingController.GetFacility(id));
    }

}
