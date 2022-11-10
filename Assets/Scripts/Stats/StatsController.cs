using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsController
{
    public List<Stat> Stats;

    public List<Stat> SummedStats;

    public Unit MyUnit;

    StatsController()
    {
        Initialize();
    }

    ~StatsController()
    {
        Destroy();
    }

    public void Initialize()
    {
        Stats = new List<Stat>();
        SummedStats = new List<Stat>();
    }

    public void Destroy()
    {
        Stats.Clear();
        SummedStats.Clear();
    }

    public void SumStats()
    {
        for(int i = 0; i < (int)Stat.StatTypes.Last; i++)
        {

        }
    }
}
