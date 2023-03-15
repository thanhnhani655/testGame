using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MonsterManagementInterationCard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public MonsterManagement manager;
    [SerializeField]
    private GameObject AddSkillButton;

    [Header("Interact")]
    public bool interactable = true;
    public float timeCount = 0;
    public float timeHoldNeed = 1f;
    public float timeClickLimit = 0.1f;
    public bool interacted = false;

    public void InteractOnHold()
    {
        Debug.Log("On Hold");
    }

    public void InteractOnClick()
    {
        manager.SelectCard(this.GetComponent<MonsterCard>().data);

    }

    #region Interaction

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!interactable)
            return;
        StartCoroutine(OnHold());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        /*if (!interactable)
            return;
        if (timeCount < timeClickLimit)
        {
            InteractOnClick();
            interacted = true;
        }

        timeCount = 0;
        return;*/
    }

    public void ShowAddSkillButton()
    {
        if (AddSkillButton != null)
            AddSkillButton.SetActive(true);
    }

    public void HideAddSkillButton()
    {
        if (AddSkillButton != null)
            AddSkillButton.SetActive(false);
    }

    private IEnumerator OnHold()
    {
        while (true)
        {
            if (interacted)
            {
                interacted = false;
                break;
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (timeCount < timeClickLimit)
                {
                    InteractOnClick();
                }

                timeCount = 0;
                break;
            }

            timeCount += Time.deltaTime;
            if (timeCount > timeHoldNeed)
            {
                InteractOnHold();
                break;
            }
            yield return null;
        }
    }
    #endregion
}
