using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SkillType
{
    Attack,
    Heal,
    Buff,
    Passive
}

public enum TargetRange
{
    Single,
    Wide
}

public enum TargetFaction
{
    Allies,
    Enemy,
    Self
}

[Serializable]
public class SkillData : ICloneable
{
    public string skillID;
    public string skillName;
    public string skillDescription;
    public SkillType type;
    public TargetRange range;
    public TargetFaction faction;
    
    public float value1;
    public string buff;

    
    public SkillData CloneSkill()
    {
        return (SkillData)this.Clone();
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}
