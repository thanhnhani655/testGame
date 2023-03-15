using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaTripleInterface : MonoBehaviour
{
    public event System.Action OnGachaAnimDone = delegate { };
    public GachaSingleInterface interface1;
    public GachaSingleInterface interface2;
    public GachaSingleInterface interface3;

    public float timeRun1 = 3;
    public float timedelay = 1f;

    public List<GachaItemData> listItem;

    private void Start()
    {
        interface1.timeRun = timeRun1;
        interface2.timeRun = timeRun1 + timedelay;
        interface3.timeRun = timeRun1 + timedelay * 2;
        interface3.OnGachaAnimDone += FireEventGachaAnimDone;
    }

    public void Gacha()
    {
        listItem = new List<GachaItemData>();
        interface1.Gacha();
        interface2.Gacha();
        interface3.Gacha();
        listItem.Add(interface1.item);
        listItem.Add(interface2.item);
        listItem.Add(interface3.item);
    }

    public List<GachaItemData> GachaItem()
    {
        listItem = new List<GachaItemData>();
        listItem.Add(interface1.GachaItem());
        listItem.Add(interface2.GachaItem());
        listItem.Add(interface3.GachaItem());
        return listItem;
    }

    public void FireEventGachaAnimDone()
    {
        OnGachaAnimDone();
    }
}
