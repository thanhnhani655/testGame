using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class DataInformationCard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI monsterName;
    [SerializeField]
    private TextMeshProUGUI level;
    [SerializeField]
    private Image hpBar;
    [SerializeField]
    private TextMeshProUGUI atk;
    [SerializeField]
    private MonsterData data;

    public void LoadInformation(MonsterData idata)
    {
        data = idata;
        monsterName.text = idata.monName;
        level.text = "Lv " + idata.stat.Level.ToString();
        hpBar.fillAmount = idata.baseStat.currentHP / idata.baseStat.maxHP;
        idata.CaptureStat();
        atk.text = "ATK: " + idata.captureStat.atk;
    }

    public MonsterData GetData => data;
}
