using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public BoardManager boardScript;
    [HideInInspector] public bool playerInControl = true;

	void Awake () {

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

	void Update () {
		
	}
}
