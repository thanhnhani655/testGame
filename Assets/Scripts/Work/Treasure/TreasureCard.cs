using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureCard : MonoBehaviour
{
    public Text itemName;
    public Text Desc;
    public Text Stack;
    public Treasure data;

    public void LoadData(Treasure idata)
    {
        data = idata;
        itemName.text = data.itemName;
        itemName.color = data.rarity switch
        {
            Rarity.Tier1 => Color.white,
            Rarity.Tier2 => Color.green,
            Rarity.Tier3 => Color.blue,
            Rarity.Tier4 => Color.red
        };
        Desc.text = data.desc;
        Stack.text = data.stack.ToString();
    }
}
