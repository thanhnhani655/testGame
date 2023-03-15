using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class RoomManager : MonoBehaviour
{
    public List<Room> listRoomInDungeon;

    private void Start()
    {
        Invoke("AutoConnectRoom", 0.1f);
    }

    [Button]
    public void AutoConnectRoom()
    {
        foreach (Room room in listRoomInDungeon)
        {
            room.ClearConnection();
            Vector2 roomPos = new Vector2(int.Parse(room.RoomID.Substring(0, 1)), int.Parse(room.RoomID.Substring(1, 1)));
             
            //Check X
            Room rRoom = listRoomInDungeon.Find(x=> (int.Parse(x.RoomID.Substring(0,1)) == roomPos.x - 1) && (int.Parse(x.RoomID.Substring(1, 1)) == roomPos.y));
            if (rRoom != null)
                room.AddConnection(rRoom);

            Room lRoom = listRoomInDungeon.Find(x => (int.Parse(x.RoomID.Substring(0, 1)) == roomPos.x + 1) && (int.Parse(x.RoomID.Substring(1, 1)) == roomPos.y));
            if (lRoom != null)
                room.AddConnection(lRoom);

            Room aRoom = listRoomInDungeon.Find(x => (int.Parse(x.RoomID.Substring(0, 1)) == roomPos.x) && (int.Parse(x.RoomID.Substring(1, 1)) == roomPos.y - 1));
            if (aRoom != null)
                room.AddConnection(aRoom);

            Room bRoom = listRoomInDungeon.Find(x => (int.Parse(x.RoomID.Substring(0, 1)) == roomPos.x) && (int.Parse(x.RoomID.Substring(1, 1)) == roomPos.y + 1));
            if (bRoom != null)
                room.AddConnection(bRoom);
        }
    }

    public void UnlockInsideAll()
    {
        foreach (Room room in listRoomInDungeon)
        {
            room.UnlockInside();
        }
    }

    public Room GetRoomById(string id)
    {
        return listRoomInDungeon.Find(x => x.RoomID == id);
    }

    public void ClearMonster()
    {
        foreach (Room room in listRoomInDungeon)
        {
            room.RemoveAllMonster();
        }
    }
}
