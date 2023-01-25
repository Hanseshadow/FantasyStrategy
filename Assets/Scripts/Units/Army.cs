using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Save game data
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

// Runtime data
public class Army : MonoBehaviour
{
    public ArmyData Data;

    public ArmyPortrait Portrait;

    public GameObject ArmyOrientation;

    private void Start()
    {
        Canvas canvas = GetComponent<Canvas>();

        // Make sure armies overlap properly on the map.
        if(canvas != null)
            canvas.overrideSorting = true;
    }
}
