using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkSelectScene : MonoBehaviour
{
    private void OnEnable()
    {
        ActiveScene();
    }

    private void OnDisable()
    {
        if (!this.gameObject.activeSelf)
            return;
        DeActiveScene();
    }

    public void ActiveScene()
    {
        Debug.Log("Work Select Scene Actived!");
    }

    public void DeActiveScene()
    {
        Debug.Log("Work Select Scene Deactived!");
    }
}
