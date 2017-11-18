using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {
    public static GameObject[] buildingPrefabs;
    [SerializeField]
    private int type;

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

    public Building()
    {
        type = -1;
    }
    static Building()
    {
        buildingPrefabs = new GameObject[5];
    }

    void Awake()
    {
        for (int i = 0; i < 5; i++) // 0 - дом, 1 - ферма, 2 - мастерская, 3 - крепость, 4 - аванпост
            buildingPrefabs[i] = (GameObject)Resources.Load("building"+i);
    }
    void Start () {
	}
	void Update () {
		
	}

    public static GameObject CreateBuilding(GameObject obj, int type)
    {
        GameObject building = Instantiate(buildingPrefabs[type]);
        building.transform.position = new Vector3(obj.transform.position.x + TileManager.TileSize / 2, obj.transform.position.y + TileManager.TileSize / 2, 0.2f);
        building.transform.rotation = Quaternion.Euler(0, 0, 0);
        obj.GetComponent<TileManager>().House = building;
        GameObject newObj = CheckBuildingsAround(obj);
        if (newObj != null)
            MergeTwoBuildings(obj, newObj);
        return building;
    }
    private static GameObject CheckBuildingsAround(GameObject obj) 
    {
        string[] name = obj.name.Split('_');
        int[] nameInt = { int.Parse(name[0]), int.Parse(name[1]) };
        if ((nameInt[0] - 1) >= 0 && GameObject.Find((nameInt[0] - 1) + "_" + nameInt[1]).GetComponent<TileManager>() && GameObject.Find((nameInt[0] - 1) + "_" + nameInt[1]).GetComponent<TileManager>().Active)
            if (GameObject.Find((nameInt[0] - 1) + "_" + nameInt[1]).GetComponent<TileManager>().House.GetComponent<Building>().Type == obj.GetComponent<TileManager>().House.GetComponent<Building>().Type)
            {
                return GameObject.Find((nameInt[0] - 1) + "_" + nameInt[1]);
            }
        if ((nameInt[1] + 1) < GameObject.Find("LevelManager").GetComponent<LevelManager>().MapSizeY && GameObject.Find((nameInt[0]) + "_" + (nameInt[1] + 1)).GetComponent<TileManager>() && GameObject.Find((nameInt[0]) + "_" + (nameInt[1] + 1)).GetComponent<TileManager>().Active)
            if (GameObject.Find((nameInt[0]) + "_" + (nameInt[1] + 1)).GetComponent<TileManager>().House.GetComponent<Building>().Type == obj.GetComponent<TileManager>().House.GetComponent<Building>().Type)
            {
                return GameObject.Find((nameInt[0]) + "_" + (nameInt[1] + 1));
            }
        if ((nameInt[0] + 1) < GameObject.Find("LevelManager").GetComponent<LevelManager>().MapSizeX && GameObject.Find((nameInt[0] + 1) + "_" + nameInt[1]).GetComponent<TileManager>()  && GameObject.Find((nameInt[0] + 1) + "_" + nameInt[1]).GetComponent<TileManager>().Active)
            if (GameObject.Find((nameInt[0] + 1) + "_" + nameInt[1]).GetComponent<TileManager>().House.GetComponent<Building>().Type == obj.GetComponent<TileManager>().House.GetComponent<Building>().Type)
            {
                return GameObject.Find((nameInt[0] + 1) + "_" + nameInt[1]);
            }
        if ((nameInt[1] - 1) >= 0 && GameObject.Find((nameInt[0]) + "_" + (nameInt[1] - 1)).GetComponent<TileManager>() && GameObject.Find((nameInt[0]) + "_" + (nameInt[1] - 1)).GetComponent<TileManager>().Active)
            if (GameObject.Find((nameInt[0]) + "_" + (nameInt[1] - 1)).GetComponent<TileManager>().House.GetComponent<Building>().Type == obj.GetComponent<TileManager>().House.GetComponent<Building>().Type)
            {
                return GameObject.Find((nameInt[0]) + "_" + (nameInt[1] - 1));
            }
        return null;
    }
    private static void MergeTwoBuildings(GameObject obj1, GameObject obj2)
    {
        Road.HideRoad(obj1, obj2);
        Vector3 newpos = new Vector3(obj1.transform.position.x + (obj2.transform.position.x - obj1.transform.position.x) / 2f, obj1.transform.position.y + (obj2.transform.position.y - obj1.transform.position.y) / 2f, 0.2f);
        obj1.GetComponent<TileManager>().House.transform.position = new Vector3(newpos.x + TileManager.TileSize / 2, newpos.y + TileManager.TileSize / 2, 0.2f);
        Destroy(obj2.GetComponent<TileManager>().House); // добавить доп.проверки, обнаружен баг опустевающих тайлов
        obj2.GetComponent<TileManager>().House = obj1.GetComponent<TileManager>().House;
    }
}
