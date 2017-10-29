using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour {
    private Ray ray;
    private RaycastHit hit;
    public float tileSize;
    private Vector2 roadSize;
    [SerializeField]
    private float moveArea = 50;
    [SerializeField]
    private float camSpeed = 5;
    float maxX, maxY;
    [SerializeField]
    private GameObject[] roadPrefabs;
    private GameObject previewRoad;
    public void SetMax(float x, float y) {
        maxX = x;
        maxY = y;
    }
    // Use this for initialization
    void Start() {
        previewRoad = Instantiate(roadPrefabs[0]);
        roadSize = previewRoad.GetComponent<BoxCollider>().bounds.size;
    }

    // Update is called once per frame
    void Update() {
        Move(Input.mousePosition.x, Input.mousePosition.y);
        DetectCollision();
    }
    private void DetectCollision() {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            int posRoad = PosRoad(hit.collider.transform.position, cursorWorldPos);
            if (posRoad < 4 ) {
                if (Input.GetKeyDown(KeyCode.Mouse0)) {
                    if (!hit.collider.GetComponent<TileManager>().CheckRoad(posRoad)) {
                        CreateRoad(0, posRoad);
                    }
                }
            }
        }
    }
    private void CreateRoad(int type, int pos) {
        GameObject newRoad = Instantiate(roadPrefabs[type]);
        newRoad.transform.position = previewRoad.transform.position;
        newRoad.transform.rotation = previewRoad.transform.rotation;
        hit.collider.gameObject.GetComponent<TileManager>().UpdateTileState(pos);
        string[] name = hit.collider.gameObject.name.Split('_');
        int[] nameInt = { int.Parse(name[0]), int.Parse(name[1]) };
        Debug.Log(nameInt[0] + " " + nameInt[1]);
        Debug.Log(maxY);
        switch (pos) {
            case 0:
                if ((nameInt[0] - 1) >= 0) {
                    GameObject.Find((nameInt[0] - 1) + "_" + nameInt[1]).GetComponent<TileManager>().UpdateTileState(2);
                }
                break;
            case 1:
                if ((nameInt[1] + 1) < GameObject.Find("LevelManager").GetComponent<LevelManager>().MapSizeY) {
                    GameObject.Find(nameInt[0] + "_" + (nameInt[1] + 1)).GetComponent<TileManager>().UpdateTileState(3);
                }
                break;
            case 2:
                if ((nameInt[0] + 1) < GameObject.Find("LevelManager").GetComponent<LevelManager>().MapSizeX) {
                    GameObject.Find((nameInt[0] + 1) + "_" + nameInt[1]).GetComponent<TileManager>().UpdateTileState(0);
                }
                break;
            case 3:
                if ((nameInt[1] - 1) >= 0) {
                    GameObject.Find(nameInt[0] + "_" + (nameInt[1] - 1)).GetComponent<TileManager>().UpdateTileState(1);
                }
                break;
            default:
                break;
        }
    }
    private bool ExistRoad(int pos) {
        try {
            return hit.collider.gameObject.GetComponent<TileManager>().CheckRoad(pos);
        } catch {
            return true;
        }
    }
    private int PosRoad(Vector3 obj, Vector2 cursor) {
        float x, y;
        x = cursor.x - obj.x;
        y = cursor.y - obj.y;
        previewRoad.transform.position = obj;
        //Debug.Log(x + " " + y);
        Vector2 roadPos = new Vector2(previewRoad.transform.position.x - obj.x, previewRoad.transform.position.y - obj.y);
        if (IsInTriangle(new Vector2(0, 0),
                              new Vector2(0, tileSize),
                              new Vector2(Mathf.Round(tileSize / 2), Mathf.Round(tileSize / 2)),
                              new Vector2(x, y))&& !ExistRoad(0)) {
            previewRoad.transform.position = new Vector3(previewRoad.transform.position.x + roadSize.y / 2,
                                                         previewRoad.transform.position.y, -5);
            previewRoad.transform.rotation = Quaternion.Euler(0, 0, 90);
            Debug.Log("Left");
            return 0;

        } else if (IsInTriangle(new Vector2(0, tileSize),
                              new Vector2(Mathf.Round(tileSize / 2), Mathf.Round(tileSize / 2)),
                              new Vector2(tileSize, tileSize),
                              new Vector2(x, y))&& !ExistRoad(1)) {
            previewRoad.transform.position = new Vector3(previewRoad.transform.position.x,
                                                         previewRoad.transform.position.y + tileSize - roadSize.y / 2, -5);
            previewRoad.transform.rotation = Quaternion.Euler(0, 0, 0);
            Debug.Log("Top");
            return 1;
        } else if (IsInTriangle(new Vector2(tileSize, 0),
                               new Vector2(Mathf.Round(tileSize / 2), Mathf.Round(tileSize / 2)),
                               new Vector2(tileSize, tileSize),
                               new Vector2(x, y))&& !ExistRoad(2)) {
            previewRoad.transform.position = new Vector3(previewRoad.transform.position.x + tileSize + roadSize.y / 2,
                                                         previewRoad.transform.position.y, -5);
            previewRoad.transform.rotation = Quaternion.Euler(0, 0, 90);
            Debug.Log("Right");
            return 2;
        } else if (IsInTriangle(new Vector2(tileSize, 0),
                             new Vector2(Mathf.Round(tileSize / 2), Mathf.Round(tileSize / 2)),
                             new Vector2(0, 0),
                             new Vector2(x, y)) && !ExistRoad(3)) {
            previewRoad.transform.position = new Vector3(previewRoad.transform.position.x,
                                                         previewRoad.transform.position.y - roadSize.y / 2, -5);
            previewRoad.transform.rotation = Quaternion.Euler(0, 0, 0);
            Debug.Log("Bottom");
            return 3;
        } else {
            Debug.Log("None");
            return 4;
        }
    }
    private bool IsInTriangle(Vector2 a, Vector2 b, Vector2 c, Vector2 point) {
        float N1 = (b.y - a.y) * (point.x - a.x) - (b.x - a.x) * (point.y - a.y);
        float N2 = (c.y - b.y) * (point.x - b.x) - (c.x - b.x) * (point.y - b.y);
        float N3 = (a.y - c.y) * (point.x - c.x) - (a.x - c.x) * (point.y - c.y);

        return ((N1 > 0) && (N2 > 0) && (N3 > 0)) || ((N1 < 0) && (N2 < 0) && (N3 < 0));
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
