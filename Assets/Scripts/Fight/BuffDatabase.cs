using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff Database", menuName = "CustomAsset/Buff Database", order = 1)]
public class BuffDatabase : ScriptableObject
{
    [SerializeField]
    private List<Buff> listBuff;

    public Buff GetBuffByID(string id)
    {
        return listBuff.Find(x => x.id == id).CloneBuff();
    }

    public Buff CreateBuff(string id, int istack, float value)
    {
        Buff newBuff = this.GetBuffByID(id);

        newBuff.buffStack = istack;
        newBuff.buffValue = value;

        return newBuff;
    }
}
