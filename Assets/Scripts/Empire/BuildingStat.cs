using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingStat
{
    public enum StatType
    {
        Unknown,
        Food,
        Mana,
        Defense,
        Attack,
        HitPoints,
        Warrior,
        Archer,
        Knight,
        Wyrm,
        Sorcerer,
    }

    public StatType Stat;
    public int Value;
}
