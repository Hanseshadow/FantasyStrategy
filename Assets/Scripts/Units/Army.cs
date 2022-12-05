using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArmyData
{
    public enum ArmyAlignments
    {
        None,
        Friend,
        Enemy,
        Last
    }

    public int PlayerOwner = 0;

    public int ArmyID = 0;

    public Vector2 MapLocation = Vector2.zero;

    public ArmyAlignments ArmyAlignment;
}

public class Army : MonoBehaviour
{
    public ArmyData Data;

    public ArmyPortrait Portrait;

    public GameObject ArmyOrientation;
}
