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
        Lair,
        Nest,
        Road,
        Cursed,
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
    public List<Overlay> Features = new List<Overlay>();
    public MovementType Movement = MovementType.None;

    public float MinimumElevation = 0f;
    public float MaximumElevation = 0f;

    public GameObject TilePrefab;

    public Vector2 Location = Vector2.zero;

    public float Elevation = 0f;

    public bool IsFakeTile = false;

    public Tile LinkedTile = null;

    private List<MeshRenderer> Renderers = new List<MeshRenderer>();

    private List<Unit> Units = new List<Unit>();

    public bool RenderersEnabled = false;

    private void Awake()
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
        newTile.Features = Features;
        newTile.Movement = Movement;
        newTile.MinimumElevation = MinimumElevation;
        newTile.MaximumElevation = MaximumElevation;
        newTile.TilePrefab = TilePrefab;
        newTile.Location = Location;
        newTile.Elevation = Elevation;
        newTile.IsFakeTile = IsFakeTile;
        newTile.LinkedTile = LinkedTile;

        return newTile;
    }

    public float GetMovementCost()
    {
        float cost = 0f;

        if(Features.Contains(Overlay.Road))
            cost += -0.5f;

        if(Features.Contains(Overlay.Cursed))
            cost += 2f;

        switch(Movement)
        {
            case MovementType.None:
                return 0f + cost;
            case MovementType.Clear:
                return 1f + cost;
            case MovementType.Rough:
                return 2f + cost;
            case MovementType.Mountain:
                return 5f + cost;
            case MovementType.Water:
                return 1f + cost;
            case MovementType.Turbulent:
                return 2f + cost;
            default:
                return 1f + cost;
        }
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
        Debug.Log("Is next to land? " + MapGrid.Instance.IsAdjacentToLand(this));

        return false;
    }

    public void SelectTile()
    {
        IsAdjacentToLand();
    }

    public bool IsHidden()
    {
        if(Renderers.Count <= 0)
            return true;

        for(int i = 0; i < Renderers.Count; i++)
        {
            if(Renderers[i].enabled)
                return true;
            else
                return false;
        }

        return false;
    }

    public void HideTile()
    {
        if(Renderers.Count <= 0)
            return;

        for(int i = 0; i < Renderers.Count; i++)
        {
            Renderers[i].enabled = false;
        }

        RenderersEnabled = false;

        if(LinkedTile != null && LinkedTile.IsFakeTile && LinkedTile != this && !LinkedTile.IsHidden())
            LinkedTile.HideTile();
    }

    public void ShowTile()
    {
        if(Renderers.Count <= 0)
            return;

        for(int i = 0; i < Renderers.Count; i++)
        {
            Renderers[i].enabled = true;
        }

        RenderersEnabled = true;

        if(LinkedTile != null && LinkedTile.IsFakeTile && LinkedTile != this && LinkedTile.IsHidden())
            LinkedTile.ShowTile();
    }
}
