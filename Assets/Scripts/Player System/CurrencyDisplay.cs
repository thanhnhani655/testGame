using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyDisplay : MonoBehaviour
{
    [SerializeField]
    private Text soulValue;
    [SerializeField]
    private Text coSoulValue;
    [SerializeField]
    private Text conSoulValue;
    [SerializeField]
    private Text soulStoneValue;
    private void Start()
    {
        PlayerCurrency.Instance.SoulChange += UpdateSoul;
        PlayerCurrency.Instance.CorruptedSoulChange += UpdateCorruptSoul;
        PlayerCurrency.Instance.SoulConcentratedChange += UpdateSoulConcentrated;
        PlayerCurrency.Instance.SoulStoneChange += UpdateSoulStone;
    }

    private void UpdateSoul()
    {
        if (soulValue != null)
            soulValue.text = ((int)PlayerCurrency.Instance.Soul).ToString();
    }

    private void UpdateCorruptSoul()
    {
        if (coSoulValue != null)
            coSoulValue.text = ((int)PlayerCurrency.Instance.CorruptedSoul).ToString();
    }

    private void UpdateSoulConcentrated()
    {
        if (conSoulValue != null)
            conSoulValue.text = ((int)PlayerCurrency.Instance.SoulConcentrated).ToString();
    }

    private void UpdateSoulStone()
    {
        if (soulStoneValue != null)
            soulStoneValue.text = ((int)PlayerCurrency.Instance.SoulStone).ToString();
    }
}
