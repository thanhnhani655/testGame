using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnAnimatedEnd : MonoBehaviour
{
    public void DestroyParent()
    {
        Destroy(this.transform.parent.gameObject);
    }
}
