using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MobState
{
    Thinking,
    Moving,
    Combating,
    InActive
}

public class MobInDungeon : MonoBehaviour
{
    public MobState state;
    public Room currentRoom;

    //Moving
    public Room nextRoom;
    public float speed = 1;
    public float t = 0;

    private void Update()
    {
        if (state == MobState.InActive)
            return;
        switch (state)
        {
            case MobState.Thinking:
                MobThink();
                break;
            case MobState.Moving:
                break;           
        }
    }

    public void SetInActive()
    {
        state = MobState.InActive;
    }

    public void MobThink()
    {
        // Join Room
        if (currentRoom.isLockedInside)
        {
            this.GetComponent<Invader>().isJustFight = false;
            MonsterData data = this.GetComponent<Invader>().data;
            data.baseStat.timeCounter += Time.deltaTime * Parameter.Instance.SPEED;
            if (data.baseStat.timeCounter < data.baseStat.attackSpeed)
                return;
            
            data.baseStat.timeCounter -= data.baseStat.attackSpeed;
            CheckingCombat();
            return;
        }
        // Moving
        List<Room> listRoomMoveAble = currentRoom.GetAllConnectRoom();
        int index = Random.Range(0, listRoomMoveAble.Count);
        nextRoom = listRoomMoveAble[index];
        t = 0;
        state = MobState.Moving;
        StartCoroutine(Moving());
    }

    private IEnumerator Moving()
    {
        while (true)
        {
            if (this.state == MobState.InActive)
                break;
            t += Time.deltaTime * speed;

            Vector3 temp = this.transform.position;

            temp.x = Mathf.Lerp(currentRoom.transform.position.x, nextRoom.transform.position.x, t);
            temp.y = Mathf.Lerp(currentRoom.transform.position.y, nextRoom.transform.position.y, t);

            this.transform.position = temp;

            if (t >= 1)
            {
                currentRoom = nextRoom;
                //state = MobState.Thinking;
                CheckingCombat();
                break;
            }

            yield return null;
        }
    }

    private void CheckRoomDebuff()
    {
        if (currentRoom.facility == null)
            return;
        switch (currentRoom.facility.id)
        {
            case "ROSY_006":
                this.GetComponent<Invader>().data.baseStat.AddBuff(DatabaseInstanceAccess.Instance.buffDatabase.CreateBuff("BUFF_002", (int)currentRoom.facility.value, 0));
                break;
            case "ROSY_007":
                this.GetComponent<Invader>().data.baseStat.AddBuff(DatabaseInstanceAccess.Instance.buffDatabase.CreateBuff("BUFF_006", (int)currentRoom.facility.value, 0));
                break;
            case "ROSY_009":
                InvadeController.Instance.soulFromSuck += currentRoom.facility.value;
                break;
        }
    }

    private void CheckingCombat()
    {
        if (!this.GetComponent<Invader>().isJustFight)
            foreach (MonsterData data in currentRoom.ListMonInRoom)
            {
                if (data.isDead == false)
                {
                    state = MobState.Combating;
                    currentRoom.StartCombat();
                    return;
                }
            }

        state = MobState.Thinking; 
    }

    public void CombatDone()
    {
        this.GetComponent<ShakeObject>().ShakeDone -= CombatDone;
        if (state == MobState.Combating)
        {
            state = MobState.Thinking;
        }
    }
}
