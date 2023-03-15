using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaSingleInterface : MonoBehaviour
{
    public event System.Action OnGachaAnimDone = delegate { };
    public GachaBanner gachaSystem;
    public Text itemDisplay;
    public GachaItemData item;
    public List<GachaItemData> listGachaAble;

    [SerializeField]
    public float timeRun = 3f;
    [SerializeField]
    private float timeCount = 0;
    private float delayRandomTrack = 0.05f;
    [Header("Rarity Color")]
    public Color tier1Color = Color.white;
    public Color tier2Color = Color.blue;
    public Color tier3Color = Color.yellow;
    public Color tier4Color = Color.red;

    public void Gacha()
    {
        item = gachaSystem.RandomItem();
        listGachaAble = gachaSystem.listGachaAble;
        itemDisplay.gameObject.SetActive(true);

        MonsterInventory.Instance.AddMonster(DatabaseInstanceAccess.Instance.monsterDatabase.GetDataByID(item.itemID));

        StartCoroutine(RunGacha());
    }

    public GachaItemData GachaItem()
    {
        item = gachaSystem.RandomItem();
        listGachaAble = gachaSystem.listGachaAble;
        itemDisplay.gameObject.SetActive(true);
        StartCoroutine(RunGacha());

        return item;
    }

    private IEnumerator RunGacha()
    {
        while (true)
        {
            timeCount += Time.deltaTime;
            if (timeCount >= timeRun || Input.GetMouseButtonUp(0))
            {
                DisplayItem(item);
                Debug.Log("Item Name: " + item.itemName + " | " + "Rarity: " + item.itemRarity.ToString());
                timeCount = 0;
                delayRandomTrack = 0;
                OnGachaAnimDone();
                break;
            }

            if (timeCount - delayRandomTrack > 0.05f)
            {
                int Index = Random.Range(0, listGachaAble.Count);
                DisplayItem(listGachaAble[Index]);
                delayRandomTrack = timeCount;
            }

            yield return null;
        }
    }

    private void DisplayItem(GachaItemData data)
    {
        itemDisplay.text = data.itemName;
        switch (data.itemRarity)
        {
            case Rarity.Tier1:
                itemDisplay.color = tier1Color;
                break;
            case Rarity.Tier2:
                itemDisplay.color = tier2Color;
                break;
            case Rarity.Tier3:
                itemDisplay.color = tier3Color;
                break;
            case Rarity.Tier4:
                itemDisplay.color = tier4Color;
                break;
        }

    }
}
