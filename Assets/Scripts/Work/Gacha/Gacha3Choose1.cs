using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gacha3Choose1 : MonoBehaviour
{
    public event System.Action<GachaItemData> OnGachaResult = delegate { };

    [SerializeField]
    private GachaTripleInterface gacha3;
    [SerializeField]
    private bool UIController;

    [SerializeField]
    private GameObject fakeBlack;
    [SerializeField]
    private GameObject ChooseButtonHolder;

    private void Start()
    {
        gacha3.OnGachaAnimDone += ShowButtonHolder;
    }

    public void GachaItem()
    {
        gacha3.GachaItem();
        if (UIController)
        {
            fakeBlack.gameObject.SetActive(true);
        }
    }

    public void ChooseItem(int index)
    {
        switch (index)
        {
            case 1:
                this.OnGachaResult(gacha3.listItem[0]);
                break;
            case 2:
                this.OnGachaResult(gacha3.listItem[1]);
                break;
            case 3:
                this.OnGachaResult(gacha3.listItem[2]);
                break;
        }
        if (UIController)
        {
            fakeBlack.gameObject.SetActive(false);
            ChooseButtonHolder.SetActive(false);
        }

    }

    public void ShowButtonHolder()
    {
        if (UIController)
            ChooseButtonHolder.SetActive(true);
    }
}
