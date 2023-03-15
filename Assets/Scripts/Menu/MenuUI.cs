using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject GetBonusMenu;
    public void ShowMenuUI()
    {
        this.gameObject.SetActive(true);
        ShowMainMenu();
    }

    public void HideMenuUI()
    {
        this.gameObject.SetActive(false);
    }

    public void ShowGetBonusMenu()
    {
        HideMainMenu();
        GetBonusMenu.SetActive(true);
    }

    public void BackToMainMenu()
    {
        GetBonusMenu.SetActive(false);
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenu.gameObject.SetActive(true);
    }

    public void HideMainMenu()
    {
        mainMenu.gameObject.SetActive(false);
    }
}
