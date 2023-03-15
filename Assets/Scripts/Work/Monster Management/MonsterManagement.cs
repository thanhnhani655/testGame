using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterManagement : MonoBehaviour
{   
    private void OnEnable()
    {
        ActiveScene();
    }

    private void OnDisable()
    {
        
    }

    public void ActiveScene()
    {
        ShowTributeObject();
        LoadInventory();
    }

    

    public void DeactiveScene()
    {

    }

    [SerializeField]
    private MonsterCard MMCardPrefaps;
    [SerializeField]
    private MonsterInventory inventory;
    [SerializeField]
    private List<MonsterCard> listCards;
    [SerializeField]
    private GameObject CardArea;
    [SerializeField]
    private MonsterCard selectedCard;

    //Tribute
    [SerializeField]
    private GameObject tributeTransform;
    [SerializeField]
    private Outline tributeOutline;
    [SerializeField]
    private Button LevelUPButton;
    [SerializeField]
    private Text soulNeedText;

    //Sacrifice
    [SerializeField]
    private GameObject sacrificeTransform;
    [SerializeField]
    private Outline sacrificeOutline;
    [SerializeField]
    private Button SacrificeButton;
    [SerializeField]
    private Text soulGainText;


    #region Initialize
    private void Start()
    {
        if (listCards == null)
            listCards = new List<MonsterCard>();
    }
    
    //0: Load For Leveling      1: Load For Sacrifice
    public void LoadInventory(int type = 0)
    {
        Debug.Log("Load Inventory");
        this.ClearCard();
        inventory.SortInventory();
        List<MonsterData> listDatas = inventory.GetListData();
        
        foreach (MonsterData data in listDatas)
        {
            if (type == 0)
                if (data.rank != MonsterRank.Boss)
                    continue;
            if (data.address == ItemAddress.Dungeon && data.rank != MonsterRank.Boss)
                continue;
            MonsterCard newCard = Instantiate<MonsterCard>(MMCardPrefaps, CardArea.transform);
            newCard.SetMonster(data);
            newCard.GetComponent<MonsterManagementInterationCard>().manager = this;
            listCards.Add(newCard);
            Debug.Log(data.monName + " " + data.address.ToString());
        }
    }

    public void ClearCard()
    {
        foreach (MonsterCard card in listCards)
        {
            Destroy(card.gameObject);
        }

        listCards.Clear();
    }
    #endregion

    public void ShowTributeObject()
    {
        tributeTransform.SetActive(true);
        sacrificeTransform.SetActive(false);

        selectedCard.gameObject.SetActive(false);
        LevelUPButton.gameObject.SetActive(false);
        soulNeedText.gameObject.SetActive(false);
        soulGainText.gameObject.SetActive(false);
        SacrificeButton.gameObject.SetActive(false);

        tributeOutline.enabled = true;
        sacrificeOutline.enabled = false;
        LoadInventory(0);
    }

    public void ShowSacrificeObject()
    {
        tributeTransform.SetActive(false);
        sacrificeTransform.SetActive(true);

        selectedCard.gameObject.SetActive(false);
        LevelUPButton.gameObject.SetActive(false);
        soulNeedText.gameObject.SetActive(false);
        soulGainText.gameObject.SetActive(false);
        SacrificeButton.gameObject.SetActive(false);

        tributeOutline.enabled = false;
        sacrificeOutline.enabled = true;
        LoadInventory(1);
    }

    public void SelectCard(MonsterData data)
    {
        selectedCard.SetMonster(data);
        if (data.bossSkillPoint > 0)
            selectedCard.GetComponent<MonsterManagementInterationCard>().ShowAddSkillButton();
        else
            selectedCard.GetComponent<MonsterManagementInterationCard>().HideAddSkillButton();

        selectedCard.gameObject.SetActive(true);
        soulNeedText.gameObject.SetActive(true);
        LevelUPButton.gameObject.SetActive(true);
        soulGainText.gameObject.SetActive(true);
        SacrificeButton.gameObject.SetActive(true);

        float soulNeed = data.stat.level.ExpNeedToLevelUp;
        soulNeedText.text = ((int)soulNeed).ToString() + " Soul";
        if (!data.isCorrupted)
        {
            if (data.rank == MonsterRank.Minion)
            {
                soulGainText.text = "Gain 1 Concentrated Soul";
                soulGainText.color = Color.cyan;
            }
            else if (data.rank == MonsterRank.Boss)
            {
                soulGainText.text = "Gain 5 Concentrated Soul";
                soulGainText.color = Color.cyan;
            }
        }
        else
        {
            soulGainText.text = "Gain " + (data.stat.Level * 10f).ToString() + " Corrupted Soul";
            soulGainText.color = Color.red;
        }

        if (soulNeed > PlayerCurrency.Instance.Soul)
        {
            soulNeedText.color = Color.red;
            LevelUPButton.interactable = false;
        }
        else
        {
            soulNeedText.color = Color.green;
            LevelUPButton.interactable = true;
        }
    }

    public void LevelUp()
    {
        if (!selectedCard.LevelUp())
            return;
        PlayerCurrency.Instance.DeltaSoul(-selectedCard.data.stat.level.ExpNeedToLevelUp);
        
        this.SelectCard(this.selectedCard.data);
        LoadInventory();
    }

    public void Sacrifice()
    {
        if (!selectedCard.data.isCorrupted)
        {
            if (selectedCard.data.rank == MonsterRank.Boss)
            {
                PlayerCurrency.Instance.DeltaSoulConcentrated(5);
            }
            else
            {
                PlayerCurrency.Instance.DeltaSoulConcentrated(1);
            }
        }
        else
            PlayerCurrency.Instance.DeltaCorruptedSoul(selectedCard.data.stat.Level * 10 );

        MonsterInventory.Instance.RemoveMonster(selectedCard.data);

        selectedCard.gameObject.SetActive(false);
        soulGainText.gameObject.SetActive(false);
        SacrificeButton.gameObject.SetActive(false);

        LoadInventory(1);
    }

}
