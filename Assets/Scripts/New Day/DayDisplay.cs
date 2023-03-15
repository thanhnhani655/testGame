using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayDisplay : MonoBehaviour
{
    public DayCounter dayCounter;
    public Text dayText;
    public FadeInFadeOut fader;
    public GameObject newDayScene;
    public Button EndSceneButton;

    public float timeWaitToEndScene = 3;

    public void NextDay()
    {
        newDayScene.SetActive(true);
        dayText.text = "DAY " + dayCounter.Day.ToString();
        fader.FadeOut();
        Debug.Log("Start Fade Out");
        fader.OnFadeOutDone += WaitToEnd;
    }

    public void WaitToEnd()
    {
        Debug.Log("Fade Out End");
        fader.OnFadeOutDone -= WaitToEnd;
        EndSceneButton.interactable = true;
        fader.mainImage.gameObject.SetActive(false);
        //StartCoroutine(WaitForEndScene());
    }

    IEnumerator WaitForEndScene()
    {
        while(true)
        {
            if (Input.GetMouseButtonUp(0))
            {
                EndSceneStage1();
                break;
            }
            yield return new WaitForSeconds(timeWaitToEndScene * Parameter.Instance.SceneChangeSpeed);
            EndSceneStage1();
            break;
        }
    }

    public void EndScene()
    {
        EndSceneStage1();
        EndSceneButton.interactable = false;
    }

    public void EndSceneStage1()
    {
        Debug.Log("End Scene");
        fader.FadeIn();
        fader.OnFadeInDone += EndSceneStage2;
        fader.mainImage.gameObject.SetActive(true);
    }

    public void EndSceneStage2()
    {
        fader.OnFadeInDone -= EndSceneStage2;
        dayCounter.EndScene();
        newDayScene.SetActive(false);
    }
}
