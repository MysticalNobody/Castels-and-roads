using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    List<GameObject> tiles = new List<GameObject>();
    [SerializeField]
    private int points = 0;
    public int Points {
        get {
            return points;
        }
    }
    void Start() {

    }

    void Update() {
    }
    public void AddTile(GameObject tile) {
        tiles.Add(tile);
    }
    public void UpdatePoints() {
        points = 0;
        if (tiles.Count > 0) {
            for (int i = 0; i < tiles.Count; i++) {
                if (!tiles[i].GetComponent<TileManager>().House.GetComponent<Building>().IsCastleNear) {
                    points += PointsPerTile(tiles[i].GetComponent<TileManager>().House.GetComponent<Building>().Type,
                                            tiles[i].GetComponent<TileManager>().House.GetComponent<Building>().Level);
                }
            }
        }
    }
    public void UpdateTileColor() {
        Color mColor = new Color();
        if (tiles.Count > 0) {
            for (int i = 0; i < tiles.Count; i++) {
                if (tiles[i].GetComponent<TileManager>().owner.Equals(GameController.players[0])) {
                    ColorUtility.TryParseHtmlString("#FDC7C7FF", out mColor);
                    tiles[i].GetComponent<SpriteRenderer>().color = mColor;
                } else if (tiles[i].GetComponent<TileManager>().owner.Equals(GameController.players[1])) {
                    ColorUtility.TryParseHtmlString("#D0FDC7FF", out mColor); 
                     tiles[i].GetComponent<SpriteRenderer>().color = mColor;
                }
            }
        }
    }
    int PointsPerTile(int type, int level) {
        switch (type) {
            case 0:
                return 5 + level;
            case 1:
                return 2 * level;
            case 2:
                if (level == 5) {
                    return 50;
                }
                break;
            case 3:
            case 4:
                if (level == 1) {
                    return 8;
                }
                break;
            default:
                break;
        }
        return 0;
    }
}
