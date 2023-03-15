using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum InvaderRank
{
    Tier1,
    Tier2,
    Tier3,
    Tier4
}

public class WaveGenerator : MonoBehaviour
{
    private static WaveGenerator instance;
    public static WaveGenerator Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    [SerializeField]
    private MonsterDatabase database;
    [SerializeField]
    private List<MonsterData> listInvaderDatabase;
    [SerializeField]
    private List<MonsterData> listInvaderHeroDatabase;

    //Tam thoi khong dung
    //[SerializeField]
    //private float strenght = 1; 

    // X = MAX  Y = MIN
    [SerializeField]
    private Vector2 invLevel;
    // X = MIN  Y = MAX
    [SerializeField]
    private Vector2 invLenght;
    [SerializeField]
    private float deviantMinMax = 1;

    [SerializeField]
    private float difficultLevel = 1;
    [SerializeField]
    private float difficultInscrease = 3;
    //[SerializeField]
    //private int LevelCount = 1;
    [SerializeField]
    public InvaderRank rank = InvaderRank.Tier1;

    [SerializeField]
    private List<PowerLevelRecorder> powerLevelRecord;


    [SerializeField]
    public int Level = 1;
    [SerializeField]
    public int levelInRow = 1;
    [SerializeField]
    private float difficultInscreaseInRow = 1;
    [SerializeField]
    private float difficultInscreaseInRowRatio = 1;

    private void Start()
    {
        Initialize();
    }

    [Button]
    public void Initialize()
    {
        Level = 1;
        levelInRow = 1;
        invLenght = new Vector2(5, 5);
        difficultLevel = 5;
        difficultInscrease = 3;
        invLevel.x = difficultLevel / invLenght.x;
        invLevel.y = difficultLevel / invLenght.y;
        deviantMinMax = 1;
        difficultInscreaseInRow = 1;
        difficultInscreaseInRowRatio = 0.3f;
    }

    [Button]
    private void AddInvaderInDatabase(string id)
    {
        listInvaderDatabase.Add(database.GetDataByID(id));
    }

    [Button]
    private void AddHeroInDatabase(string id)
    {
        listInvaderHeroDatabase.Add(database.GetDataByID(id));
    }

    public void LevelUP()
    {
        Level++;
        levelInRow++;

        if (levelInRow == 11)
        {
            levelInRow = 1;
            difficultInscrease *= 1.5f;
            difficultInscreaseInRow++;
            difficultInscreaseInRowRatio += 0.3f;
        }

        difficultInscrease += (difficultInscreaseInRow * levelInRow * difficultInscreaseInRowRatio);

        difficultLevel += difficultInscrease;

        //Lenght
        if (invLenght.x < 48)
            invLenght.x++;
        if (Level < 10)
            invLenght.y = invLenght.x + 2;
        else if (invLenght.y < 98)
        {
            invLenght.y = invLenght.x + deviantMinMax;
            deviantMinMax++;
        }

        //Level
        invLevel.x = difficultLevel / invLenght.x;
        invLevel.y = difficultLevel / invLenght.y;
        invLevel.x += Level * 0.5f;
    }

    public List<MonsterData> GenerateWave(InvaderRank iRank = InvaderRank.Tier1)
    {
        List<MonsterData> listInvaderComing = new List<MonsterData>();
        rank = iRank;

        int lenght = (int)Random.Range(invLenght.x, invLenght.y);
        int currentDF = 0;

        for (int i = 0; i < lenght; i++)
        {
            MonsterData newInvader = new MonsterData();
            newInvader = GetInvaderByRank();

            //Set Min Level
            newInvader.SetLevel((int)invLevel.y);

            currentDF += newInvader.stat.Level;

            listInvaderComing.Add(newInvader);
        }

        while (currentDF < difficultLevel)
        {
            foreach (MonsterData invader in listInvaderComing)
            {
                if (Random.value > 0.5f)
                {
                    invader.LevelUp();
                    currentDF += 1;
                    if (currentDF >= difficultLevel)
                        break;
                }
            }
        }

 


        return listInvaderComing;
    }

    public MonsterData GetInvaderByRank()
    {
        MonsterData newInvader;
        List<MonsterData> listRollAble = new List<MonsterData>();
        switch (rank)
        {
            case InvaderRank.Tier1:
                listRollAble.AddRange(listInvaderDatabase.FindAll((x => x.rarity == Rarity.Tier1)));
                listRollAble.AddRange(listInvaderDatabase.FindAll((x => x.rarity == Rarity.Tier2)));
                break;
            case InvaderRank.Tier2:
                listRollAble.AddRange(listInvaderDatabase.FindAll((x => x.rarity == Rarity.Tier1)));
                listRollAble.AddRange(listInvaderDatabase.FindAll((x => x.rarity == Rarity.Tier2)));
                listRollAble.AddRange(listInvaderDatabase.FindAll((x => x.rarity == Rarity.Tier3)));
                break;
            case InvaderRank.Tier3:
                listRollAble.AddRange(listInvaderDatabase.FindAll((x => x.rarity == Rarity.Tier2)));
                listRollAble.AddRange(listInvaderDatabase.FindAll((x => x.rarity == Rarity.Tier3)));
                listRollAble.AddRange(listInvaderDatabase.FindAll((x => x.rarity == Rarity.Tier4)));
                break;
            case InvaderRank.Tier4:
                listRollAble.AddRange(listInvaderDatabase.FindAll((x => x.rarity == Rarity.Tier3)));
                listRollAble.AddRange(listInvaderDatabase.FindAll((x => x.rarity == Rarity.Tier4)));
                break;
        }
        int index = Random.Range(0, listRollAble.Count);
        newInvader = listRollAble[index].CloneMon();
        return newInvader;
    }

    public void RecordPowerLevel()
    {
        if (powerLevelRecord == null)
            powerLevelRecord = new List<PowerLevelRecorder>();
        List<MonsterData> listMonsInDungeon = MonsterInventory.Instance.GetListData().FindAll(x => x.address == ItemAddress.Dungeon);
        PowerLevelRecorder total = new PowerLevelRecorder();
        foreach( MonsterData data in listMonsInDungeon)
        {
            total.DungeonPowerLevel += data.stat.Level * 10;
        }
        total.InvaderDifficulty = difficultLevel;
        total.CalculateRatio();
        powerLevelRecord.Add(total);
    }

}


[System.Serializable]
public class PowerLevelRecorder
{
    public float DungeonPowerLevel = 0;
    public float InvaderDifficulty = 0;
    public float DungeonAndInvaderRaio = 0;

    public void CalculateRatio()
    {
        DungeonAndInvaderRaio = DungeonPowerLevel / InvaderDifficulty;
    }
}
