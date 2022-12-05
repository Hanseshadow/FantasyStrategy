using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Building
{
    // Do not reorganize this enum, after a building is defined.
    public enum BuildingClass
    {
        None,
        Garden,
        Shrine,
        Market,
        Bank,
        Mint,
        GoldMine,
        Bakery,
        Butchery,
        Warehouse,
        Port,
        Shipyard,
        PirateCove,
        Oceanarium,
        KrakenGrotto,
        Arena,
        Fairground,
        Armory,
        Barrack,
        Paddock,
        Stable,
        JoustingLists,
        Library,
        AlchemyLab,
        Sorcerarium,
        WizardTower,
        Cemetary,
        Church,
        Crypt,
        Falconry,
        Eyrie,
        DragonSpire,
        Wall,
        Castle,
        ManaShield,
    }

    public BuildingClass BuildingType;

    public int BuildingCost;

    public List<BuildingStat> Stats;

    public BuildingClass Prerequisite;
}

[CreateAssetMenu(fileName = "Buildings", menuName = "ScriptableObject/Buildings", order = 1)]
public class Buildings : ScriptableObject
{
    public List<Building> BuildingList;
}
