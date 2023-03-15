using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillInformation : MonoBehaviour
{
    public GameObject skillInfor_Holder;
    public SkillDescription skillDescPrefap;
    public List<SkillDescription> listItem;
    [SerializeField]
    private VerticalLayoutGroup vert;

    public void Clear()
    {
        if (listItem == null)
        {
            listItem = new List<SkillDescription>();
            return;
        }
        foreach (SkillDescription item in listItem)
        {
            Destroy(item.gameObject);
        }

        listItem.Clear();
    }

    public void LoadSkillData(MonsterData data)
    {
        Clear();
        SkillDescription mainSkill = Instantiate<SkillDescription>(skillDescPrefap, skillInfor_Holder.transform);
        mainSkill.SetSkillDesc(data.mainSkill);
        listItem.Add(mainSkill);

        foreach(SkillData subSkill in data.subSkill)
        {
            SkillDescription newItem = Instantiate<SkillDescription>(skillDescPrefap, skillInfor_Holder.transform);
            newItem.SetSkillDesc(subSkill);
            listItem.Add(newItem);
        }

        Invoke("ReActiveLayout", 0.05f);
    }


    private void ReActiveLayout()
    {
        vert.enabled = false;
        vert.enabled = true;
    }
}
