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

    public bool IsFakeTile = false;

    private List<MeshRenderer> Renderers = new List<MeshRenderer>();

    private void Start()
    {
        Renderers.Clear();

        Renderers.Add(this.gameObject.GetComponent<MeshRenderer>());
    }

    public Tile CopyTo(GameObject newObject)
    {
        Tile newTile;

        if(newObject.GetComponent<Tile>() == null)
            newTile = newObject.AddComponent<Tile>();
        else
            newTile = newObject.GetComponent<Tile>();

        newTile.Type = Type;
        newTile.Feature = Feature;
        newTile.Movement = Movement;
        newTile.MinimumElevation = MinimumElevation;
        newTile.MaximumElevation = MaximumElevation;
        newTile.TilePrefab = TilePrefab;
        newTile.Location = Location;
        newTile.Elevation = Elevation;
        newTile.IsFakeTile = IsFakeTile;

        return newTile;
    }

    public bool IsWater()
    {
        switch(Type)
        {
            case TileType.DeepOcean:
            case TileType.ShallowWater:
            case TileType.Whirlpool:
            case TileType.BubblingSea:
            case TileType.SteamySea:
                return true;
        }

        return false;
    }

    public bool IsAdjacentToLand()
    {
        MapGrid mg = MapGrid.Instance;

        Debug.Log("Is next to land? " + mg.IsAdjacentToLand(this));

        return false;
    }

    public void SelectTile()
    {
        IsAdjacentToLand();
    }

    public void HideTile()
    {
        if(Renderers.Count <= 0)
            return;

        for(int i = 0; i < Renderers.Count; i++)
        {
            Renderers[i].enabled = false;
        }
    }

    public void ShowTile()
    {
        if(Renderers.Count <= 0)
            return;

        for(int i = 0; i < Renderers.Count; i++)
        {
            Renderers[i].enabled = true;
        }
    }
}
