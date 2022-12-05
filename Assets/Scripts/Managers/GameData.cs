using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string GameName;
    public List<StrategyPlayer> Players;
    public MapData Map;
    public List<ArmyData> Armies;
    public int Turn;
}
