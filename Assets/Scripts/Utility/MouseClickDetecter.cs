using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickDetecter : MonoBehaviour
{
    public event System.Action OnClick = delegate { };

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit, 100f))
            {
                if (raycastHit.transform != null)
                {
                    OnClick();
                }
            }
        }
    }
}
