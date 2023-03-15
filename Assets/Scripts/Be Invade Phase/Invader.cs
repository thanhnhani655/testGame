using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;


public class Invader : MonoBehaviour
{
    public MonsterData data;

    public SpriteRenderer invaderSprite;

    public GameObject damagedTextPrefap;

    [SerializeField]
    private bool isDead = false;

    public bool IsDead { get => isDead;}

    private void Start()
    {
        data.OnDead += Dead;
    }

    public void SetData(MonsterData idata)
    {
        data = idata;
        data.baseStat.Initialize(data.stat);
    }

    [Button]
    private void Dead()
    {
        this.GetComponent<MobInDungeon>().SetInActive();
        invaderSprite.enabled = false;
        isDead = true;
    }

    public bool isJustFight = false;
    
    public void ShowDmg()
    {
        GameObject dmgText = Instantiate<GameObject>(damagedTextPrefap, this.transform);
        dmgText.GetComponent<DamageTextSetting>().SetText(data.dmgReceived.ToString());
        data.dmgReceived = 0;
    }
}
