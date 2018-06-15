using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    public int cols = 8, rows = 8;
    public GameObject floorTile;
    public GameObject wallTile;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitList() {
        gridPositions.Clear();

        for (int x = 0; x < cols; ++x) {
            for (int y = 0; y < rows; ++y) {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void BoardSetup() {
        boardHolder = new GameObject("Board").transform;

        //for(int x = -1; x < cols + 1; ++x) {
        //    for (int y = -1; y < rows + 1; ++y) {
        //        GameObject toInstantiate = floorTile;
        //        if (x == -1 || x == cols || y == -1 || y == rows)
        //            toInstantiate = wallTile;

        //        GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

        //        instance.transform.SetParent(boardHolder);
        //    }
        //}
    }

    public void setupScene() {
        BoardSetup();
        InitList();

    }
}
