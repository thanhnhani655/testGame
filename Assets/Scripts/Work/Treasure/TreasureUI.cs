using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureUI : MonoBehaviour
{
    [SerializeField]
    private TreasureInventory inventory;
    [SerializeField]
    private GameObject cardArea;
    [SerializeField]
    private TreasureCard prefap;
    [SerializeField]
    private List<TreasureCard> listTreasureCard;

    private void OnEnable()
    {
        ActiveScene();
    }

    private void OnDisable()
    {
        DeactiveScene();
    }

    public void ActiveScene()
    {
        Loading();
    }

    public void DeactiveScene()
    {

    }

    public void Loading()
    {
        Clear();

        foreach (Treasure data in inventory.GetListData())
        {
            TreasureCard newCard = Instantiate<TreasureCard>(prefap, cardArea.transform);
            newCard.LoadData(data);

            listTreasureCard.Add(newCard);
        }
    }

    public void Clear()
    {
        if (listTreasureCard == null)
            listTreasureCard = new List<TreasureCard>();
        foreach (TreasureCard card in listTreasureCard)
        {
            Destroy(card.gameObject);
        }

        listTreasureCard.Clear();
    }
}
