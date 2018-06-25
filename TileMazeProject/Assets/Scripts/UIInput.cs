using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIInput : MonoBehaviour{
    public int vertical,
        horizontal;

    void Start() {
        vertical = 0;
        horizontal = 0;
    }

    public void UpDateVertical(int change) {
        vertical += change;
    }

    public void UpdateHorizontal(int change) {
        horizontal += change;
    }

}
