using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MonsterCard : MonoBehaviour, IPointerClickHandler
{
    public Text level;
    public Text monName;
    public Text atkValue;
    public Text hpValue;
    public Text address;
    public Text legion;
    public Text stack;

    public MonsterData data;
    public DungeonManagementUI manager;
    public PrisonUI prisonerManager;

    [Header("Interact")]
    public bool interactable = true;
    public bool disableLeftInteract = false;
    public bool disableRightInteract = false;
    public float timeCount = 0;
    public float timeHoldNeed = 0.5f;
    public float timeClickLimit = 0.1f;
    public float timeClicked = 0;
    public float delayClick = 0.15f;
    public bool interacted = false;

    [SerializeField]
    private GameObject MonsterData;
    [SerializeField]
    private SkillInformation monsterSkillData;

    [SerializeField]
    private bool ReadSkillMode;


    public bool LevelUp()
    {
        if (!data.LevelUp())
            return false;
        this.SetMonster(data);
        return true;
    }

    public void SetMonster(MonsterData idata)
    {
        data = idata;
        this.ActiveMonsterDataUI();
        level.text = "LV " + data.stat.Level.ToString();
        monName.color = data.rarity switch
        {
            Rarity.Tier1 => Color.white,
            Rarity.Tier2 => Color.green,
            Rarity.Tier3 => Color.blue,
            Rarity.Tier4 => Color.red
        };

        monName.text = data.monName;
        atkValue.text = data.stat.atk.ToString();
        hpValue.text = data.stat.maxHP.ToString();
        legion.text = data.legion switch
        {
            Legion.Red => "Red",
            Legion.Blue => "Blue",
            Legion.Black => "Black"
        };

        if (data.address == ItemAddress.Inventory)
        {
            if (address != null)
                address.gameObject.SetActive(false);
        }
        else
        {
            if (address != null)
            {
                address.gameObject.SetActive(true);
                address.text = data.dungeonAddress;
            }
        }
    }

    public void InteractOnLeftClick()
    {
        if (manager != null)
        {
            if (manager.IsActive)
                manager.MoveMonster(data);
        }
    }

    public void InteractOnDoubleClick()
    {
        if (manager != null)
        {
            if (manager.IsActive)
                manager.Zoom(data);
        }
        if (prisonerManager != null)
        {
            prisonerManager.Zoom(data);
        }
    }

    public void InteractOnRightClick()
    {
        if (manager != null)
        {
            if (manager.IsActive)
                manager.Zoom(data);
        }
        if (monsterSkillData != null)
        {
            if (monsterSkillData.gameObject.activeSelf)
            {
                ActiveMonsterDataUI();
            }
            else
            {
                ActiveMonsterSkillUI();
            }
        }
    }

    public void ActiveMonsterDataUI()
    {
        if (monsterSkillData == null)
            return;
        monsterSkillData.gameObject.SetActive(false);
        MonsterData.gameObject.SetActive(true);
    }

    public void ActiveMonsterSkillUI()
    {
        monsterSkillData.gameObject.SetActive(true);
        monsterSkillData.LoadSkillData(data);
        MonsterData.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!interactable)
            return;
        if (eventData.clickCount == 1)
        {
            if (eventData.button == PointerEventData.InputButton.Left && !disableLeftInteract)
                InteractOnLeftClick();
            if (eventData.button == PointerEventData.InputButton.Right && !disableRightInteract)
                InteractOnRightClick();
        }
        if (prisonerManager != null)
        {
            this.GetComponent<PrisonCard>().CorruptChoice();
        }
        
    }

}
