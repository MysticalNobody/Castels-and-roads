using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public static GameObject[] players;
    private static int currentPlayer = 0;
    static Text labelInfo;
    public static Dropdown picker;
    public static Text score;
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
        picker = GameObject.Find("Dropdown").GetComponent<Dropdown>();
    }
    void Update() {
        for (int i = 0; i < players.Length; i++) {
            players[i].GetComponent<PlayerController>().UpdatePoints();
            players[i].GetComponent<PlayerController>().UpdateTileColor();
        }
        score.text = "Player1: " + players[0].GetComponent<PlayerController>().Points + " vs Player2: " + players[1].GetComponent<PlayerController>().Points;
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
            players[i].name = "Player" + i.ToString();
        }
    }
}
