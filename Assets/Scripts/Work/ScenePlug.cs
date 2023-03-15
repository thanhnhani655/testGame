using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePlug : MonoBehaviour
{

    public void ActiveScene()
    {
        this.gameObject.SetActive(true);
    }

    public void DeActiveScene()
    {
        this.gameObject.SetActive(false);
    }
}
