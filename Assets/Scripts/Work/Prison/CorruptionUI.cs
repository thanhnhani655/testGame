
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptionUI : MonoBehaviour
{
    [SerializeField]
    private CorruptionController controller;
    [SerializeField]
    private List<GameObject> listMonCard;
    [SerializeField]
    private MonsterCard monCardPrefaps;
    [SerializeField]
    private GameObject AddableCardPrefaps;
    [SerializeField]
    private GameObject MonCardArea;

    public void LoadMonster()
    {
        if (listMonCard == null)
            listMonCard = new List<GameObject>();
        ClearCard();

        if (listMonCard.Count < controller.maxCorruptor)
        {
            GameObject newCard = Instantiate<GameObject>(AddableCardPrefaps, MonCardArea.transform);
            listMonCard.Add(newCard.gameObject);

        }
    }

    public void ClearCard()
    {
        foreach (GameObject card in listMonCard)
        {
            Debug.Log("Destroy " + card.name);
            Destroy(card.gameObject);
        }

        listMonCard.Clear();
    }
}
