using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapData
{
    public MapGrid.MapSize CurrentSize = MapGrid.MapSize.Small;
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

    List<Vector2> Sizes;

    public bool Resize = false;

    public List<GameObject> TileTypes = new List<GameObject>();

    List<Tile> Tiles;
    List<Tile> FakeTiles;

    public GameObject FakeTilesParent;

    private List<Tile> AllTileTypes = new List<Tile>();

    public int PerlinScale = 8;
    public float Elevation = 0.6f;
    public int Passes = 2;
    public float Division = 2f;

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

        Tiles.Clear();
    }

    private void ResetCamera()
    {
        if(MainCamera == null)
            return;

        float cameraX = ((float)MapWidth * MainOffset.x) / 2f;
        float cameraZ = ((float)MapHeight * MainOffset.z) / 2f;

        MainCamera.transform.position = new Vector3(cameraX, MainCamera.transform.position.y, cameraZ);

        Debug.Log("Main camera position: " + MainCamera.transform.position + " x: " + cameraX + " z: " + cameraZ);
    }

    List<Tile> GetPossibleTiles(float height)
    {
        return AllTileTypes.FindAll(x => x.MinimumElevation <= height && x.MaximumElevation >= height);
    }

    private IEnumerator Generate()
    {
        IsGenerating = true;

        MapWidth = (int)Sizes[(int)CurrentSize].x;
        MapHeight = (int)Sizes[(int)CurrentSize].y;

        GameObject tilePrefab;
        GameObject newTile;

        List<List<float>> tileMap = new List<List<float>>();

        for(int i = 0; i < MapHeight; i++)
        {
            tileMap.Add(new List<float>());

            for(int j = 0; j < MapWidth; j++)
                tileMap[i].Add(0);
        }

        for(int h = 0; h < Passes; h++)
        {
            float originX = Random.Range(0, 10000000);
            float originY = Random.Range(0, 10000000);

            float scale = PerlinScale / (h + 1) * Division;

            for(int i = 0; i < MapWidth; i++)
            {
                for(int j = 0; j < MapHeight; j++)
                {
                    float elevation = Mathf.PerlinNoise((originX + (float)i) / (float)MapWidth * 2 * scale, (originY + (float)j) / (float)MapHeight * scale);

                    if(tileMap[i][j] < elevation)
                        tileMap[i][j] = elevation;
                }
            }
        }

        int passes = 0;

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

                Tile tile = newTile.GetComponent<Tile>();

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
                    yield return null;
                }
            }
        }

        ///////
        // Fake map edges
        ///////

        // Left side

        passes = 0;

        Debug.Log("Left Side");

        for(int i = MapWidth - 1; i > MapWidth - 20; i--)
            for(int j = 0; j < MapHeight; j++)
            {
                Tile tile = Tiles.Find(x => x.Location.x == i && x.Location.y == j);

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
                Tile tile = Tiles.Find(x => x.Location.x == i && x.Location.y == j);

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
