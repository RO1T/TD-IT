using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private GameObject[] tilePrefabs;
    [SerializeField]
    private CameraMovement cameraMovement;
    [SerializeField]
    private Transform map;
    [SerializeField]
    private string lvlName;
    public EnemyBase EnemyBase { get; set; }
    public FriendlyBase FriendlyBase { get; set; }

    private Point baseSpawnPoint, enemyBaseSpawnPoint;

    [SerializeField]
    private GameObject basePrefab, enemyBasePrefab;

    public Dictionary<Point,TileScript> Tiles { get; set; }
    public float TileSize
    {
        get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }
    void Start()
    {
        CreateLevel();
    }

    void CreateLevel()
    {
        Tiles = new Dictionary<Point, TileScript>();
        string[] mapData = ReadLevelText();
        int mapX = mapData[0].ToCharArray().Length;
        int mapY = mapData.Length;
        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
        for (int y = 0; y < mapY; y++)
        {
            char[] newTiles = mapData[y].ToCharArray();

            for (int x = 0; x < mapX; x++)
            {
                PlaceTile(newTiles[x].ToString(), x, y, worldStart);
            }
        }
        var maxTile = Tiles[new Point(mapX - 1, mapY - 1)].transform.position;
        cameraMovement.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y - TileSize, -10));
        SpawnBases();
    }

    public List<TileScript> GetWaypoints()
    {
        List<TileScript> wayPoints = new List<TileScript>();
        foreach (var waypoint in Tiles)
        {
            if (IsCorner(waypoint.Value.TileIndex))
            {
                wayPoints.Add(waypoint.Value);
            }
        }
        wayPoints.Add(Tiles[new Point(13, 7)]);
        return wayPoints;
    }

    private bool IsCorner(int tileIndex)
    {
        return tileIndex == 2 || tileIndex == 4 || tileIndex == 3 || tileIndex == 6;
    }

    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart)
    {
        int tileIndex = int.Parse(tileType);
        TileScript newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<TileScript>();
        if (tileType != "0")
        {
            newTile.IsPath = true;
        }
        newTile.Setup(new Point(x, y), new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0), map, tileIndex);
    }

    private string[] ReadLevelText()
    {
        TextAsset bindData = Resources.Load<TextAsset>(lvlName) as TextAsset;
        string data = bindData.text.Replace(Environment.NewLine, string.Empty);
        return data.Split('-');
    }

    private void SpawnBases()
    {
        baseSpawnPoint = new Point(13, 7);
        GameObject friendlyTmp = Instantiate(basePrefab, Tiles[baseSpawnPoint].transform.position, Quaternion.identity);
        FriendlyBase = friendlyTmp.GetComponent<FriendlyBase>();
        FriendlyBase.name = "FriendlyBase";

        enemyBaseSpawnPoint = new Point(0, 1);
        GameObject enemyTmp = Instantiate(enemyBasePrefab, Tiles[enemyBaseSpawnPoint].transform.position, Quaternion.identity);
        EnemyBase = enemyTmp.GetComponent<EnemyBase>();
        EnemyBase.name = "EnemyBase";
    }
}
