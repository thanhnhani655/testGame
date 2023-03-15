using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffDayUIController : MonoBehaviour
{
    [SerializeField]
    private RandomBuffDay randomBuffDay;
    [SerializeField]
    private FadeInFadeOut fader;

    [SerializeField]
    private List<Button> listBuffCard;

    [SerializeField]
    private Image blackFake;

    public void ShowBuffDayUI_Begin()
    {
        this.gameObject.SetActive(true);
        fader.FadeOut();
        fader.OnFadeOutDone += ShowBuffDayUI_Finish;
        blackFake.gameObject.SetActive(false);
        foreach (Button card in listBuffCard)
        {
            card.GetComponent<FadeInFadeOut>().FadeOut();
            card.interactable = false;
        }
    }

    public void ShowBuffDayUI_Finish()
    {
        fader.OnFadeOutDone -= ShowBuffDayUI_Finish;
        foreach (Button card in listBuffCard)
        {
            card.interactable = true;
        }
    }

    public void BuffChoosen(int index)
    {
        for (int i = 0; i < listBuffCard.Count; i++)
        {
            listBuffCard[i].interactable = false;
            if (index == i)
                continue;
            listBuffCard[i].GetComponent<FadeInFadeOut>().FadeIn();
        }
        blackFake.gameObject.SetActive(true);
        StartCoroutine(WaitToNextPhase());
    }

    private IEnumerator WaitToNextPhase()
    {
        while (true)
        {
            //if (Input.GetMouseButtonUp(0))
            //{
            //    FadeOut_Begin();
            //    break;
            //}
            
            yield return new WaitForSeconds(Parameter.Instance.BuffDayChangeSceneSpeed * Parameter.Instance.SceneChangeSpeed);
            FadeOut_Begin();
            break;
        }
    }

    private void FadeOut_Begin()
    {
        fader.FadeIn();
        fader.OnFadeInDone += FadeOut_Finish;
    }

    private void FadeOut_Finish()
    {
        fader.OnFadeInDone -= FadeOut_Finish;
        randomBuffDay.EndPhase();
        this.gameObject.SetActive(false);
        
    }
}
