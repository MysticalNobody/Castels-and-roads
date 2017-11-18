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
    private static float maxX, maxY;

    public CamMove()
    {
        moveArea = 50;
        camSpeed = 5;
    }
    
    void Start()
    {
    }
    void Update() {
        Move(Input.mousePosition.x, Input.mousePosition.y);
        DetectCollision();
    }

    private void DetectCollision() {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
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
                        GameObject go = hit.collider.gameObject;
                        Road.CreateRoad(posRoad, go);
                    }
                }
            }
        }
    }
    public static void SetMax(float x, float y)
    {
        maxX = x;
        maxY = y;
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
