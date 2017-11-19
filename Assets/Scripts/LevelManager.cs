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
        map = new int[,]{
        { 0,0,0,0,1,0,0,0,1,0,0,0,0,1,0,0,0,1},
        { 1,0,0,0,0,0,0,0,1,0,0,0,0,1,0,0,0,1},
        { 0,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,1,0},
        { 0,0,0,0,1,0,0,0,1,0,0,0,1,0,0,0,1,0},
        { 1,0,0,0,0,0,0,0,1,1,0,0,0,1,0,0,0,1},
        { 1,0,0,0,0,0,0,0,1,0,0,0,0,1,0,0,0,1},
        { 1,0,0,0,0,0,0,0,1,1,0,0,0,1,0,0,0,1}
        };
        TilePrefabs = new GameObject[2];
    }

    void Awake()
    {
        for (int i = 1; i <= 2; i++)
            TilePrefabs[i - 1] = (GameObject)Resources.Load("tayl" + i);
    }
    void Start()
    {
        CreateLevel();
        CamMove.SetMax(map.GetLength(1) * TileManager.TileSize - Camera.main.ViewportToWorldPoint(new Vector3(1, 0)).x * 2, map.GetLength(0) * TileManager.TileSize - Camera.main.ViewportToWorldPoint(new Vector3(0, 1)).y * 2);
    }
    void Update() {
    }

    private void CreateLevel() {
        Vector2 initPos = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        for (int i = 0; i < map.GetLength(0); i++) {
            for (int j = 0; j < map.GetLength(1); j++) {
                PlaceTitles(initPos, j, i, map[i, j]);
            }
        }
    }
    private void PlaceTitles(Vector2 initPos, int x, int y, int type) {
    GameObject newTile = Instantiate(TilePrefabs[type]);
    newTile.transform.position = new Vector2(initPos.x + (x * TileManager.TileSize), initPos.y + (y * TileManager.TileSize));
    newTile.name = x + "_" + y;
}

}
