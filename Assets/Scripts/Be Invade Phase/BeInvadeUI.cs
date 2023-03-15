using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeInvadeUI : MonoBehaviour
{
    private static BeInvadeUI instance;
    public static BeInvadeUI Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public event System.Action OnEndInvadePhase;
    public event System.Action OnReset;
    public FadeInFadeOut fader0;
    public FadeInFadeOut fader1;
    public FadeInFadeOut fader2;
    public FadeInFadeOut fader3;
    public FadeInFadeOut defeatUI;
    public Text SoulGain;
    public Text SoulStoneGain;
    [SerializeField]
    private GameObject Dungeon;
    [SerializeField]
    private InvadeController invadeController;
    [SerializeField]
    public RoomInformation roomInformation;
    [SerializeField]
    private TreasureCard treasureCard;

    private void Start()
    {
        invadeController.OnVictory += OnVictory_1;
        invadeController.OnDefeat += OnDefeat_1;
    }

    public void ShowBeInvadeUI_Begin()
    {
        Debug.Log("ShowBeInvadeUI_Begin");
        treasureCard.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
        fader0.gameObject.SetActive(true);
        fader1.gameObject.SetActive(true);
        fader2.gameObject.SetActive(true);

        fader0.FadeOut();
        fader0.OnFadeOutDone += ShowBeInvadeUI_1;
    }

    public void ShowBeInvadeUI_1()
    {
        Debug.Log("ShowBeInvadeUI_1");
        fader0.OnFadeOutDone -= ShowBeInvadeUI_1;
        fader1.FadeOut();
        fader1.OnFadeOutDone += ShowBeInvadeUI_2;
    }
    public void ShowBeInvadeUI_2()
    {
        Debug.Log("ShowBeInvadeUI_2");
        fader1.OnFadeOutDone -= ShowBeInvadeUI_2;
        fader2.FadeOut();
        fader2.OnFadeOutDone += ShowBeInvadeUI_3;
    }
    public void ShowBeInvadeUI_3()
    {
        Debug.Log("ShowBeInvadeUI_3");
        fader2.OnFadeOutDone -= ShowBeInvadeUI_3;
        fader0.FadeIn();
        fader1.FadeIn();
        fader2.FadeIn();
        fader0.OnFadeInDone += ShowBeInvadeUI_3_1;
        fader1.OnFadeInDone += ShowBeInvadeUI_3_2;
        fader2.OnFadeInDone += ShowBeInvadeUI_3_3;
    }

    #region miniprocess
    bool fader0FadedIn = false;
    bool fader1FadedIn = false;
    bool fader2FadedIn = false;
    public void ShowBeInvadeUI_3_1()
    {
        fader0.OnFadeInDone -= ShowBeInvadeUI_3_1;
        fader0FadedIn = true;
        ShowBeInvadeUI_3_4();
    }
    public void ShowBeInvadeUI_3_2()
    {
        fader1.OnFadeInDone -= ShowBeInvadeUI_3_2;
        fader1FadedIn = true;
        ShowBeInvadeUI_3_4();
    }
    public void ShowBeInvadeUI_3_3()
    {
        fader2.OnFadeInDone -= ShowBeInvadeUI_3_3;
        fader2FadedIn = true;
        ShowBeInvadeUI_3_4();
    }

    public void ShowBeInvadeUI_3_4()
    {
        if (fader0FadedIn &&
            fader1FadedIn &&
            fader2FadedIn)
        {
            fader0FadedIn = false;
            fader1FadedIn = false;
            fader2FadedIn = false;
            ShowBeInvadeUI_4();
        }
    }
    #endregion

    public void ShowBeInvadeUI_4()
    {
        Debug.Log("ShowBeInvadeUI_4");
        fader2.OnFadeInDone -= ShowBeInvadeUI_4;
        fader1.gameObject.SetActive(false);
        fader2.gameObject.SetActive(false);

        fader0.FadeOut();

        Dungeon.gameObject.SetActive(true);
        fader0.OnFadeOutDone += ShowBeInvadeUI_5;
    }

    public void ShowBeInvadeUI_5()
    {
        Debug.Log("ShowBeInvadeUI_5");
        fader0.OnFadeOutDone -= ShowBeInvadeUI_5;
        fader0.gameObject.SetActive(false);
        invadeController.InvadeIn1Second();
    }

    public void OnVictory_1()
    {
        LoadTreasure();
        fader0.gameObject.SetActive(true);
        SoulGain.text = "Soul Gains: " + invadeController.VictorySoul.ToString();
        fader0.FadeIn();
        fader0.OnFadeInDone += OnVictory_2;
        roomInformation.Deactive();
    }

    public void OnVictory_2()
    {
        fader0.OnFadeInDone -= OnVictory_2;
        Dungeon.gameObject.SetActive(false);
        fader0.gameObject.SetActive(false);
        fader3.gameObject.SetActive(true);
        fader3.FadeOut();
        fader3.OnFadeOutDone += OnVictory_3;
    }

    public void OnVictory_3()
    {
        fader3.OnFadeOutDone -= OnVictory_3;
        fader3.mainImage.gameObject.SetActive(false);
        //StartCoroutine(WaitToEndPhase());
    }

    //private IEnumerator WaitToEndPhase ()
    //{
    //    while (true)
    //    {
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            OnVictory_4();
    //            break;
    //        }
    //        yield return new WaitForSeconds(5f);
    //        OnVictory_4();
    //        break;
    //    }
    //}

    public void EndPhase()
    {
        OnVictory_4();
        fader3.mainImage.gameObject.SetActive(true);
    }

    public void OnVictory_4()
    {
        fader3.FadeIn();
        fader3.OnFadeInDone += OnEndPhase;
    }
    
    public void OnEndPhase()
    {        
        fader3.OnFadeInDone -= OnEndPhase;
        fader3.gameObject.SetActive(false);
        OnEndInvadePhase();
        this.gameObject.SetActive(false);
    }

    public void LoadTreasure()
    {
        if (invadeController.newTreasure == null)
            return;
        treasureCard.gameObject.SetActive(true);
        treasureCard.LoadData(invadeController.newTreasure);
    }

    public void OnDefeat_1()
    {
        fader0.gameObject.SetActive(true);
        fader0.FadeIn();
        SoulStoneGain.text = "Soul Stone Gain: " + invadeController.soulStoneBySurvival.ToString();
        fader0.OnFadeInDone += OnDefeat_2;
        roomInformation.Deactive();
    }

    public void OnDefeat_2()
    {
        fader0.OnFadeInDone -= OnDefeat_2;
        defeatUI.gameObject.SetActive(true);
        Dungeon.gameObject.SetActive(false);
        defeatUI.FadeOut();
        defeatUI.OnFadeOutDone += OnDefeat_3;
    }

    public void OnDefeat_3()
    {
        defeatUI.OnFadeOutDone -= OnDefeat_3;
        defeatUI.mainImage.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        OnDefeat_4();
        defeatUI.mainImage.gameObject.SetActive(true);
    }

    public void OnDefeat_4()
    {
        defeatUI.FadeIn();
        defeatUI.OnFadeInDone += OnGameOver;
    }

    public void OnGameOver()
    {
        defeatUI.OnFadeInDone -= OnGameOver;
        defeatUI.gameObject.SetActive(false);
        OnReset();
        this.gameObject.SetActive(false);
    }
}
