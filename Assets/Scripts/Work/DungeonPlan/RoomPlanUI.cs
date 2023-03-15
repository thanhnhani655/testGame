using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class RoomPlanUI : MonoBehaviour
{
    [SerializeField]
    private DungeonManagementUI dungeonManager;
    public Room room;
    public TextMeshProUGUI monsterCount; 
    public Image RoomSealImage;


    public void UpdateDisplay()
    {
        if (room.ListMonInRoom.Count > 0)
        {
            monsterCount.gameObject.SetActive(true);
            monsterCount.text = room.ListMonInRoom.Count.ToString();
        }
        else
        {
            monsterCount.gameObject.SetActive(false);
        }
    }

    public void InteractClick()
    {
        dungeonManager.ActiveRoomSettingV2(this.room);

    }
}
