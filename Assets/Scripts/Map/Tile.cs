using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile : MonoBehaviour
{
    public enum TileType
    {
        None,
        DeepOcean,
        ShallowWater,
        Whirlpool,
        BubblingSea,
        SteamySea,
        Desert,
        Grassland,
        Plain,
        Swamp,
        River,
        Forest,
        Hill,
        Mountain,
        Tundra
    }

    public enum Overlay
    {
        None,
        Cave,
        Dungeon,
        WizardTower,
        LeyLine,
    }

    public enum MovementType
    {
        None,
        Clear,
        Rough,
        Mountain,
        Water,
        Turbulent
    }

    public TileType Type = TileType.None;
    public Overlay Feature = Overlay.None;
    public MovementType Movement = MovementType.None;

    public float MinimumElevation = 0f;
    public float MaximumElevation = 0f;

    public GameObject TilePrefab;

    public Vector2 Location = Vector2.zero;

    public float Elevation = 0f;
}
