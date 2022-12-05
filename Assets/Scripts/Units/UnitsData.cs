using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitsList", menuName = "ScriptableObject/UnitsList", order = 1)]
public class UnitsData : ScriptableObject
{
    public List<UnitData> UnitList;
}
