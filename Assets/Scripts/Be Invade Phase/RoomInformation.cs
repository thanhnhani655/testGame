using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomInformation : MonoBehaviour
{
    [SerializeField]
    private Room room;

    [SerializeField]
    private List<DataInformationCard> listCard;
    [SerializeField]
    private DataInformationCard dataCardPrefap;
    [SerializeField]
    private GameObject card_holder;

    [SerializeField]
    private float updateDelay = 1f;
    [SerializeField]
    private float counter = 0;

    public void ActiveRoomInformation(Room iroom)
    {
        if (listCard == null)
            listCard = new List<DataInformationCard>();
        this.gameObject.SetActive(true);
        LoadRoom(iroom);
    }

    public void Deactive()
    {
        this.gameObject.SetActive(false);
    }

    public void LoadRoom(Room iRoom)
    {
        ClearCard();
        room = iRoom;

        foreach (MonsterData data in iRoom.ListMonInRoom)
        {
            DataInformationCard newCard = Instantiate<DataInformationCard>(dataCardPrefap, card_holder.transform);
            listCard.Add(newCard);

            newCard.LoadInformation(data);
        }
    }

    public void ClearCard()
    {
        foreach(DataInformationCard card in listCard)
        {
            Destroy(card.gameObject);
        }
        listCard.Clear();
    }

    private void Update()
    {
        counter += Time.deltaTime;

        if (counter < updateDelay)
            return;

        counter = 0;

        foreach (DataInformationCard card in listCard)
        {
            card.LoadInformation(card.GetData);
        }

        
    }
}
