using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public const float TileSize = 2.048f;
    [SerializeField]
    private bool[] roadsAround;
    [SerializeField]
    private bool active;
    public static Vector2 roadSize;
    // Use this for initialization
    TileManager()
    {
        roadsAround = new bool[4];
        active = false;
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
        }
    }
    public void UpdateTileState(int pos) { // 0 - left, 1 - up, 2 - right, 3 - bottom
        roadsAround[pos] = true;
        active = CheckTileState();
        Debug.Log(active);
    }
    public static void CreateRoad(int type, int pos, RaycastHit hit, GameObject prefab, GameObject previewRoad)
    {
        GameObject road = Instantiate(prefab);
        road.transform.position = previewRoad.transform.position;
        road.transform.rotation = previewRoad.transform.rotation;
        hit.collider.gameObject.GetComponent<TileManager>().UpdateTileState(pos);
        string[] name = hit.collider.gameObject.name.Split('_');
        int[] nameInt = { int.Parse(name[0]), int.Parse(name[1]) };
        Debug.Log(nameInt[0] + " " + nameInt[1]);
        //Debug.Log(maxY); // ? тут что ?
        switch (pos)
        {
            case 0:
                if ((nameInt[0] - 1) >= 0)
                    GameObject.Find((nameInt[0] - 1) + "_" + nameInt[1]).GetComponent<TileManager>().UpdateTileState(2);
                break;
            case 1:
                if ((nameInt[1] + 1) < GameObject.Find("LevelManager").GetComponent<LevelManager>().MapSizeY)
                    GameObject.Find(nameInt[0] + "_" + (nameInt[1] + 1)).GetComponent<TileManager>().UpdateTileState(3);
                break;
            case 2:
                if ((nameInt[0] + 1) < GameObject.Find("LevelManager").GetComponent<LevelManager>().MapSizeX)
                    GameObject.Find((nameInt[0] + 1) + "_" + nameInt[1]).GetComponent<TileManager>().UpdateTileState(0);
                break;
            case 3:
                if ((nameInt[1] - 1) >= 0)
                    GameObject.Find(nameInt[0] + "_" + (nameInt[1] - 1)).GetComponent<TileManager>().UpdateTileState(1);
                break;
            default:
                break;
        }
    }

    private bool CheckTileState() {
        for (int i = 0; i < roadsAround.Length; i++) {
            if (!roadsAround[i]) {
                return false;
            }
        }
        //GameObject building = Instantiate();
        return true;
    }
    public bool CheckRoad(int pos) {
        return roadsAround[pos];
    }

}
