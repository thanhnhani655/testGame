using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffCard : MonoBehaviour
{
    [SerializeField]
    private Text cardText;
    public int cardIndex;


    public void SetBuffCard(BuffDay buffDay)
    {
        cardText.text = buffDay.BuffDescription;
    }
}
