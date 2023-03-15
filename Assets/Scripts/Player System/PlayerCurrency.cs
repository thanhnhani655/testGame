using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCurrency : MonoBehaviour
{
    private static PlayerCurrency instance;
    public static PlayerCurrency Instance => instance;

    public event System.Action CorruptedSoulChange = delegate { };
    public event System.Action SoulChange = delegate { };
    public event System.Action SoulConcentratedChange = delegate { };
    public event System.Action SoulStoneChange = delegate { };

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    [SerializeField]
    private float soul = 0;
    [SerializeField]
    private float corruptedSoul = 0;
    [SerializeField]
    private float soulConcentrated = 0;
    [SerializeField]
    private float soulStone = 0;

    public float Soul
    { 
        get => soul; 
        set
        {
            soul = value;
            SoulChange(); 
        }
    }

    public float CorruptedSoul 
    { 
        get => corruptedSoul; 
        set
        { 
            corruptedSoul = value;
            CorruptedSoulChange();
        }
    }

    public float SoulConcentrated 
    { 
        get => soulConcentrated; 
        set
        {
            soulConcentrated = value;
            SoulConcentratedChange();
        }
    }

    public float SoulStone { 
        get => soulStone;
        set
        {
            soulStone = value;
            SoulStoneChange();
        }
    }

    public void DeltaSoul(float delta)
    {
        Soul += delta;
        if (Soul <= 0)
            Soul = 0;
    }

    public void SetSoul(float value)
    {
        Soul = value;
    }

    public void DeltaCorruptedSoul(float delta)
    {
        if (delta > 0)
        {
            delta *= (1 + DungeonCore.Instance.CorruptedSoulGain/100);
        }
        CorruptedSoul += delta;
        if (CorruptedSoul <= 0)
            CorruptedSoul = 0;
    }

    public void SetCorruptedSoul(float value)
    {
        CorruptedSoul = value;
    }

    public void DeltaSoulConcentrated(float delta)
    {
        SoulConcentrated += delta;
        if (soulConcentrated <= 0)
            SoulConcentrated = 0;
    }


    public void SetSoulConcentrated(float value)
    {
        SoulConcentrated = value;
    }

    public void SetSoulStone(float value)
    {
        SoulStone = value;
    }

    public void DeltaSoulStone(float delta)
    {
        SoulStone += delta;
        if (soulStone <= 0)
            SoulStone = 0;
    }
}
