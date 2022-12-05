using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyManager : MonoBehaviour
{
    public List<Army> Armies;

    public GameObject ArmyBase;

    public UnitsData m_UnitsData;

    private GameManager GM;

    // Start is called before the first frame update
    void Start()
    {
        Armies = new List<Army>();

        GM = FindObjectOfType<GameManager>();
    }

    public bool HasArmy(Tile tile)
    {
        if(tile == null)
            return false;

        return Armies.Find(x => x.Data.MapLocation == tile.Location) != null ? true : false;
    }

    public void CreateUnit(/* UnitData unit,*/ Tile tile)
    {
        // TODO: Remove random units once unit type is selected by the spawner.
        int unitIndex = Random.Range(0, m_UnitsData.UnitList.Count);

        UnitData unit = m_UnitsData.UnitList[unitIndex];

        CreateNewArmy(unit, tile);
    }

    // New Army on an empty tile
    public void CreateNewArmy(UnitData unit, Tile tile)
    {
        if(unit == null || tile == null || ArmyBase == null || m_UnitsData == null || HasArmy(tile))
            return;

        GameObject army = Instantiate(ArmyBase, GM.UnitUI.transform);

        Army newArmy = army.GetComponent<Army>();

        if(newArmy == null)
        {
            Debug.Log("ArmyManager: Bad army file.");
            return;
        }

        GameObject unitPortrait = Instantiate(unit.UnitPrefab, newArmy.ArmyOrientation.transform);

        unitPortrait.transform.parent = newArmy.ArmyOrientation.transform;

        army.transform.position = tile.transform.position;

        Army armyComponent = army.GetComponent<Army>();

        armyComponent.Portrait = unitPortrait.GetComponent<ArmyPortrait>();
        armyComponent.Data.MapLocation = tile.Location;

        // TODO: Remove random army alignment
        armyComponent.Data.ArmyAlignment = (ArmyData.ArmyAlignments)Random.Range(0, (int)ArmyData.ArmyAlignments.Last);

        armyComponent.Portrait.ChangeFrameColor(armyComponent.Data.ArmyAlignment);

        Armies.Add(armyComponent);
    }
}
