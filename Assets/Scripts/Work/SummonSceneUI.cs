using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonSceneUI : MonoBehaviour
{
    private void OnEnable()
    {
        ActiveScene();
    }

    [SerializeField]
    private List<GachaBanner> listBanner;

    [SerializeField]
    private List<GachaBanner> listAvailableBanner;
    [SerializeField]
    private int index = 0;
    [SerializeField]
    private Button Next;
    [SerializeField]
    private Button Back;

    private void OnDisable()
    {
        if (!this.gameObject.activeSelf)
            return;
        DeActiveScene();
    }
    

    public void ActiveScene()
    {
        Debug.Log("Summon Scene Actived!");
        Next.gameObject.SetActive(true);
        Back.gameObject.SetActive(true);
        ShowBanner(0);
    }

    public void DeActiveScene()
    {
        Debug.Log("Summon Scene Deactived!");
    }

    public void ShowBanner(int iindex)
    {
        listAvailableBanner[index].gameObject.SetActive(false);
        index = iindex;
        listAvailableBanner[index].gameObject.SetActive(true);

        if (index == 0)
            Back.gameObject.SetActive(false);
        if (index == listAvailableBanner.Count - 1)
        {
            Next.gameObject.SetActive(false);
        }
    }

    public void NextBanner()
    {
        ShowBanner(index + 1);
    }

    public void BackBanner()
    {
        ShowBanner(index - 1);
    }

    public void UnlockBanner(string ID)
    {
        listAvailableBanner.Add(listBanner.Find(x => x.id == ID));
    }
}
