using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseInstanceAccess : MonoBehaviour
{
    private static DatabaseInstanceAccess instance;
    public static DatabaseInstanceAccess Instance => instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public GachaItemDatabase GachaDatabase;
    public MonsterDatabase monsterDatabase;
    public SkillDatabase skillDatabase;
    public BuffDatabase buffDatabase;
}
