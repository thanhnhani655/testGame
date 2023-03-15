using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Facility : ICloneable
{
    public Sprite sprite;
    public string id;
    public string facilityName;
    public string description;
    public float value;
    public float valueInscreasePerLevel = 1;
    public float soulPrice;
    public float corruptedSoulPrice;
    public float concentratedSoulPrice;

    public Room address;

    public Level level;

    public bool isLevelMax;
    public Facility CloneFacility()
    {
        Facility newFac = (Facility)this.Clone();
        newFac.level = this.level.CloneLevel();

        return newFac;
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public void LevelUp()
    {
        level.LevelUp();
        value += valueInscreasePerLevel;
    }
}
