using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed = 3f;
    Direction currentDir;
    Vector2 input;
    bool isMoving = false;
    Vector3 startPos, endPos;
    float moveTime;
    int h, v;
    Animator animator;

    public Sprite[] dirSprites;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!isMoving) {
            h = (int)Input.GetAxisRaw("Horizontal");
            v = (int)Input.GetAxisRaw("Vertical");

            if (h != 0)
                v = 0;

            input = new Vector2(h, v);

            if (input != Vector2.zero){

                if (h != 0)
                    currentDir = (h > 0) ? Direction.Right : Direction.Left;
                else
                    currentDir = (v > 0) ? Direction.Up : Direction.Down;

                switch (currentDir) {
                    case Direction.Down:
                        animator.SetTrigger("moveDown");
                        break;
                    case Direction.Up:
                        animator.SetTrigger("moveUp");
                        break;
                    case Direction.Left:
                        animator.SetTrigger("moveLeft");
                        break;
                    case Direction.Right:
                        animator.SetTrigger("moveRight");
                        break;
                }

                StartCoroutine(Move(transform));
            }
        }

	}

    public IEnumerator Move(Transform t) {
        isMoving = true;
        startPos = t.position;
        moveTime = 0;
        endPos = new Vector3(startPos.x + h, startPos.y + v, 0f);

        while (moveTime < 1f){
            moveTime += Time.deltaTime * moveSpeed;
            t.position = Vector3.Lerp(startPos, endPos, moveTime);
            yield return null;
        }

        isMoving = false;
        yield return 0;
    }
}

enum Direction {
    Down,
    Up,
    Left,
    Right
}

