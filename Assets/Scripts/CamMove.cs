using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour {
    private Ray ray;
    private RaycastHit hit;
    [SerializeField]
    private float moveArea;
    [SerializeField]
    private float camSpeed;
    private float maxX, maxY;
    private Vector3 oldMousePos;
    [SerializeField]
    private GameObject roadPrefab;
    private GameObject previewRoad;
    public void SetMax(float x, float y) {
        maxX = x;
        maxY = y;
    }
    CamMove()
    {
        moveArea = 50;
        camSpeed = 5;
    }
    // Use this for initialization
    void Start()
    {
        oldMousePos = new Vector3(0, 0, 0);
        TileManager.roadSize = roadPrefab.GetComponent<BoxCollider>().size;
        Debug.Log(TileManager.roadSize.y);
    }

    // Update is called once per frame
    void Update() {
        Move(Input.mousePosition.x, Input.mousePosition.y);
        DetectCollision();
        oldMousePos = Input.mousePosition;
    }
    private void DetectCollision() {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (!previewRoad)
                previewRoad = Instantiate(roadPrefab);
            int posRoad = PosRoad(hit.collider.transform.position, cursorWorldPos, hit);
            if (posRoad < 4)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (!hit.collider.GetComponent<TileManager>().CheckRoad(posRoad))
                    {
                        TileManager.CreateRoad(0, posRoad, hit, roadPrefab, previewRoad);
                    }
                }
            }
        }
    }

    private int PosRoad(Vector3 obj, Vector2 cursor, RaycastHit hit)
    {
        float x, y;
        x = cursor.x - obj.x;
        y = cursor.y - obj.y;
        previewRoad.transform.position = obj;
        if (IsInTriangle(new Vector2(0, 0),
                              new Vector2(0, TileManager.TileSize),
                              new Vector2(Mathf.Round(TileManager.TileSize / 2), Mathf.Round(TileManager.TileSize / 2)),
                              new Vector2(x, y)) && !ExistRoad(0, hit))
        {
            previewRoad.transform.position = new Vector3(previewRoad.transform.position.x + TileManager.roadSize.y/2,
            previewRoad.transform.position.y, -5);
            previewRoad.transform.rotation = Quaternion.Euler(0, 0, 90);
            return 0;

        }
        else if (IsInTriangle(new Vector2(0, TileManager.TileSize),
                            new Vector2(Mathf.Round(TileManager.TileSize / 2), Mathf.Round(TileManager.TileSize / 2)),
                            new Vector2(TileManager.TileSize, TileManager.TileSize),
                            new Vector2(x, y)) && !ExistRoad(1, hit))
        {
            previewRoad.transform.position = new Vector3(previewRoad.transform.position.x,
                                                         previewRoad.transform.position.y + TileManager.TileSize - TileManager.roadSize.y / 2, -5);
            previewRoad.transform.rotation = Quaternion.Euler(0, 0, 0);
            return 1;
        }
        else if (IsInTriangle(new Vector2(TileManager.TileSize, 0),
                             new Vector2(Mathf.Round(TileManager.TileSize / 2), Mathf.Round(TileManager.TileSize / 2)),
                             new Vector2(TileManager.TileSize, TileManager.TileSize),
                             new Vector2(x, y)) && !ExistRoad(2, hit))
        {
            previewRoad.transform.position = new Vector3(previewRoad.transform.position.x + TileManager.TileSize + TileManager.roadSize.y / 2,
                                                         previewRoad.transform.position.y, -5);
            previewRoad.transform.rotation = Quaternion.Euler(0, 0, 90);
            return 2;
        }
        else if (IsInTriangle(new Vector2(TileManager.TileSize, 0),
                           new Vector2(Mathf.Round(TileManager.TileSize / 2), Mathf.Round(TileManager.TileSize / 2)),
                           new Vector2(0, 0),
                           new Vector2(x, y)) && !ExistRoad(3, hit))
        {
            previewRoad.transform.position = new Vector3(previewRoad.transform.position.x,
                                                         previewRoad.transform.position.y - TileManager.roadSize.y / 2, -5);
            previewRoad.transform.rotation = Quaternion.Euler(0, 0, 0);
            return 3;
        }
        else
        {
            return 4;
        }
    }

    private bool IsInTriangle(Vector2 a, Vector2 b, Vector2 c, Vector2 point)
    {
        float N1 = (b.y - a.y) * (point.x - a.x) - (b.x - a.x) * (point.y - a.y);
        float N2 = (c.y - b.y) * (point.x - b.x) - (c.x - b.x) * (point.y - b.y);
        float N3 = (a.y - c.y) * (point.x - c.x) - (a.x - c.x) * (point.y - c.y);

        return ((N1 > 0) && (N2 > 0) && (N3 > 0)) || ((N1 < 0) && (N2 < 0) && (N3 < 0));
    }
    private bool ExistRoad(int pos, RaycastHit hit)
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

    private void Move(float mouseX, float mouseY) {
        if (0 < mouseX && mouseX < moveArea) {
            transform.Translate(Vector2.left * camSpeed * Time.deltaTime);
        }
        if (Screen.width - moveArea < mouseX) {
            transform.Translate(Vector2.right * camSpeed * Time.deltaTime);
        }
        if (0 < mouseY && mouseY < moveArea) {
            transform.Translate(Vector2.down * camSpeed * Time.deltaTime);
        }
        if (Screen.height - moveArea < mouseY) {
            transform.Translate(Vector2.up * camSpeed * Time.deltaTime);
        }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0, maxX), Mathf.Clamp(transform.position.y, 0, maxY), -10);
    }
    private Vector3 cursorWorldPos {
        get {
            return Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x,
                Input.mousePosition.y,
                Camera.main.nearClipPlane));
        }
    }
}
