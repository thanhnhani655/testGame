using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonCoreUI : MonoBehaviour
{
    [SerializeField]
    private DungeonCore core;
    public Text CoreLevel;
    public Text monsterLevel;
    public Text monsterHp;
    public Text monsterAtk;
    public Text monsterCriticalRate;
    public Text monsterCriticalDamage;
    public Text soulGain;
    public Text pointRemain;

    public Text soulNeedToLevelUp;

    public GameObject ButtonHolder;
    public GameObject LevelUpButton;

    public GameObject CreateSoulConcentrated;
    public LegionStatUI legionStatUI;
    public GameObject coreStatUI;

    private void OnEnable()
    {
        SceneActive();
    }

    private void OnDisable()
    {
        SceneDeactive();
    }

    public void SceneActive()
    {
        ActiveCoreStatUI();
        core.GetLegionBasicBonus();
        UpdateDungeonCoreUI();
    }

    public void SceneDeactive()
    {

    }
    
    public void CheckPointRemain()
    {
        if (core.pointRemain > 0)
        {
            ButtonHolder.SetActive(true);
        }
        else
        {
            ButtonHolder.SetActive(false);
        }
    }

    public void CheckSoulLevelUp()
    {
        soulNeedToLevelUp.text = core.GetSoulNeedToLevelUp().ToString();
        Debug.Log(core.GetSoulNeedToLevelUp());
        if (core.GetSoulNeedToLevelUp() > PlayerCurrency.Instance.Soul)
        {
            LevelUpButton.SetActive(false);
        }
        else
        {
            LevelUpButton.SetActive(true);
        }
    }

    public void UpdateCoreLevel()
    {
        CoreLevel.text = "Core Level " + core.coreLevel;
    }

    public void UpdateStat()
    {
        monsterLevel.text = core.monsterlevel.ToString();
        monsterHp.text = core.Hp.ToString() + "%"; 
        monsterAtk.text = core.Atk.ToString() + "%";
        monsterCriticalRate.text = core.CriticalRate.ToString() + "%";
        monsterCriticalDamage.text = core.criticalDmg.ToString() + "%";
        soulGain.text = core.SoulGain.ToString() + "%";
        pointRemain.text = core.pointRemain.ToString() + "%";
    }

    public void UpdateDungeonCoreUI()
    {
        UpdateCoreLevel();
        UpdateStat();
        CheckPointRemain();
        CheckSoulLevelUp();
    }

    public void UnlockSoulConcentrated()
    {
        CreateSoulConcentrated.SetActive(true);
    }

    public void ActiveCoreStatUI()
    {
        legionStatUI.DeActiveScene();
        coreStatUI.gameObject.SetActive(true);
    }

    public void ActiveLegionStatUI()
    {
        legionStatUI.ActiveScene();
        coreStatUI.gameObject.SetActive(false);
    }
}
