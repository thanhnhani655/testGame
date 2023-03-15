using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacilityCard : MonoBehaviour
{
    [SerializeField]
    private Facility facility;

    [SerializeField]
    private Text facName;
    [SerializeField]
    private Text facLevel;
    [SerializeField]
    private Text Description;
    [SerializeField]
    private Image image;
    [SerializeField]
    private Text levelUP;

    public void LoadFacility(Facility ifac)
    {
        facility = ifac;
        image.sprite = facility.sprite;
        facName.text = facility.facilityName;

        if (facility.id == "ROSY_007")
        {
            if (BuildingController.Instance.isLockGumTraplevel10 && facility.level.LV == 10 ||
                BuildingController.Instance.isLockGumTraplevel20 && facility.level.LV == 20)
                facility.isLevelMax = true;
            else
                facility.isLevelMax = false;
        }
        if (facility.id == "ROSY_002")
        {
            if (BuildingController.Instance.isLockSoulExtractorTraplevel10 && facility.level.LV == 10 ||
                BuildingController.Instance.isLockSoulExtractorTraplevel20 && facility.level.LV == 20)
                facility.isLevelMax = true;
            else
                facility.isLevelMax = false;
        }

        if (!facility.isLevelMax)
            facLevel.text = "Level " + facility.level.LV.ToString();
        else
            facLevel.text = "Level Max" ;
        Description.text = facility.description;
        if (facility.value != 0)
            Description.text = Description.text.Replace("&x", facility.value.ToString());
        if (!facility.isLevelMax)
        {
            levelUP.transform.parent.gameObject.SetActive(true);
            levelUP.text = "Level UP (" + facility.level.ExpNeedToLevelUp + " soul)";
        }
        else
            levelUP.transform.parent.gameObject.SetActive(false);

        
    }

    public void LevelUp()
    {
        if (PlayerCurrency.Instance.Soul < facility.level.ExpNeedToLevelUp)
        {
            return;
        }

        PlayerCurrency.Instance.DeltaSoul(-facility.level.ExpNeedToLevelUp);
        facility.LevelUp();

        LoadFacility(facility);
        if (facility.id == "ROSY_003")
        {
            BuildingController.Instance.UpdatePrisonCapacity();
        }
        if (facility.id == "ROSY_004")
        {
            BuildingController.Instance.UpdateCorruptionCapacity();
        }
        if (facility.id == "ROSY_005")
        {
            BuildingController.Instance.UpdateBannerTier2();
        }
    }
}
