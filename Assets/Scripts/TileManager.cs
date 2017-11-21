using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {
    public const float TileSize = 2.048f;
    private GameObject[] roadsAround; // 0 - left, 1 - top, 2 - right, 3 - bottom
    private bool active;
    public GameObject owner;
    private GameObject house;
    public bool Active {
        get {
            return active;
        }

        set {
            active = value;
        }
    }
    public GameObject[] RoadsAround {
        get {
            return roadsAround;
        }

        set {
            roadsAround = value;
        }
    }
    public GameObject House {
        get {
            return house;
        }

        set {
            house = value;
        }
    }

    public TileManager() {
        RoadsAround = new GameObject[4] { null, null, null, null };
        House = null;
        Active = false;
    }

    void Start() {
    }
    void Update() {
        if (CheckTileState() && !Active) {
            Building.CreateBuilding(gameObject, GameController.chosenBuilding);
            Active = true;
        }
    }

    public bool CheckRoad(int pos) {
        return RoadsAround[pos];
    }
    public bool CheckTileState() {
        for (int i = 0; i < 4; i++) {
            if (!RoadsAround[i]) {
                return false;
            }
        }
        return true;
    }

}
