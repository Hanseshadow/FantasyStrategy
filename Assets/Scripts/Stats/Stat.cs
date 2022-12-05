using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    // All stat types.  This enum cannot be added to, if a player has a saved game.
    public enum StatTypes
    {
        HitPoints,
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
        Moves,
        Poisoned,
        Last
    }

    public StatTypes StatType;
    public int Value;
    public int ModifiedValue;
    public bool Modifyable = false;
    public bool ResetsPerTurn = false;

    public void TurnReset()
    {
        if(Modifyable && ResetsPerTurn)
            ModifiedValue = Value;
    }
}
