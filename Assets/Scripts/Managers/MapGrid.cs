using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class MapData
{
    public MapGrid.MapSize CurrentSize = MapGrid.MapSize.Small;
    public MapGrid.LandmassSize LandmassSize = MapGrid.LandmassSize.Small;
    public List<Tile> Tiles;
    public int MapHeight = 0;
    public int MapWidth = 0;
}

public class MapGrid : MonoBehaviour
{
    public enum MapSize
    {
        Small,
        Medium,
        Large
    }

    public MapSize CurrentSize = MapSize.Small;

    public enum LandmassSize
    {
        Small = 30,
        Medium = 10,
        Large = 8,
        Huge = 6
    }

    public List<Vector2> YIsEven = new List<Vector2>() { new Vector2(-1, 1) /* NW */, new Vector2(0, 2) /* N */, new Vector2(0, 1) /* NE */,
        new Vector2(-1, -1) /* SW */, new Vector2(0, -2) /* S */, new Vector2(0, -1) /* SE */
    };

    public List<Vector2> YIsOdd  = new List<Vector2>() { new Vector2( 0, 1) /* NW */, new Vector2(0, 2) /* N */, new Vector2(1, 1) /* NE */,
        new Vector2( 0, -1) /* SW */, new Vector2(0, -2) /* S */, new Vector2(1, -1) /* SE */
    };

public LandmassSize IslandSize = LandmassSize.Small;

    List<Vector2> Sizes;

    public bool Resize = false;

    public List<GameObject> TileTypes = new List<GameObject>();

    List<Tile> Tiles;
    List<Tile> FakeTiles;

    public GameObject FakeTilesParent;

    private List<Tile> AllTileTypes = new List<Tile>();

    public float Elevation = 0.6f;
    public int Passes = 2;
    public float NextSize = 2f;

    public bool IsGenerating = false;

    public Camera MainCamera;

    public GameObject GoldHexSelectionPrefab;

    public GameObject GoldHexSelection;
    public GameObject FakeGoldHexSelection;

    private int MapHeight = 0;
    private int MapWidth = 0;
    private Vector3 MainOffset;
    private Vector3 AlternateOffset;

    private Vector3 SelectionOffset = new Vector3(0, 0.1f, 0);

    private GameManager GM;

    void Start()
    {
    }

    public void BeginGeneration(GameManager gm)
    {
        if(gm != null)
            GM = gm;

        Initialize();
        StartCoroutine(Generate());
    }

    void Update()
    {
        if(Resize)
        {
            ClearTiles();
            StartCoroutine(Generate());
            Resize = false;
        }
    }

    private void Initialize()
    {
        Sizes = new List<Vector2>()
        {
            new Vector2(100, 100),
            new Vector2(200, 200),
            new Vector2(300, 300)
        };

        MainOffset = new Vector3(15f, 0f, 5f);
        AlternateOffset = new Vector3(7.5f, 0f, 0f);

        Tiles = new List<Tile>();
        FakeTiles = new List<Tile>();

        // Initialize what types of tiles can be placed
        foreach(GameObject go in TileTypes)
        {
            if(go == null)
                continue;

            Tile t = go.GetComponent<Tile>();

            if(t == null)
                continue;

            AllTileTypes.Add(t);
        }
    }

    private void ClearTiles()
    {
        foreach(Tile tile in Tiles)
        {
            if(tile != null && tile.gameObject != null)
                Destroy(tile.gameObject);
        }

        foreach(Tile tile in FakeTiles)
        {
            if(tile != null && tile.gameObject != null)
                Destroy(tile.gameObject);
        }

        Tiles.Clear();
        FakeTiles.Clear();
    }

    private void ResetCamera()
    {
        if(MainCamera == null)
            return;

        float cameraX = ((float)MapWidth * MainOffset.x) / 2f;
        float cameraZ = ((float)MapHeight * MainOffset.z) / 2f;

        MainCamera.transform.position = new Vector3(cameraX, MainCamera.transform.position.y, cameraZ);

        // Debug.Log("Main camera position: " + MainCamera.transform.position + " x: " + cameraX + " z: " + cameraZ);
    }

    List<Tile> GetPossibleTiles(float height)
    {
        return AllTileTypes.FindAll(x => x.MinimumElevation <= height && x.MaximumElevation >= height);
    }

    List<Tile> GetPossibleTiles(Tile.TileType type)
    {
        List<Tile> tiles = new List<Tile>();

        foreach(Tile tile in AllTileTypes)
        {
            if(tile.Type == type)
                tiles.Add(tile);
        }

        return tiles;
        //return AllTileTypes.FindAll(x => x.Type == type);
    }

    private IEnumerator Generate()
    {
        IsGenerating = true;

        Debug.Log("Map generating.");

        int frames = 0;

        MapWidth = (int)Sizes[(int)CurrentSize].x;
        MapHeight = (int)Sizes[(int)CurrentSize].y;

        GameObject tilePrefab;
        GameObject newTile;
        Tile tile = null;

        List<List<float>> tileMap = new List<List<float>>();

        for(int i = 0; i < MapHeight; i++)
        {
            tileMap.Add(new List<float>());

            for(int j = 0; j < MapWidth; j++)
                tileMap[i].Add(0);
        }

        // TODO: Need a sea pass and then an island/continent pass
        float originX = Random.Range(0, 10000000);
        float originY = Random.Range(0, 10000000);

        float scale = (float)IslandSize / 2f; // / (h + 1) * Division;

        if(scale < 10f)
            scale = 10f;

        for(int i = 0; i < MapWidth; i++)
        {
            for(int j = 0; j < MapHeight; j++)
            {
                float elevation = Mathf.PerlinNoise((originX + (float)i) / (float)MapWidth * 3 * scale, (originY + (float)j) / (float)MapHeight * scale);

                if(tileMap[i][j] < Elevation)
                    tileMap[i][j] = elevation;
            }
        }

        for(int h = 0; h < Passes; h++)
        {
            originX = Random.Range(0, 10000000);
            originY = Random.Range(0, 10000000);

            scale = (float)IslandSize - (NextSize * h); // / (h + 1) * Division;

            for(int i = 0; i < MapWidth; i++)
            {
                for(int j = 0; j < MapHeight; j++)
                {
                    float elevation = Mathf.PerlinNoise((originX + (float)i) / (float)MapWidth * 3 * scale, (originY + (float)j) / (float)MapHeight * scale);

                    if(tileMap[i][j] < elevation && tileMap[i][j] >= Elevation)
                        tileMap[i][j] = elevation;
                }
            }
        }

        int passes = 0;

        // Place tiles
        for(int i = 0; i < MapWidth; i++)
        {
            for(int j = 0; j < MapHeight; j++)
            {
                tilePrefab = null;

                List<Tile> tilesForHeight = GetPossibleTiles(tileMap[i][j]);

                //Debug.Log(tileMap[i][j]);

                if(tilesForHeight != null && tilesForHeight.Count > 0)
                    tilePrefab = tilesForHeight[Random.Range(0, tilesForHeight.Count)].TilePrefab;

                if(tilePrefab == null)
                    continue;

                newTile = Instantiate(tilePrefab);

                newTile.transform.position = new Vector3(MainOffset.x * i, 0f, MainOffset.z * j) + (j % 2 == 0 ? Vector3.zero : AlternateOffset);
                newTile.transform.parent = this.transform;

                tile = newTile.GetComponent<Tile>();

                if(tile != null)
                {
                    tile.Location = new Vector2(i, j);
                    tile.Elevation = tileMap[i][j];
                }

                Tiles.Add(tile);

                passes++;

                if(passes > 1000)
                {
                    passes = 0;
                    frames++;
                    yield return null;
                }
            }
        }

        int tileIndex = 0;

        passes = 0;
        
        List<Tile> possibleTiles = new List<Tile>();

        // Fix shallow water
        while(tileIndex < Tiles.Count)
        {
            tile = Tiles[tileIndex];

            if(tile == null)
            {
                Tiles.RemoveAt(tileIndex);
                continue;
            }

            // Orphan shallow water removal
            if(!IsAdjacentToLand(tile) && tile.Type == Tile.TileType.ShallowWater)
            {
                possibleTiles = GetPossibleTiles(1);

                if(possibleTiles.Count > 0)
                {
                    tilePrefab = possibleTiles[Random.Range(0, possibleTiles.Count)].TilePrefab;
                }
                else
                {
                    continue;
                }

                newTile = Instantiate(tilePrefab);

                newTile.transform.position = new Vector3(MainOffset.x * tile.Location.x, 0f, MainOffset.z * tile.Location.y) + (tile.Location.y % 2 == 0 ? Vector3.zero : AlternateOffset);
                newTile.transform.parent = this.transform;

                newTile.GetComponent<Tile>().Location = new Vector2(tile.Location.x, tile.Location.y);
                newTile.GetComponent<Tile>().Elevation = 1;

                Tiles.Remove(tile);
                Tiles.Add(newTile.GetComponent<Tile>());

                Destroy(tile.gameObject);

                passes++;

                if(passes > 1000)
                {
                    Debug.Log("Fix shallow water orphan at: " + tile.Location);

                    passes = 0;
                    frames++;
                    yield return null;
                }
            }

            tileIndex++;
        }

        passes = 0;
        tileIndex = 0;

        // Fix shallow water
        while(tileIndex < Tiles.Count)
        {
            tile = Tiles[tileIndex];

            if(tile == null)
            {
                Tiles.RemoveAt(tileIndex);
                Debug.Log("Fix missing shallow water: Null tile found.");
                continue;
            }

            // Coastal shallow water adding
            if(IsAdjacentToLand(tile) && tile.IsWater() && tile.Type != Tile.TileType.ShallowWater)
            {
                possibleTiles = GetPossibleTiles(Tile.TileType.ShallowWater);

                if(possibleTiles.Count > 0)
                {
                    tilePrefab = possibleTiles[Random.Range(0, possibleTiles.Count)].TilePrefab;
                }
                else
                {
                    Debug.Log("Fix missing shallow water: No tile replacement at: " + tile.Location);
                    continue;
                }

                newTile = Instantiate(tilePrefab);

                if(newTile == null)
                {
                    Debug.Log("Fix missing shallow water: No replacement tile at: " + tile.Location);
                }

                newTile.transform.position = new Vector3(MainOffset.x * tile.Location.x, 0f, MainOffset.z * tile.Location.y) + (tile.Location.y % 2 == 0 ? Vector3.zero : AlternateOffset);
                newTile.transform.parent = this.transform;

                newTile.GetComponent<Tile>().Location = new Vector2(tile.Location.x, tile.Location.y);
                newTile.GetComponent<Tile>().Elevation = newTile.GetComponent<Tile>().MinimumElevation;

                Tiles.Remove(tile);
                Tiles.Add(newTile.GetComponent<Tile>());

                Destroy(tile.gameObject);

                passes++;

                if(passes > 1000)
                {
                    Debug.Log("Fix shallow water missing at: " + tile.Location);

                    passes = 0;
                    frames++;
                    yield return null;
                }
            }
            else if(IsAdjacentToLand(tile) && tile.IsWater() && tile.Type != Tile.TileType.ShallowWater)
            {
                Debug.Log("Deep water tile replaced at: " + tile.Location);
            }

            tileIndex++;
        }

        Debug.Log("Main map frames: " + frames);

        ///////
        // Fake map edges
        ///////

        // Left side

        passes = 0;

        Debug.Log("Left Side");

        for(int i = MapWidth - 1; i > MapWidth - 20; i--)
            for(int j = 0; j < MapHeight; j++)
            {
                tile = Tiles.Find(x => x.Location.x == i && x.Location.y == j);

                if(tile == null)
                    continue;

                newTile = Instantiate(tile.TilePrefab);

                newTile.transform.position = new Vector3(MainOffset.x * (i - MapWidth), 0f, MainOffset.z * j) + (j % 2 == 0 ? Vector3.zero : AlternateOffset);
                newTile.transform.parent = FakeTilesParent.transform;

                tile = newTile.GetComponent<Tile>();

                if(tile != null)
                {
                    tile.Location = new Vector2(i, j);
                    tile.Elevation = tileMap[i][j];
                    tile.IsFakeTile = true;
                    FakeTiles.Add(tile);
                }

                passes++;

                if(passes > 1000)
                {
                    passes = 0;
                    yield return null;
                }
            }

        passes = 0;

        Debug.Log("Right Side");

        // Right side
        for(int i = 0; i < 20; i++)
            for(int j = 0; j < MapHeight; j++)
            {
                tile = Tiles.Find(x => x.Location.x == i && x.Location.y == j);

                if(tile == null)
                    continue;

                newTile = Instantiate(tile.TilePrefab);

                newTile.transform.position = new Vector3(MainOffset.x * (i + MapWidth), 0f, MainOffset.z * j) + (j % 2 == 0 ? Vector3.zero : AlternateOffset);
                newTile.transform.parent = FakeTilesParent.transform;

                tile = newTile.GetComponent<Tile>();

                if(tile != null)
                {
                    tile.Location = new Vector2(i, j);
                    tile.Elevation = tileMap[i][j];
                    tile.IsFakeTile = true;
                    FakeTiles.Add(tile);
                }

                passes++;

                if(passes > 1000)
                {
                    passes = 0;
                    yield return null;
                }
            }

        IsGenerating = false;

        ResetCamera();

        if(GM != null)
        {
            GM.LoadingFinished();
        }
    }

    public void SetMapSize(string size)
    {
        switch(size)
        {
            case "small":
                CurrentSize = MapSize.Small;
                break;
            case "medium":
                CurrentSize = MapSize.Medium;
                break;
            case "large":
                CurrentSize = MapSize.Large;
                break;
        }
    }

    public void SetLandmassSize(string size)
    {
        switch(size)
        {
            case "small":
                IslandSize = LandmassSize.Small;
                break;
            case "medium":
                IslandSize = LandmassSize.Medium;
                break;
            case "large":
                IslandSize = LandmassSize.Large;
                break;
            case "huge":
                IslandSize = LandmassSize.Huge;
                break;
        }
    }

    public bool IsAdjacentToLand(Tile tile)
    {
        // There could be a floating point error here.
        int x = (int)tile.Location.x;
        int y = (int)tile.Location.y;

        List<Vector2> locationsByY = (y % 2 == 0 ? YIsEven : YIsOdd);

        for(int i = 0; i < locationsByY.Count; i++)
        {
            Tile tileToCheck = GetTileAtLocation(tile.Location.x + locationsByY[i].x, tile.Location.y + locationsByY[i].y);

            if(tileToCheck == null)
                continue;

            if(!tileToCheck.IsWater())
            {
                return true;
            }
        }

        return false;
    }

    public Tile GetTileAtLocation(float x, float y)
    {
        return Tiles.Find(a => (int)a.Location.x == (int)x && (int)a.Location.y == (int)y);
    }

    public Vector2 GetMapSizeInMeters()
    {
        return new Vector2((float)MapWidth * MainOffset.x, (float)MapHeight * MainOffset.z);
    }

    public void SelectTile(Tile tile)
    {
        if(tile == null)
            return;

        if(GoldHexSelection == null && GoldHexSelectionPrefab != null)
        {
            GoldHexSelection = Instantiate(GoldHexSelectionPrefab);
        }

        GoldHexSelection.transform.position = tile.transform.position + SelectionOffset;

        GM.Armies.CreateUnit(tile);
    }
}
