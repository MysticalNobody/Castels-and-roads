using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public static GameObject[] players;
    private static int currentPlayer = 0;
    static Text labelInfo;
    public static int chosenBuilding = 0;
    public static Text labelBuilding;
    public static Text score;
    public static RawImage minimap;
    enum Buildings{
    House = 0,
    Ferma = 1,
    Manufactory = 2,
    Castle = 3,
    Outpost = 4
    }
    public static int Turn {
        get {
            return currentPlayer;
        }
        set {
            currentPlayer = value;
        }
    }
    void Start() {
        CreatePlayers(2);
        labelInfo = GameObject.Find("Info").GetComponent<Text>();
        score = GameObject.Find("Score").GetComponent<Text>();
        labelBuilding = GameObject.Find("LabelBuilding").GetComponent<Text>();
        minimap = GameObject.Find("Minimap").GetComponent<RawImage>();
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            chosenBuilding = 0;
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            chosenBuilding = 1;
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            chosenBuilding = 2;
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            chosenBuilding = 3;
        }
        else if (Input.GetKeyDown(KeyCode.F5))
        {
            chosenBuilding = 4;
        }
        else if (Input.GetKeyDown(KeyCode.F10)) {
            minimap.enabled = true;
        }
        for (int i = 0; i < players.Length; i++) {
            players[i].GetComponent<PlayerController>().UpdatePoints();
            players[i].GetComponent<PlayerController>().UpdateTileColor();
        }
        score.text = "Player1: " + players[0].GetComponent<PlayerController>().Points + " vs Player2: " + players[1].GetComponent<PlayerController>().Points;
        labelBuilding.text = "Now chosen " + (Buildings)chosenBuilding;
    }
    public static void SwitchTurn() {
        labelInfo.text = "Player " + (currentPlayer + 1) + " is currently playing";
        if (currentPlayer < players.Length - 1) {
            currentPlayer += 1;

        } else {
            currentPlayer = 0;
        }

    }
    public static void CreatePlayers(int count) {
        players = new GameObject[count];
        for (int i = 0; i < count; i++) {
            players[i] = new GameObject();
            players[i].AddComponent<PlayerController>();
            players[i].name = "Player" + i;
        }
    }
}
