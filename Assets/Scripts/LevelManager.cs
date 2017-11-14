using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    [SerializeField]
    private int[,] map;
    [SerializeField]
    public GameObject[] tilePrefabs;
    [SerializeField]
    private CamMove camMove;
    public int MapSizeX {
        get { return map.GetLength(0); }
    }
    public int MapSizeY {
        get { return map.GetLength(0); }
    }
    LevelManager()
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
    }
// Use this for initialization
void Start() {
    CreateLevel();
    camMove.SetMax(map.GetLength(1) * TileManager.TileSize - Camera.main.ViewportToWorldPoint(new Vector3(1, 0)).x * 2, map.GetLength(0) * TileManager.TileSize - Camera.main.ViewportToWorldPoint(new Vector3(0, 1)).y * 2);
}

// Update is called once per frame
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
    GameObject newTile = Instantiate(tilePrefabs[type]);
    newTile.transform.position = new Vector2(initPos.x + (x * TileManager.TileSize), initPos.y + (y * TileManager.TileSize));
    newTile.name = x + "_" + y;
    //newTile.GetComponent<BoxCollider>().size = new Vector3(TileManager.tileBoundSize, TileManager.tileBoundSize, 0.2f);
}

}
