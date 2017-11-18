using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour {
    public static GameObject RoadPrefab;
    public static GameObject PreviewRoad;
    public static Vector2 RoadSize;
    
    void Awake()
    {
        RoadPrefab = (GameObject)Resources.Load("road_1");
    }
    void Start ()
    {
        Road.RoadSize = RoadPrefab.GetComponent<BoxCollider>().size;
    }
	void Update () {
		
	}

    private static bool IsInTriangle(Vector2 a, Vector2 b, Vector2 c, Vector2 point)
    {
        float N1 = (b.y - a.y) * (point.x - a.x) - (b.x - a.x) * (point.y - a.y);
        float N2 = (c.y - b.y) * (point.x - b.x) - (c.x - b.x) * (point.y - b.y);
        float N3 = (a.y - c.y) * (point.x - c.x) - (a.x - c.x) * (point.y - c.y);

        return ((N1 > 0) && (N2 > 0) && (N3 > 0)) || ((N1 < 0) && (N2 < 0) && (N3 < 0));
    }
    public static int GetPreviewRoadPos(Vector3 obj, Vector2 cursor, RaycastHit hit)
    {
        float x, y;
        x = cursor.x - obj.x;
        y = cursor.y - obj.y;
        PreviewRoad.transform.position = obj;
        if (IsInTriangle(new Vector2(0, 0),
                              new Vector2(0, TileManager.TileSize),
                              new Vector2(Mathf.Round(TileManager.TileSize / 2), Mathf.Round(TileManager.TileSize / 2)),
                              new Vector2(x, y)) && !ExistRoad(0, hit))
        {
            PreviewRoad.transform.position = new Vector3(PreviewRoad.transform.position.x + Road.RoadSize.y / 2,
            PreviewRoad.transform.position.y, -5);
            PreviewRoad.transform.rotation = Quaternion.Euler(0, 0, 90);
            return 0;

        }
        else if (IsInTriangle(new Vector2(0, TileManager.TileSize),
                            new Vector2(Mathf.Round(TileManager.TileSize / 2), Mathf.Round(TileManager.TileSize / 2)),
                            new Vector2(TileManager.TileSize, TileManager.TileSize),
                            new Vector2(x, y)) && !ExistRoad(1, hit))
        {
            PreviewRoad.transform.position = new Vector3(PreviewRoad.transform.position.x,
                                                         PreviewRoad.transform.position.y + TileManager.TileSize - Road.RoadSize.y / 2, -5);
            PreviewRoad.transform.rotation = Quaternion.Euler(0, 0, 0);
            return 1;
        }
        else if (IsInTriangle(new Vector2(TileManager.TileSize, 0),
                             new Vector2(Mathf.Round(TileManager.TileSize / 2), Mathf.Round(TileManager.TileSize / 2)),
                             new Vector2(TileManager.TileSize, TileManager.TileSize),
                             new Vector2(x, y)) && !ExistRoad(2, hit))
        {
            PreviewRoad.transform.position = new Vector3(PreviewRoad.transform.position.x + TileManager.TileSize + Road.RoadSize.y / 2,
                                                         PreviewRoad.transform.position.y, -5);
            PreviewRoad.transform.rotation = Quaternion.Euler(0, 0, 90);
            return 2;
        }
        else if (IsInTriangle(new Vector2(TileManager.TileSize, 0),
                           new Vector2(Mathf.Round(TileManager.TileSize / 2), Mathf.Round(TileManager.TileSize / 2)),
                           new Vector2(0, 0),
                           new Vector2(x, y)) && !ExistRoad(3, hit))
        {
            PreviewRoad.transform.position = new Vector3(PreviewRoad.transform.position.x,
                                                         PreviewRoad.transform.position.y - Road.RoadSize.y / 2, -5);
            PreviewRoad.transform.rotation = Quaternion.Euler(0, 0, 0);
            return 3;
        }
        else
        {
            return 4;
        }
    }
    private static bool ExistRoad(int pos, RaycastHit hit)
    {
        try
        {
            return hit.collider.gameObject.GetComponent<TileManager>().CheckRoad(pos);
        }
        catch
        {
            return true;
        }
    }
    public static GameObject CreateRoad(int pos, GameObject obj)
    {
        GameObject road = Instantiate(Road.RoadPrefab);
        road.transform.position = PreviewRoad.transform.position;
        road.transform.rotation = PreviewRoad.transform.rotation;
        obj.GetComponent<TileManager>().RoadsAround[pos] = road;
        string[] name = obj.name.Split('_');
        int[] nameInt = { int.Parse(name[0]), int.Parse(name[1]) };
        switch (pos)
        {
            case 0:
                if ((nameInt[0] - 1) >= 0)
                    GameObject.Find((nameInt[0] - 1) + "_" + nameInt[1]).GetComponent<TileManager>().RoadsAround[2] = road;
                break;
            case 1:
                if ((nameInt[1] + 1) < GameObject.Find("LevelManager").GetComponent<LevelManager>().MapSizeY)
                    GameObject.Find(nameInt[0] + "_" + (nameInt[1] + 1)).GetComponent<TileManager>().RoadsAround[3] = road;
                break;
            case 2:
                if ((nameInt[0] + 1) < GameObject.Find("LevelManager").GetComponent<LevelManager>().MapSizeX)
                    GameObject.Find((nameInt[0] + 1) + "_" + nameInt[1]).GetComponent<TileManager>().RoadsAround[0] = road;
                break;
            case 3:
                if ((nameInt[1] - 1) >= 0)
                    GameObject.Find(nameInt[0] + "_" + (nameInt[1] - 1)).GetComponent<TileManager>().RoadsAround[1] = road;
                break;
            default:
                break;
        }
        return road;
    }
    public static void HideRoad(GameObject obj1, GameObject obj2)
    {
        if (obj2.transform.position.x - obj1.transform.position.x > 0) // справа от исходного
        {
            obj1.GetComponent<TileManager>().RoadsAround[2].GetComponent<SpriteRenderer>().enabled = false;
            obj2.GetComponent<TileManager>().RoadsAround[0].GetComponent<SpriteRenderer>().enabled = false;
        }
        else if (obj2.transform.position.x - obj1.transform.position.x < 0) // слева от исходного
        {
            obj1.GetComponent<TileManager>().RoadsAround[0].GetComponent<SpriteRenderer>().enabled = false;
            obj2.GetComponent<TileManager>().RoadsAround[2].GetComponent<SpriteRenderer>().enabled = false;
        }
        else 
        {
            if (obj2.transform.position.y - obj1.transform.position.y > 0) // выше от исходного
            {
                obj1.GetComponent<TileManager>().RoadsAround[1].GetComponent<SpriteRenderer>().enabled = false;
                obj2.GetComponent<TileManager>().RoadsAround[3].GetComponent<SpriteRenderer>().enabled = false;
            }
            else // ниже от исходного
            {
                obj1.GetComponent<TileManager>().RoadsAround[3].GetComponent<SpriteRenderer>().enabled = false;
                obj2.GetComponent<TileManager>().RoadsAround[1].GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }
}
