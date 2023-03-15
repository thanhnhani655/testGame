using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    [SerializeField]
    private List<MonsterData> enemys;
    [SerializeField]
    private List<MonsterData> allies;
    [SerializeField]
    private List<MonsterData> targets;

    [SerializeField]
    private MonsterData Fighter;

    private float heal = 0;
    private Buff buff;
    private float dmg = 0;

    
    private void Start()
    {
        if (enemys == null)
            enemys = new List<MonsterData>();
        if (allies == null)
            allies = new List<MonsterData>();
        if (targets == null)
            targets = new List<MonsterData>();
    }
    
    public void GetAllTargetAble(MonsterData ifighter ,Room room)
    {
        Fighter = ifighter;
        enemys = new List<MonsterData>();
        allies = new List<MonsterData>();
        targets = new List<MonsterData>();
        heal = 0;
        buff = null;
        dmg = 0;
        if (Parameter.Instance.debugCombatController)
        {
            Debug.Log(ifighter.monName + " is a " + ifighter.faction.ToString() + " fighter.");
            Debug.Log("Get All Target Able");
        }

        //Get Mob In Dungeon
        foreach (MonsterData data in room.ListMonInRoom)
        {
            if (data.faction == Fighter.faction)
            {
                allies.Add(data);
            }
            else
            {
                enemys.Add(data);
            }
            data.CaptureStat();
        }

        //Get Invader In Dungeon
        foreach (Invader data in room.ListInvaderInRoom)
        {
            if (data.isJustFight)
            {
                if (Parameter.Instance.debugCombatController)
                {
                    Debug.Log(data.data.monName + "Just Fight");
                }
                continue;                
            }
          
            if (data.data.faction == Fighter.faction)
            {
                allies.Add(data.data);
            }
            else
            {
                enemys.Add(data.data);
            }
            data.data.CaptureStat();
        }


        if (Parameter.Instance.debugCombatController)
        {
            if (enemys.Count == 0)
            {
                DebugABC();
            }
        }

        this.RemovingDead();

        if (Parameter.Instance.debugCombatController)
        {
            Debug.Log("Check Amount Target: " + targets.Count);
            Debug.Log("Has " + allies.Count.ToString() + " Allies Target.");
            Debug.Log("Has " + enemys.Count.ToString() + " Enemy Target.");
        }
        

        this.SelectTarget();
    }


    private void RemovingDead()
    {
        for (int i = 0; i < enemys.Count; i++)
        {
            if (enemys[i].isDead)
            {
                enemys.RemoveAt(i);
                i--;
            }
        }

        for (int i = 0; i < allies.Count; i++)
        {
            if (allies[i].isDead)
            {
                allies.RemoveAt(i);
                i--;
            }
        }
    }

    public void DebugABC()
    {
        Debug.Log("------Start Log------");
        Debug.Log("Room name: " + this.gameObject.name);
        Debug.Log("Fighter name: " + this.Fighter.monName);
        foreach(MonsterData data in this.GetComponent<Room>().ListMonInRoom)
        {
            Debug.Log("Monster Name: " + data.monName);
        }
        foreach (Invader data in this.GetComponent<Room>().ListInvaderInRoom)
        {
            Debug.Log("Invader Name: " + data.data.monName + " & isJustFight: " + data.isJustFight.ToString());
        }

        Debug.Log("------Ends Log------");
    }

    public void SelectTarget()
    {
        if (Parameter.Instance.debugCombatController)
        {
            Debug.Log("Select Target");
        }
        if (Fighter.mainSkill.range == TargetRange.Single)
        {
            if (Fighter.mainSkill.faction == TargetFaction.Self)
            {
                targets.Add(Fighter);
            }
            else if (Fighter.mainSkill.faction == TargetFaction.Allies)
            {
                if (allies.Count > 0)
                {
                    targets.Add(allies[Random.Range(0, allies.Count)]);
                }
            }
            else if (Fighter.mainSkill.faction == TargetFaction.Enemy)
            {
                if (enemys.Count > 0)
                {
                    targets.Add(RandomTarget(enemys));
                }
            }
        }
        else if (Fighter.mainSkill.range == TargetRange.Wide)
        {
            if (Fighter.mainSkill.faction == TargetFaction.Allies)
            {
                if (allies.Count > 0)
                    targets.AddRange(allies);
            }
            else if (Fighter.mainSkill.faction == TargetFaction.Enemy)
            {
                if (enemys.Count > 0)
                    targets.AddRange(enemys);
            }
        }

        if (Parameter.Instance.debugCombatController)
        {
            Debug.Log("Has " + targets.Count + " target.");
        }
        if (targets.Count == 0)
        {
            Fighter.baseStat.timeCounter = Fighter.baseStat.attackSpeed;
            if (Parameter.Instance.debugCombatController)
            {
                Debug.Log("Combat Over Without Target");
            }
            return;
        }

        //Select Route
        if (Fighter.mainSkill.type == SkillType.Attack)
        {
            CalculateBeforeAttack();
            return;
        }
        if (Fighter.mainSkill.type == SkillType.Heal)
        {
            CalculateBeforeHeal();
            return;
        }
        if (Fighter.mainSkill.type == SkillType.Buff)
        {
            CalculateBeforeBuff();
        }
    }

    //Healing Route
    public void CalculateBeforeHeal()
    {
        heal = Fighter.mainSkill.value1 * Fighter.captureStat.atk * (1 + Fighter.captureStat.healingBonus);
        if (Parameter.Instance.debugCombatController)
        {
            Debug.Log("Heal = " + heal.ToString());
        }
        Heal();
    }

    public void Heal()
    {
        foreach (MonsterData data in targets)
        {
            if (Parameter.Instance.debugCombatController)
            {
                Debug.Log(data.monName + " HP Before Heal: " + data.baseStat.currentHP);
            }
            data.baseStat.currentHP = data.baseStat.currentHP + heal > data.baseStat.maxHP 
                                        ? data.baseStat.maxHP
                                        : data.baseStat.currentHP + heal;
            if (Parameter.Instance.debugCombatController)
            {
                Debug.Log(data.monName + " HP After Heal: " + data.baseStat.currentHP);
            }
        }

        DoStuffAfterDoAction();
    }

    //Buff Route
    public void CalculateBeforeBuff()
    {
        // Dành cho những buff có stat được tính dựa theo Stat, ví dụ như Tăng %ATK DỰA THEO %HP

    }

    public void Buff ()
    {
        foreach (MonsterData data in targets)
        {
            //Còn Sơ Sài, Cần Hàm Add Buff riêng
            data.baseStat.listBuff.Add(buff);
        }

        DoStuffAfterDoAction();
    }

    //Attack Route
    public void CalculateBeforeAttack()
    {
        //Notification Phase
        if (Parameter.Instance.debugCombatController)
        {
            Debug.Log("Calculate Before Attack");
        }

        //Check Weak Buff
        float weakBuff = 0;
        Buff WeakBuff = Fighter.baseStat.listBuff.Find(x => x.id == "BUFF_002");
        if (WeakBuff != null)
        {
            weakBuff = 0.25f;
            WeakBuff.buffStack--;
            if (WeakBuff.buffStack == 0) Fighter.baseStat.listBuff.Remove(WeakBuff);

            Parameter.Instance.CombatDebug("Weak Buff: " + weakBuff);
        }
        //Check Attack More Buff
        float atkMoreBuff = 0;
        Buff AtkMoreBuff = Fighter.baseStat.listBuff.Find(x => x.id == "BUFF_006");
        if (AtkMoreBuff != null)
        {
            atkMoreBuff = AtkMoreBuff.buffStack;
            Parameter.Instance.CombatDebug("Attack More Buff: " + atkMoreBuff);
        }
        
        //Calculate Critical
        float focusBuff = 0;
        Buff FocusBuff = Fighter.baseStat.listBuff.Find(x => x.id == "BUFF_004");
        if (FocusBuff != null)
        {
            focusBuff = 0.1f;
            FocusBuff.buffStack = FocusBuff.buffStack == 1 ? 0 : FocusBuff.buffStack / 2;
            if (FocusBuff.buffStack == 0) Fighter.baseStat.listBuff.Remove(FocusBuff);

            Parameter.Instance.CombatDebug("Focus Buff: " + focusBuff);
        }
        float criticalRatio = Random.value < (Fighter.captureStat.criticalRate + focusBuff) ? Fighter.captureStat.criticalDmg : 0;

        //Danger Instict
        float dangerInstictBuff = 0;
        if (Fighter.subSkill.Exists(x=>x.skillID == "SKIL_014"))
        {
            dangerInstictBuff += (0.05f * enemys.Count);

            Parameter.Instance.CombatDebug("Danger Instict: " + dangerInstictBuff);
        }

        //Murder
        float murederAttackBuff = 0;
        if (Fighter.subSkill.Exists(x => x.skillID == "SKIL_016"))
        {
            murederAttackBuff += (0.2f * targets.Count);

            Parameter.Instance.CombatDebug("Murder: " + murederAttackBuff);
        }

        //Check Power Boots Buff
        float powerBoots = 0;
        Buff PowerBoots = Fighter.baseStat.listBuff.Find(x => x.id == "BUFF_008");
        if (PowerBoots != null)
        {
            powerBoots = PowerBoots.buffValue;
            Parameter.Instance.CombatDebug("Power Boots: " + powerBoots);
        }

        //Check Share Power Buff
        float sharePower = 0;
        Buff SharePower = Fighter.baseStat.listBuff.Find(x => x.id == "BUFF_008");
        if (SharePower != null)
        {
            sharePower = SharePower.buffValue;
            Fighter.baseStat.listBuff.Remove(SharePower);
            Parameter.Instance.CombatDebug("Share Power: " + sharePower);
        }

        //Calculate Dmg
        dmg = Fighter.mainSkill.value1 * ((Fighter.captureStat.atk + atkMoreBuff + sharePower) *  (1 + murederAttackBuff + powerBoots)) * (1 + Fighter.captureStat.dmgOutput + dangerInstictBuff) * (1 + criticalRatio) * (1-weakBuff);



        //Debug Stat
        if (Parameter.Instance.debugCombatController)
        {
            Debug.Log("Capture Atk: " + Fighter.captureStat.atk);
            Debug.Log("Dmg = " + dmg.ToString());
            Debug.Log("Skill Value: " + Fighter.mainSkill.value1);
            Debug.Log("Total Flat Attack: " + (Fighter.captureStat.atk + atkMoreBuff + sharePower));
            Debug.Log("%Attack Buff: " + (1 + murederAttackBuff + powerBoots));
            Debug.Log("Dmg Bonus Buff: " + (1 + Fighter.captureStat.dmgOutput + dangerInstictBuff));
            Debug.Log("Critical: " + (1 + criticalRatio));
        }

        

        Attack();
        //Thêm phần tính thêm sát thương mỗi stack của 1 loại buff nào đó
    }

    public void Attack()
    {
        //Steel
        float steelReduceDmg = 0;

        //Vulnerable
        float vulnerableDmg = 0;

        foreach (MonsterData data in targets)
        {
            //Steel
            steelReduceDmg = 0f;
            if (data.subSkill.Exists(x => x.skillID == "SKIL_015"))
                steelReduceDmg = 0.15f;

            //Vulnerable
            vulnerableDmg = 0;

            //Check Fragile
            float fragileDebuff = 0;
            Buff FragileDebuff = data.baseStat.listBuff.Find(x => x.id == "BUFF_007");
            if (FragileDebuff != null)
            {
                fragileDebuff = 0.25f;
                FragileDebuff.buffStack = FragileDebuff.buffStack == 1 ? 0 : (int)(FragileDebuff.buffStack / 2f);
                if (FragileDebuff.buffStack == 0) data.baseStat.listBuff.Remove(FragileDebuff);
            }


            //Diamond
            Buff diamond = data.baseStat.listBuff.Find(x => x.id == "BUFF_005");
            if (diamond != null)
            {
                dmg = dmg * (1 + data.captureStat.dmgGain - 0.5f - steelReduceDmg + fragileDebuff) + vulnerableDmg * ( 1 + data.captureStat.dmgGain + fragileDebuff);
                diamond.buffStack--;
                if (diamond.buffStack <= 0) data.baseStat.listBuff.Remove(diamond);
            }
            else
                dmg = dmg * (1 + data.captureStat.dmgGain - steelReduceDmg) + vulnerableDmg* data.captureStat.dmgGain;
            

            dmg = data.baseStat.DmgShield(dmg);

            //Cần làm Function để Delta HP
            data.baseStat.currentHP = Mathf.Clamp(data.baseStat.currentHP - dmg, 0, data.baseStat.maxHP);
            data.dmgReceived += dmg;
            data.isTakeDmg = true;

            if (data.baseStat.currentHP == 0)
            {
                data.Dead();
            }

            if (Parameter.Instance.debugCombatController)
            {
                Debug.Log(data.monName + " recieve " + dmg + " dmg");
            }
        }

        DoStuffAfterDoAction();
    }

    public void DoStuffAfterDoAction()
    {
        if (Fighter.subSkill != null)
        {
            if (Fighter.subSkill.Exists(x=>x.skillID == "SKIL_012"))
            {
                float extraHeal = dmg * 0.1f;
                Fighter.baseStat.currentHP = Fighter.baseStat.currentHP + extraHeal > Fighter.baseStat.maxHP
                                        ? Fighter.baseStat.maxHP
                                        : Fighter.baseStat.currentHP + extraHeal;
            }
            if (Fighter.subSkill.Exists(x=>x.skillID == "SKIL_011"))
            {
                foreach (MonsterData data in targets)
                {
                    data.baseStat.AddBuff("BUFF_004",1,0);
                }
            }
            if (Fighter.subSkill.Exists(x => x.skillID == "SKIL_009"))
            {
                foreach (MonsterData data in targets)
                {
                    data.baseStat.AddBuff("BUFF_002", 5, 0);
                }
            }
            if (Fighter.subSkill.Exists(x => x.skillID == "SKIL_017"))
            {
                foreach (MonsterData data in targets)
                {
                    data.baseStat.AddBuff("BUFF_007", 5, 0);
                }
            }
            if (Fighter.subSkill.Exists(x => x.skillID == "SKIL_022"))
            {
                foreach (MonsterData data in targets)
                {
                    data.baseStat.dmgOutput += 0.05f;
                }
            }
            if (Fighter.subSkill.Exists(x => x.skillID == "SKIL_024"))
            {
                foreach (MonsterData data in targets)
                {
                    data.baseStat.InscreaseShield(Fighter.baseStat.Capture().atk * 0.2f);
                }
            }
            if (Fighter.subSkill.Exists(x => x.skillID == "SKIL_025"))
            {
                foreach (MonsterData data in targets)
                {
                    data.baseStat.AddBuff("BUFF_008", 1,0.3f);
                }
            }
            if (Fighter.subSkill.Exists(x => x.skillID == "SKIL_006"))
            {
                foreach (MonsterData data in targets)
                {
                    data.baseStat.AddBuff("BUFF_009", 1, 0.6f * Fighter.baseStat.Capture().atk);
                }
            }
        }
        
        if (Parameter.Instance.debugCombatController)
        {
            Debug.Log("End");
        }
        //Nothing Here
        //Active Counter Attack
        //Attach Buff
        //Life Steal, Life Leech, Heal Allies
        //Revive
    }

    public MonsterData RandomTarget(List<MonsterData> listDatas)
    {
        MonsterData temp;
        List<int> listRatio = new List<int>();
        for (int i = 0; i < listDatas.Count; i++)
        {
            if (listDatas[i].subSkill != null)
            {
                if (listDatas[i].subSkill.Exists(x=>x.skillID == "SKIL_013"))
                {
                    listRatio.Add(i);
                    listRatio.Add(i);
                    listRatio.Add(i);
                    continue;
                }
            }
            listRatio.Add(i);
        }

        int index = listRatio[Random.Range(0, listRatio.Count)];
        temp = listDatas[index];
        return temp;
    }
}
