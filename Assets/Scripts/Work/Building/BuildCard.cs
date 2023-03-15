using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildCard : MonoBehaviour
{
    public Facility facility;
    public Image image;
    public Text buildName;
    public Text Desc;
    public Text soulPrice;
    public Text corrupPrice;
    public Text concentraPrice;

    public void Load(Facility ifac)
    {
        facility = ifac;
        image.sprite = facility.sprite;
        buildName.text = facility.facilityName;
        Desc.text = facility.description;
        Desc.text.Replace("&x", facility.value.ToString());
        soulPrice.text = ifac.soulPrice.ToString();
        corrupPrice.text = ifac.corruptedSoulPrice.ToString();
        concentraPrice.text = ifac.concentratedSoulPrice.ToString();
    }
}
