using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum FlowPhase
{
    NewDay,
    Story,
    BuffPhase,
    WorkPhase,
    InvadePhase,
    EndDayPhase
}

public class GameFlowController : MonoBehaviour
{
    private static GameFlowController instance;
    public static GameFlowController Instance => instance;

    public MenuUI menuUI;

    public FlowPhase flowPhase;
    public UIController uiController;
    public DayCounter dayCounter;
    public RandomBuffDay randomBuffDay;
    public WorkUI workUI;
    public BeInvadeUI beInvadeUI;
    public InvadeController invadeController;

    public TreasurePool treasurePool;

    private void Start()
    {
        dayCounter.OnEndScene += EndNewDayPhase;
        randomBuffDay.OnEndPhase += EndBuffPhase;
        workUI.OnEndWorkPhase += EndWorkPhase;
        beInvadeUI.OnEndInvadePhase += EndBeInvadePhase;
        beInvadeUI.OnReset += GameOver;
    }

    public void StartNewgame()
    {
        menuUI.HideMenuUI();
        InitializeNewGame();
        StartNewDayPhase();
    }

    private void InitializeNewGame()
    {
        BonusNewgame bonus = BonusNewgame.Instance;
        dayCounter.Day = 0;
        WaveGenerator.Instance.Initialize();

        PlayerCurrency.Instance.SetSoul(100 + bonus.StartWithMoreSoul);
        PlayerCurrency.Instance.SetCorruptedSoul(0);
        PlayerCurrency.Instance.SetSoulConcentrated(0);

        MonsterInventory.Instance.SetStarterInventory();
        PrisonerInventory.Instance.Clear();
        TreasureInventory.Instance.Clear();
        treasurePool.UpdateFromDatabase();          //Reset Treasure Pool
        CorruptionController.Instance.Clear();
        BuildingController.Instance.Clear();
        DungeonCore.Instance.ResetLevel();
        for (int i = 1; i < bonus.StartWithDungeonCoreLevel; i++)
            DungeonCore.Instance.LevelUp(true);

        for (int i = 0; i < bonus.StartWith1RandomTreasureTier1; i++)
        {
            invadeController.invadeRank = InvaderRank.Tier2;
            invadeController.GetTreasure();
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F1))
        {
            StartNewDayPhase();
        }
    }

    [Button]
    public void StartNewDayPhase()
    {
        dayCounter.NextDay();
    }

    public void EndNewDayPhase()
    {
        RunStoryPhase();
    }

    public void RunStoryPhase()
    {
        //Still Not
        EndRunStoryPhase();
    }

    public void EndRunStoryPhase()
    {
        RunBuffPhase();
    }

    public void RunBuffPhase()
    {
        randomBuffDay.RandomBuff();
    }

    public void EndBuffPhase()
    {
        BeginWorkPhase();
    }

    public void BeginWorkPhase()
    {
        DungeonCore.Instance.GetLegionBasicBonus();
        workUI.ShowWorkUI_Begin();
    }

    public void EndWorkPhase()
    {
        DungeonCore.Instance.GetLegionBasicBonus();
        BeginBeInvadePhase();
    }

    public void BeginBeInvadePhase()
    {
        beInvadeUI.ShowBeInvadeUI_Begin();
        invadeController.Initialize();
    }

    public void EndBeInvadePhase()
    {
        StartNewDayPhase();
    }

    public void GameOver()
    {
        menuUI.ShowMenuUI();

    }
}
