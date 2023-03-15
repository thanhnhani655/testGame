using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill Database", menuName = "CustomAsset/Skill Database", order = 1)]
public class SkillDatabase : ScriptableObject
{
    [SerializeField]
    private List<SkillData> listSkill;

    public SkillData GetSkillDataByID(string id)
    {
        return listSkill.Find(x => x.skillID == id).CloneSkill();
    }
}
