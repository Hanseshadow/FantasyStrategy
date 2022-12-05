using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class UnitData
{
    // Do not reorganize this enum, after a building is defined.
    public enum UnitClass
    {
        None,
        BlueDragon,
        OrcArmy,
    }

    public UnitClass UnitType;
    public GameObject UnitPrefab;

    public int UnitCost;
    public List<Stat> Stats;

    public Building.BuildingClass Prerequisite;
}
