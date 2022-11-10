using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffController
{
    public List<Buff> Buffs;

    public Unit MyUnit;

    BuffController()
    {
        Initialize();
    }

    ~BuffController()
    {
        Destroy();
    }

    public void Initialize()
    {
        Buffs = new List<Buff>();
    }

    void Destroy()
    {
        Buffs.Clear();
    }

    public void AddBuff(Buff buff)
    {

    }

    public void ExpireBuff(Buff buff)
    {

    }

    public void CheckBuffTimers()
    {
        // Check initialization of a buff versus the current turn and expire overdue buffs
        foreach(Buff b in Buffs)
        {

        }
    }

    public void SumBuffStats(Stat.StatTypes type)
    {

    }
}
