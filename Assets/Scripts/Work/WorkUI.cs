using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkUI : MonoBehaviour
{
    public event System.Action OnEndWorkPhase;
    [SerializeField]
    private FadeInFadeOut fader;
    [SerializeField]
    private GameObject fakeBlack;

    [SerializeField]
    private List<ScenePlug> listScene;

    [SerializeField]
    public int actionTime = 3;

    public void ActiveScene(ScenePlug iscene)
    {
        foreach (ScenePlug scene in listScene)
        {
            if (scene == iscene)
                scene.ActiveScene();
            else
                scene.DeActiveScene();
        }
    }

    public void ShowWorkUI_Begin()
    {
        this.gameObject.SetActive(true);
        fakeBlack.gameObject.SetActive(true);
        fader.FadeOut();
        fader.OnFadeOutDone += ShowWorkUI_End;
    }

    public void ShowWorkUI_End()
    {
        fader.OnFadeOutDone -= ShowWorkUI_End;
        fakeBlack.gameObject.SetActive(false);
    }

    public void CloseWorkUI_Begin()
    {
        fakeBlack.gameObject.SetActive(true);
        fader.FadeIn();
        fader.OnFadeInDone += CloseWorkUI_End;
    }

    public void CloseWorkUI_End()
    {
        fader.OnFadeInDone -= CloseWorkUI_End;
        OnEndWorkPhase();
        ActiveScene(listScene[0]);
        this.gameObject.SetActive(false);
    }

   
}
