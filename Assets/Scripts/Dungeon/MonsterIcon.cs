using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIcon : MonoBehaviour
{
    [SerializeField]
    private MonsterData data;
    

    public void SetData(MonsterData idata)
    {
        data = idata;
    }

    
}
