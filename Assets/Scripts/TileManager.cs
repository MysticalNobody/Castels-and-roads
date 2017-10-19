using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {
    [SerializeField]
    private bool[] roadsAround = new bool[4];
    [SerializeField]
    private bool active= false;
    public void UpdateTileState(int pos) {
        roadsAround[pos] = true;
        active = CheckTileState();
    }
    private bool CheckTileState() {
        for (int i = 0; i < roadsAround.Length; i++) {
            if (!roadsAround[i]) {
                return false;
            }
        }
        return true;
    }
    public bool CheckRoad(int pos) {
        return roadsAround[pos];
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (active) {
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
        }
    }
}
