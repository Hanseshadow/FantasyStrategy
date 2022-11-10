using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    // All stat types.  This enum cannot be added to, if a player has a saved game.
    public enum StatTypes
    {
        Strength,
        Stamina,
        Agility,
        Dexerity,
        Intelligence,
        Charisma,
        Luck,
        PhysicalResist,
        FireResist,
        ColdResist,
        PoisonResist,
        ChaosResist,
        LifeResist,
        Last
    }

    public StatTypes StatType;
    public int Value;
}
