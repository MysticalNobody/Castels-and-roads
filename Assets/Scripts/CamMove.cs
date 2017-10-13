﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour {
    private Ray ray;
    private RaycastHit hit;
    public float tileSize;
    [SerializeField]
    private float moveArea;
    [SerializeField]
    private float camSpeed;
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
    }

    // Update is called once per frame
    void Update() {
        Move(Input.mousePosition.x, Input.mousePosition.y);
        DetectCollision();
    }
    private void DetectCollision() {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            PosRoad(hit.collider.transform.position, cursorWorldPos);
        }
    }
    private void PosRoad(Vector3 obj, Vector2 cursor) {
        float x, y;
        x = cursor.x / obj.x;
        y = cursor.y / obj.y;

        Debug.Log(x + " " + y);
        previewRoad.transform.position = obj;
        previewRoad.transform.position = new Vector3(previewRoad.transform.position.x, previewRoad.transform.position.y, -5);
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
