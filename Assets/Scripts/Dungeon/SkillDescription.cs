using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class SkillDescription : MonoBehaviour
{
    public TextMeshProUGUI skillNames;
    public TextMeshProUGUI skillDesc;


    public void SetSkillDesc(SkillData skillData)
    {
        skillNames.text = "[" + skillData.skillName + "]";
        skillDesc.text = skillData.skillDescription;
        Invoke("ReActiveLayout", 0.02f);
    }

    private void ReActiveLayout()
    {
        this.GetComponent<VerticalLayoutGroup>().enabled = false;
        this.GetComponent<VerticalLayoutGroup>().enabled = true;
    }
}
