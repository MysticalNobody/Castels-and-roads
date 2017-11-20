using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public static GameObject[] buildingPrefabs; // 0 - дом, 1 - ферма, 2 - мастерская, 3 - крепость, 4 - аванпост
    private static List<GameObject> mergedBuildings;
    [SerializeField]
    private int type;
    private int level;
    private bool isChecked;

    public int Type
    {
        get
        {
            return type;
        }

        set
        {
            type = value;
        }
    }
    public bool IsChecked
    {
        get
        {
            return isChecked;
        }

        set
        {
            isChecked = value;
        }
    }
    public int Level
    {
        get
        {
            return level;
        }

        set
        {
            level = value;
        }
    }

    public Building()
    {
        type = -1;
        level = 0;
        isChecked = false;
    }
    static Building()
    {
        buildingPrefabs = new GameObject[5];
        mergedBuildings = new List<GameObject>();
    }

    void Awake()
    {
        for (int i = 0; i < 5; i++)
            buildingPrefabs[i] = (GameObject)Resources.Load("building" + i);
    }
    void Start()
    {

    }
    void Update()
    {

    }

    public static int GetTypeOfBuilding(GameObject tile)
    {
        return tile.GetComponent<TileManager>().House.GetComponent<Building>().Type;
    }
    public static int GetLevelOfBuilding(GameObject tile) 
    {
        return tile.GetComponent<TileManager>().House.GetComponent<Building>().Level;
    }
    public static void SetLevelOfBuilding(GameObject tile, int level)
    {
        tile.GetComponent<TileManager>().House.GetComponent<Building>().Level = level;
    }
    public static bool GetIsChecked(GameObject tile)
    {
        return tile.GetComponent<TileManager>().House.GetComponent<Building>().IsChecked;
    }
    public static void SetIsChecked(GameObject tile, bool isChecked)
    {
        tile.GetComponent<TileManager>().House.GetComponent<Building>().IsChecked = isChecked;
    }

    public static void CalculateLevelOfBuilding(GameObject obj)
    {
        mergedBuildings.Add(obj);
        SetIsChecked(obj, true);
        for (int i = 0; i < 4; i++)
        {
            GameObject newObj = CheckBuildingsAround(obj, i);
            if (newObj && !GetIsChecked(newObj))
                CalculateLevelOfBuilding(newObj);
        }
    }
    public static GameObject CreateBuilding(GameObject obj, int type)
    {
        GameObject building = Instantiate(buildingPrefabs[type]);
        building.transform.position = new Vector3(obj.transform.position.x + TileManager.TileSize / 2, obj.transform.position.y + TileManager.TileSize / 2, 0.2f);
        building.transform.rotation = Quaternion.Euler(0, 0, 0);
        obj.GetComponent<TileManager>().House = building;
        for (int i = 0; i < 4; i++)
        {
            GameObject newObj = CheckBuildingsAround(obj, i);
            if (newObj)
                MergeTwoBuildings(obj, newObj);
        }
        CalculateLevelOfBuilding(obj);
        int level = mergedBuildings.Count;
        foreach (GameObject go in mergedBuildings)
        {
            SetLevelOfBuilding(go, level);
            SetIsChecked(go, false);
        }
        mergedBuildings = new List<GameObject>();
        Debug.Log("Level of building: "+level);
        return building;
    }
    private static GameObject CreateMergedBuilding(int type, Vector3 pos)
    {
        GameObject building = Instantiate(buildingPrefabs[type]);
        building.transform.position = new Vector3(pos.x + TileManager.TileSize / 2, pos.y + TileManager.TileSize / 2, 0.2f);
        building.transform.rotation = Quaternion.Euler(0, 0, 0);
        return building;
    }
    private static GameObject CheckBuildingsAround(GameObject obj, int pos)
    {
        string[] name = obj.name.Split('_');
        int[] nameInt = { int.Parse(name[0]), int.Parse(name[1]) };
        if (pos == 0)
        {
            if ((nameInt[0] - 1) >= 0 && GameObject.Find((nameInt[0] - 1) + "_" + nameInt[1]).GetComponent<TileManager>() && GameObject.Find((nameInt[0] - 1) + "_" + nameInt[1]).GetComponent<TileManager>().Active)
                if (GetTypeOfBuilding(GameObject.Find((nameInt[0] - 1) + "_" + nameInt[1])) == GetTypeOfBuilding(obj))
                {
                    return GameObject.Find((nameInt[0] - 1) + "_" + nameInt[1]);
                }
        }
        else if (pos == 1)
        {
            if ((nameInt[1] + 1) < LevelManager.MapSizeY && GameObject.Find((nameInt[0]) + "_" + (nameInt[1] + 1)).GetComponent<TileManager>() && GameObject.Find((nameInt[0]) + "_" + (nameInt[1] + 1)).GetComponent<TileManager>().Active)
                if (GetTypeOfBuilding(GameObject.Find((nameInt[0]) + "_" + (nameInt[1] + 1))) == GetTypeOfBuilding(obj))
                {
                    return GameObject.Find((nameInt[0]) + "_" + (nameInt[1] + 1));
                }
        }
        else if (pos == 2)
        {
            if ((nameInt[0] + 1) < LevelManager.MapSizeX && GameObject.Find((nameInt[0] + 1) + "_" + nameInt[1]).GetComponent<TileManager>() && GameObject.Find((nameInt[0] + 1) + "_" + nameInt[1]).GetComponent<TileManager>().Active)
                if (GetTypeOfBuilding(GameObject.Find((nameInt[0] + 1) + "_" + nameInt[1])) == GetTypeOfBuilding(obj))
                {
                    return GameObject.Find((nameInt[0] + 1) + "_" + nameInt[1]);
                }
        }
        else if (pos == 3)
        {
            if ((nameInt[1] - 1) >= 0 && GameObject.Find((nameInt[0]) + "_" + (nameInt[1] - 1)).GetComponent<TileManager>() && GameObject.Find((nameInt[0]) + "_" + (nameInt[1] - 1)).GetComponent<TileManager>().Active)
                if (GetTypeOfBuilding(GameObject.Find((nameInt[0]) + "_" + (nameInt[1] - 1))) == GetTypeOfBuilding(obj))
                {
                    return GameObject.Find((nameInt[0]) + "_" + (nameInt[1] - 1));
                }
        }
        return null;
    }
    private static void MergeTwoBuildings(GameObject obj1, GameObject obj2)
    {
        Road.HideRoad(obj1, obj2);
        Vector3 newpos = new Vector3(obj1.transform.position.x + (obj2.transform.position.x - obj1.transform.position.x) / 2f, obj1.transform.position.y + (obj2.transform.position.y - obj1.transform.position.y) / 2f, 0.2f);
        GameObject house = CreateMergedBuilding(GetTypeOfBuilding(obj1), newpos);
        obj1.GetComponent<TileManager>().House.GetComponent<SpriteRenderer>().enabled = false;
        obj2.GetComponent<TileManager>().House.GetComponent<SpriteRenderer>().enabled = false;
    }
}
