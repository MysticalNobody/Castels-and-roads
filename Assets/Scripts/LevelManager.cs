using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static GameObject[] TilePrefabs;
    private static int[,] map;

    public static int MapSizeX {
        get { return map.GetLength(1); }
    }
    public static int MapSizeY {
        get { return map.GetLength(0); }
    }

    static LevelManager()
    {
        TilePrefabs = new GameObject[2];
    }

    void Awake()
    {
        for (int i = 1; i <= 2; i++)
            TilePrefabs[i - 1] = (GameObject)Resources.Load("tayl" + i);
    }
    void Start()
    {
        GenerateMap();
        CreateLevel();
        PrepareMinimap();
        CamMove.SetMax(map.GetLength(1) * TileManager.TileSize - Camera.main.ViewportToWorldPoint(new Vector3(1, 0)).x * 2, map.GetLength(0) * TileManager.TileSize - Camera.main.ViewportToWorldPoint(new Vector3(0, 1)).y * 2);
    }
    void Update()
    {

    }
    void GenerateMap()
    {
        map = new int[Random.Range(10, 25), Random.Range(10, 25)];
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (x == 0 || y == 0)
                {
                    map[x, y] = 1;
                }
                else if (x == map.GetLength(0) - 1 || y == map.GetLength(1) - 1)
                {
                    map[x, y] = 1;
                }
                else if (Random.Range(0, map.GetLength(0) / 2) < Mathf.Abs(x - map.GetLength(0) / 2) && Random.Range(0, map.GetLength(1) / 2) < Mathf.Abs(y - map.GetLength(1) / 2))
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = 0;
                }
            }
        }
    }
    void PrepareMinimap()
    {
        GameObject minmapCam = GameObject.Find("MinimapCamera");
        minmapCam.GetComponent<Camera>().orthographicSize = MapSizeX > MapSizeY ? (MapSizeX * TileManager.TileSize) / 2 + TileManager.TileSize : (MapSizeY * TileManager.TileSize) / 2 + TileManager.TileSize;
        minmapCam.transform.position = new Vector3(GameObject.Find(MapSizeX / 2 + "_" + MapSizeY / 2).transform.position.x,
                                                   GameObject.Find(MapSizeX / 2 + "_" + MapSizeY / 2).transform.position.y,
                                                   minmapCam.transform.position.z);
        GameController.minimap.enabled = false;
    }
    private void CreateLevel()
    {
        Vector2 initPos = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                PlaceTitles(initPos, j, i, map[i, j]);
            }
        }
    }
    private void PlaceTitles(Vector2 initPos, int x, int y, int type)
    {
        GameObject newTile = Instantiate(TilePrefabs[type]);
        newTile.transform.position = new Vector2(initPos.x + (x * TileManager.TileSize), initPos.y + (y * TileManager.TileSize));
        newTile.name = x + "_" + y;
    }

}
