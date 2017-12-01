using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    private Ray ray;
    [SerializeField]
    private float moveArea;
    [SerializeField]
    private float camSpeed;
    private static float maxX, maxY;
    public static List<GameObject> rounds;
    public static GameObject[] roundPrefabs;
    public static bool staticCurTile;
    public static GameObject currentTile;

    public CamMove()
    {
        moveArea = 50;
        camSpeed = 5;
    }
    static CamMove()
    {
        roundPrefabs = new GameObject[5];
        rounds = new List<GameObject>();
    }

    void Start()
    {
        currentTile = new GameObject();
        staticCurTile = false;
        for (int i = 0; i < 5; i++)
            roundPrefabs[i] = (GameObject)Resources.Load("round" + i);
    }

    void Update()
    {
        Move(Input.mousePosition.x, Input.mousePosition.y);
        DetectCollision();
    }

    private void DetectCollision()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            GameObject go = hit.collider.gameObject;
            if (!staticCurTile)
            {
                if (!Road.PreviewRoad)
                    Road.PreviewRoad = Instantiate(Road.RoadPrefab);
                int posRoad = Road.GetPreviewRoadPos(hit.collider.transform.position, cursorWorldPos, hit);

                if (posRoad < 4)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        if (!hit.collider.GetComponent<TileManager>().CheckRoad(posRoad))
                        {
                            Road.CreateRoad(posRoad, go);
                            if (hit.collider.GetComponent<TileManager>().CheckTileState() && !hit.collider.GetComponent<TileManager>().Active)
                            {
                                staticCurTile = true;
                                currentTile = go;
                                ShowActions(go);
                            }
                        }
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (hit.collider.GetComponent<RoundButton>())
                    {
                        if (hit.collider.GetComponent<RoundButton>().Type == 0)
                        {
                            Building.CreateBuilding(currentTile, 0);
                            currentTile.GetComponent<TileManager>().Active = true;
                        }
                        else if (hit.collider.GetComponent<RoundButton>().Type == 1)
                        {
                            Building.CreateBuilding(currentTile, 1);
                            currentTile.GetComponent<TileManager>().Active = true;
                        }
                        else if (hit.collider.GetComponent<RoundButton>().Type == 2)
                        {
                            Building.CreateBuilding(currentTile, 2);
                            currentTile.GetComponent<TileManager>().Active = true;
                        }
                        else if (hit.collider.GetComponent<RoundButton>().Type == 3)
                        {
                            Building.CreateBuilding(currentTile, 3);
                            currentTile.GetComponent<TileManager>().Active = true;
                        }
                        else if (hit.collider.GetComponent<RoundButton>().Type == 4)
                        {
                            Building.CreateBuilding(currentTile, 4);
                            currentTile.GetComponent<TileManager>().Active = true;
                        }
                        staticCurTile = false;
                        foreach (GameObject r in rounds)
                            Destroy(r);
                        rounds = new List<GameObject>();
                        GameController.players[GameController.Turn].GetComponent<PlayerController>().AddTile(currentTile);
                        currentTile.GetComponent<TileManager>().owner = GameController.players[GameController.Turn];
                        GameController.SwitchTurn();
                    }
                }
            }
        }
    }
    public static void ShowActions(GameObject obj)
    {
        Debug.Log("Hi!");
        double y = obj.transform.position.y + TileManager.TileSize * 1.5f;
        for (int i = 0; i < 5; i++)
        {
            rounds.Add(Instantiate(roundPrefabs[i]));
            rounds[rounds.Count - 1].GetComponent<RoundButton>().Type = i;
        }
        for (int i = 0; i < rounds.Count; i++)
            rounds[i].transform.position = new Vector2((float)(obj.transform.position.x + 1.024f - 0.5f * (rounds.Count * 1.024f + (rounds.Count - 1) * 1.024f) + i * 2.048f), (float)y);

    }
    public static void SetMax(float x, float y)
    {
        maxX = x;
        maxY = y;
    }
    private void Move(float mouseX, float mouseY)
    {
        if (0 < mouseX && mouseX < moveArea)
        {
            transform.Translate(Vector2.left * camSpeed * Time.deltaTime);
        }
        if (Screen.width - moveArea < mouseX)
        {
            transform.Translate(Vector2.right * camSpeed * Time.deltaTime);
        }
        if (0 < mouseY && mouseY < moveArea)
        {
            transform.Translate(Vector2.down * camSpeed * Time.deltaTime);
        }
        if (Screen.height - moveArea < mouseY)
        {
            transform.Translate(Vector2.up * camSpeed * Time.deltaTime);
        }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0, maxX), Mathf.Clamp(transform.position.y, 0, maxY), -10);
    }
    private Vector3 cursorWorldPos
    {
        get
        {
            return Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x,
                Input.mousePosition.y,
                Camera.main.nearClipPlane));
        }
    }

}
