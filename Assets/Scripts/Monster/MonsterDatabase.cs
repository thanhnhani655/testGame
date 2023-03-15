using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Monster Database", menuName = "CustomAsset/Monster Database")]
public class MonsterDatabase : ScriptableObject
{
    [SerializeField]
    private SkillDatabase skillDatabase;
    [SerializeField]
    private List<MonsterData> listDatas;

    public MonsterData GetDataByID(string id)
    {
        return listDatas.Find(x=>x.id == id).CloneMon();
    }

    public List<MonsterData> GetListDatas()
    {
        return listDatas;
    }

    [Button]
    public void AddSkill(string id, string skillId)
    {
        MonsterData data = listDatas.Find(x => x.id == id);
        data.mainSkill = skillDatabase.GetSkillDataByID(skillId);
    }

    [Button]
    public void AddSubSkill(string id, string skillId)
    {
        MonsterData data = listDatas.Find(x => x.id == id);
        data.subSkill.Add(skillDatabase.GetSkillDataByID(skillId));
    }
}
