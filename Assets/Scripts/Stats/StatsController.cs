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
        List<Stat> statsOfType = new List<Stat>();

        for(int i = 0; i < (int)Stat.StatTypes.Last; i++)
        {
            statsOfType.Clear();

            statsOfType = Stats.FindAll(x => x.StatType == (Stat.StatTypes)i);

            int value = 0;

            foreach(Stat stat in statsOfType)
            {
                if(stat == null)
                    continue;

                if(stat.Modifyable)
                {
                    value += stat.ModifiedValue;
                }
                else
                {
                    value += stat.Value;
                }
            }
        }
    }

    public void TurnReset()
    {
        foreach(Stat stat in Stats)
        {
            if(stat == null)
                continue;

            stat.TurnReset();
        }
    }
}
