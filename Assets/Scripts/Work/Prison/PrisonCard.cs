using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrisonCard : MonoBehaviour
{
    public PrisonUI prisonController;
    [Header("Corruption")]
    public GameObject Corruption;
    public GameObject ReadyCorrupt;
    public GameObject Corrupting;
    public GameObject Corrupted;

    [SerializeField]
    private Text CorruptCapacity;

    [SerializeField]
    private Button fastCorruptButton;

    //Corruption
    public void CorruptChoice()
    {
        if (!this.GetComponent<MonsterCard>().data.isCorruptDone)
        {
            Corruption.gameObject.SetActive(true);
            ReadyCorrupt.SetActive(true);
            CorruptCapacity.text = "Capacity " + CorruptionController.Instance.currentCorruptor + "/" + CorruptionController.Instance.maxCorruptor;
        }
        else
        {
            CorruptionController.Instance.GetCorrupted(this.GetComponent<MonsterCard>().data);
        }
    }

    public void CloseCorruptChoice()
    {
        Corruption.gameObject.SetActive(false);
        ReadyCorrupt.SetActive(false);
    }

    public void BeginCorrupt()
    {
        if (CorruptionController.Instance.currentCorruptor == CorruptionController.Instance.maxCorruptor)
            return;
        prisonController.CorruptMonster(this.GetComponent<MonsterCard>().data);
    }

    public void ShowCorrupting()
    {
        Corruption.SetActive(true);
        Corrupting.SetActive(true);

        if (PlayerCurrency.Instance.SoulConcentrated >= this.GetComponent<MonsterCard>().data.corruptionTime)
            fastCorruptButton.gameObject.SetActive(true);
        else
            fastCorruptButton.gameObject.SetActive(false);
    }

    public void ShowCorruptDone()
    {
        Corruption.SetActive(true);
        Corrupted.SetActive(true);
    }

    public void FastCorrupt()
    {
        PlayerCurrency.Instance.DeltaSoulConcentrated(-this.GetComponent<MonsterCard>().data.corruptionTime);
        this.GetComponent<MonsterCard>().data.corruptionTime = 0;
        this.GetComponent<MonsterCard>().data.isCorrupting = false;
        this.GetComponent<MonsterCard>().data.isCorruptDone = true;
        this.ShowCorruptDone();
    }
}
