using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvadeController : MonoBehaviour
{
    public event System.Action AllInvaderIsDead = delegate { };
    public event System.Action OnVictory = delegate { };
    public event System.Action OnDefeat = delegate { };

    private static InvadeController instance;
    public static InvadeController Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    [SerializeField]
    private InvaderSpawner spawner;
    [SerializeField]
    private WaveGenerator InvadeGenerator;
    [SerializeField]
    private List<Invader> listInvader;

    [SerializeField]
    private bool dungeonLost = false;
    [SerializeField]
    private Room bossRoom;
    [SerializeField]
    private List<Room> RearRooms;
    [SerializeField]
    private RoomManager roomanager;
    [SerializeField]
    private TreasurePool treasurePool;
    [SerializeField]
    public Treasure newTreasure;

    [Header("Victory")]
    private int victorySoul = 0;
    public int soulStoneBySurvival = 0;
    public float soulFromSuck = 0;

    public bool isInvadePhase = false;

    public InvaderRank invadeRank;

    public bool DungeonLost { get => dungeonLost; }
    public int VictorySoul { get => victorySoul; }

    public void Initialize ()
    {
        if (RearRooms == null)
            RearRooms = new List<Room>();
        isInvadePhase = true;
        spawner.RemoveAllInvader();
        spawner.GetListInvaderComing(InvadeGenerator.GenerateWave(invadeRank));
        spawner.InvaderSpawnDone += GetListInvader;
        dungeonLost = false;
        bossRoom.isLocked = true;
        this.UnLockRearRoom();
        victorySoul = 0;
        soulFromSuck = 0;
        DungeonCore core = DungeonCore.Instance;
        foreach (MonsterData data in MonsterInventory.Instance.GetListData())
        {
            if (data.address == ItemAddress.Dungeon)
            {
                //Check Giant Buff
                float giantHp = 0;
                float giantAtk = 0;
                Debug.Log(data.dungeonAddress);
                if (roomanager.GetRoomById(data.dungeonAddress).facility != null)
                {
                    if (roomanager.GetRoomById(data.dungeonAddress).facility.id == "ROSY_008")
                    {
                        giantHp = 1;
                        giantAtk = 0.5f;
                    }

                }
                

                LegionCore legionCore = core.listLegionCore.Find(x => x.legionID == data.legion);
                Stat temp = data.stat.CloneStat();

                if (data.rank == MonsterRank.Boss)
                {
                    int bossLevel = temp.Level + core.coreLevel;
                    MonsterData tempBoss = data.CloneMon();
                    tempBoss.SetLevelUpTo(bossLevel);
                    temp = tempBoss.stat.CloneStat();
                }

                data.baseStat.Initialize(temp);

                //Inscrease Max HP
                float hpBonus = 0;
                hpBonus += data.rank == MonsterRank.Boss ? core.T_BossHP : 0;
                hpBonus += (roomanager.GetRoomById(data.dungeonAddress).isBossRoom && data.rank != MonsterRank.Boss) ? core.T_BossMinionHP : 0;
                Debug.Log("HP Bonus: " + hpBonus.ToString());
                data.baseStat.baseMaxHP = data.baseStat.baseMaxHP * (1 + core.Hp/100 + giantHp + hpBonus) + legionCore.hpFlatBonus;
                data.baseStat.maxHP = data.baseStat.baseMaxHP;
                data.baseStat.currentHP = data.baseStat.baseMaxHP;

                //Shield
                data.baseStat.InscreaseShield(data.baseStat.baseMaxHP * core.T_Shield);

                //Attack
                float atkBonus = 0;
                atkBonus += data.rank == MonsterRank.Boss ? core.T_BossAtk : 0;
                atkBonus += (roomanager.GetRoomById(data.dungeonAddress).isBossRoom && data.rank != MonsterRank.Boss) ? core.T_BossMinionAtk : 0;
                atkBonus += data.subSkill.Exists(x => x.skillID == "SKIL_020") ? 0.5f : 0;
                
                data.baseStat.atk = data.baseStat.atk * (1 + core.Atk / 100 + giantAtk + atkBonus) + legionCore.attackFlatBonus;

                float dmgBonus = 0;
                dmgBonus += atkBonus += data.subSkill.Exists(x => x.skillID == "SKIL_021") ? 0.5f : 0;
                data.baseStat.dmgOutput += core.DmgBonus + legionCore.dmgOutputBonus + dmgBonus;
                
                //Foucs Weakness
                float focusWeakness = data.subSkill.Exists(x => x.skillID == "SKIL_018") ? 0.3f : 0;
                float charge = data.subSkill.Exists(x => x.skillID == "SKIL_019") ? 1f : 0;

                data.baseStat.criticalRate += core.CriticalRate + legionCore.criticalRateBonus + focusWeakness;
                data.baseStat.criticalDmg += core.criticalDmg + charge;
                data.baseStat.healingBonus += core.HealingBonus;
                data.baseStat.attackSpeed -= core.Atkspd;

                if (core.T_DiamondBuff > 0)
                {
                    data.baseStat.AddBuff(DatabaseInstanceAccess.Instance.buffDatabase.CreateBuff("BUFF_005", (int)core.T_DiamondBuff, 0));
                }

                data.isDead = false;
            }
        }
    }

    private void VictoryCalculate()
    {
        victorySoul = Random.Range(250, 351);
        //Soul calculate
        //foreach (Invader invader in listInvader)
        //{
        //    victorySoul += (int)((invader.data.stat.Level * 50 * (1 + DungeonCore.Instance.SoulGain/100)));
        //}
        victorySoul += (int)(DayCounter.Instance.Day * Random.Range(50, 151) * (1 + DungeonCore.Instance.SoulGain / 100 + soulFromSuck));
        
        PlayerCurrency.Instance.DeltaSoul(victorySoul);
        this.RemoveAllInvader();
        spawner.InvaderSpawnDone -= GetListInvader;

        foreach (MonsterData data in MonsterInventory.Instance.GetListData())
        {
            if (data.address == ItemAddress.Dungeon)
            {
                data.ResetAlive();
            }

            //data.InscreaseExp(victorySoul * 0.05f);
        }

        roomanager.UnlockInsideAll();
        GetTreasure();

        OnVictory();
        isInvadePhase = false;
    }

    private void GameOverCalculate()
    {
        soulStoneBySurvival = DayCounter.Instance.Day;
        PlayerCurrency.Instance.DeltaSoulStone(soulStoneBySurvival);

        
        this.RemoveAllInvader();
        spawner.InvaderSpawnDone -= GetListInvader;
        roomanager.UnlockInsideAll();
        isInvadePhase = false;
        OnDefeat();
    }

    public void GetTreasure()
    {
        newTreasure = null;
        if (InvadeGenerator.rank == InvaderRank.Tier1)
        {
            return;
        }

        switch (InvadeGenerator.rank)
        {
            case InvaderRank.Tier2:
                newTreasure = treasurePool.PullOutItem(Rarity.Tier2);
                break;
            case InvaderRank.Tier3:
                newTreasure = treasurePool.PullOutItem(Rarity.Tier3);
                break;
            case InvaderRank.Tier4:
                newTreasure = treasurePool.PullOutItem(Rarity.Tier4);
                break;
        }

        TreasureInventory.Instance.AddTreasure(newTreasure);
    }

    public void InvadeIn1Second()
    {
        Invoke("StartInvade", 1f);
    }

    private void StartInvade()
    {
        Debug.Log("Start Invade");
        spawner.BeginSpawn();
    }

    private void GetListInvader(List<Invader> iList)
    {
        listInvader = iList;
        StartCoroutine(CheckAllDeadEvery2Second());
    }

    public void RemoveInvader(Invader invader)
    {
        Destroy(invader.gameObject);
        listInvader.Remove(invader);
    }

    public void RemoveAllInvader()
    {
        foreach (Invader invader in listInvader)
        {
            if (!PrisonerInventory.Instance.IsPrisonFull())
            {               
                float ratio = Random.value;
                Debug.Log("Check to Catach Prisoner " + ratio);
                if (ratio <= 0.1f)
                {
                    PrisonerInventory.Instance.AddPrisoner(invader.data);
                }
            }
            if (invader.gameObject != null)
                Destroy(invader.gameObject);
        }

        listInvader.Clear();
    }

    [SerializeField]
    private bool debugCheckMonsterDead = false;
    private bool CheckAllMonsterDead()
    {
        foreach (MonsterData data in MonsterInventory.Instance.GetListData().FindAll(x=>x.address == ItemAddress.Dungeon))
        {
            if (data.rank == MonsterRank.Boss)
                continue;
            List<MonsterData> boss = MonsterInventory.Instance.GetListData().FindAll(x => x.rank == MonsterRank.Boss);
            
            if (boss != null)
            {
                if (boss.Exists(x => x.dungeonAddress == data.dungeonAddress))
                    continue;
            }

            if (!data.isDead)
            {
                if (debugCheckMonsterDead)
                {
                    Debug.Log("Monster Cause false: " + data.id + " " + data.dungeonAddress);
                }
                return false;
            }
        }

        return true;
    }

    private bool CheckGameOver()
    {
        foreach (MonsterData data in MonsterInventory.Instance.GetListData().FindAll(x => x.address == ItemAddress.Dungeon))
        {
            if (!data.isDead)
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckAllInvaderDead()
    {
        foreach (Invader invader in listInvader)
        {
            if (!invader.IsDead)
                return false;
        }

        return true;
    }

    private void UnlockBossRoom()
    {
        bossRoom.isLocked = false;
    }

    private void LockRearRoom()
    {
        foreach (Room room in RearRooms)
        {
            room.isLocked = true;
        }
    }

    private void UnLockRearRoom()
    {
        foreach (Room room in RearRooms)
        {
            room.isLocked = false;
        }
    }

    private IEnumerator CheckAllDeadEvery2Second()
    {
        while(true)
        {
            if (DungeonLost)
                break;
            yield return new WaitForSeconds(2);
            
            if (bossRoom.isLocked)
            {
                if (CheckAllMonsterDead())
                {
                    UnlockBossRoom();
                    LockRearRoom();
                }
            }
            else
            {
                if (CheckGameOver())
                {
                    GameOverCalculate();
                    break;
                }
            }
            

            if (CheckAllInvaderDead())
            {
                AllInvaderIsDead();
                VictoryCalculate();
                break;
            }
        }
    }

    public void SetInvadeRank(InvaderRank rank)
    {
        invadeRank = rank;
    }

}
