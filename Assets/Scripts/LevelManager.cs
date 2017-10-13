using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    [SerializeField]
    private GameObject[] tilePrefabs;
    [SerializeField]
    private CamMove camMove;
    private int[,] map = {
        {0,0,0,0,1,0,0,0,1},
        {1,0,0,0,0,0,0,0,1},
        {0,0,0,0,1,0,0,0,1},
        {0,0,0,0,1,0,0,0,1},
        {1,0,0,0,0,0,0,0,1}
    };

    public float TileSize {
        get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }
    public float[] MapSize {
        get { return MapSize; }
    }
    // Use this for initialization
    void Start() {
        CreateLevel();
        camMove.SetMax(map.GetLength(1) * TileSize - Camera.main.ViewportToWorldPoint(new Vector3(1, 0)).x * 2, map.GetLength(0) * TileSize - Camera.main.ViewportToWorldPoint(new Vector3(0, 1)).y * 2);
        camMove.tileSize = TileSize;
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
        newTile.transform.position = new Vector2(initPos.x + (x * TileSize), initPos.y + (y * TileSize));
    }

}
