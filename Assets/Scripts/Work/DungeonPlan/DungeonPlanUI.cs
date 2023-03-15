using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPlanUI : MonoBehaviour
{
    public List<RoomPlanUI> listRoomUI;
    [SerializeField]
    private GameObject RoomHolder;

    public void Show()
    {
        RoomHolder.SetActive(true);
        foreach (RoomPlanUI room in listRoomUI)
        {
            room.UpdateDisplay();
        }
    }

    public void Hide()
    {
        RoomHolder.SetActive(false);
    }
}
