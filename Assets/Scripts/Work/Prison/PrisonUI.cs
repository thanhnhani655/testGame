using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonUI : MonoBehaviour
{
    [SerializeField]
    private CorruptionController corruptionController;
    [SerializeField]
    private GameObject ZoomArea;
    [SerializeField]
    private MonsterCard zoomedCard;
    [SerializeField]
    private MonsterCard miniCardPrefap;
    [SerializeField]
    private Transform CardArea;

    [SerializeField]
    private List<MonsterCard> listCards;
    [SerializeField]
    private PrisonerInventory listPrisoner;

    private void Start()
    {
        if (listCards == null)
            listCards = new List<MonsterCard>();
    }

    public void OnEnable()
    {
        this.ActiveScene();
    }

    public void OnDisable()
    {
        this.DeactiveScene();
    }

    public void ActiveScene()
    {
        LoadInventory();
    }

    public void DeactiveScene()
    {
    }

    public void LoadInventory()
    {
        Debug.Log("Load Inventory");
        this.ClearCard();
        List<MonsterData> listDatas = listPrisoner.GetListData();

        foreach (MonsterData data in listDatas)
        {
            if (data.address != ItemAddress.Prison)
                continue;
            MonsterCard newCard = Instantiate<MonsterCard>(miniCardPrefap, CardArea);
            newCard.SetMonster(data);
            newCard.prisonerManager = this;
            newCard.GetComponent<PrisonCard>().prisonController = this;
            if (data.isCorrupting)
            {
                newCard.GetComponent<PrisonCard>().ShowCorrupting();
                newCard.interactable = false;
            }
            else if (data.isCorruptDone)
            {
                newCard.GetComponent<PrisonCard>().ShowCorruptDone();
                newCard.interactable = true;
            }
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

    #region Function Zoom
    public void Zoom(MonsterData data)
    {
        ZoomArea.SetActive(true);
        zoomedCard.SetMonster(data);
        StartCoroutine(ZoomWaitingEvent());
    }

    private IEnumerator ZoomWaitingEvent()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Mouse Down");
                ZoomArea.SetActive(false);
                break;
            }
            yield return null;
        }
    }
    #endregion

    public void CorruptMonster(MonsterData data)
    {
        corruptionController.AddCorruptor(data);
        LoadInventory();
    }
}
