using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LegionStatUI : MonoBehaviour
{
    [SerializeField]
    private Text atkFlat;
    [SerializeField]
    private Text hpFlat;
    [SerializeField]
    private Text critFlat;
    [SerializeField]
    private Text critdmgFlat;

    [SerializeField]
    private DungeonCore core;

    public void ActiveScene()
    {
        this.LoadLegionStat(Legion.Red);
        this.gameObject.SetActive(true);
    }

    public void DeActiveScene()
    {
        this.gameObject.SetActive(false);
    }

    public void LoadLegionStat(Legion legion)
    {
        LegionCore leCore = core.listLegionCore.Find(x => x.legionID == legion);
        atkFlat.text = leCore.attackFlatBonus.ToString();
        hpFlat.text = leCore.hpFlatBonus.ToString();

    }

    public void LoadLegionStat(int legion)
    {
        switch (legion)
        {
            case 1:
                this.LoadLegionStat(Legion.Red);
                break;
            case 2:
                this.LoadLegionStat(Legion.Blue);
                break;
            case 3:
                this.LoadLegionStat(Legion.Black);
                break;
        }
    }
}
