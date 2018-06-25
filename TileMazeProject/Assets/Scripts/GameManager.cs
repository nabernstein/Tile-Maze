using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public float turnDelay = 0f;
    public BoardManager boardScript;
    [HideInInspector] public bool playerInControl = true;

    private bool tileMoving;

    void Awake() {

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    void InitGame() {
        boardScript.setupScene();
    }

    //These don't acually do anything... yet.
    //void Update() {
    //    if (playerInControl || tileMoving)
    //        return;

    //    StartCoroutine(TileMovement());
    //}

    //IEnumerator TileMovement() {
    //    tileMoving = true;

    //    yield return new WaitForSeconds(turnDelay);
    //    playerInControl = true;
    //    tileMoving = false;
    //}
}