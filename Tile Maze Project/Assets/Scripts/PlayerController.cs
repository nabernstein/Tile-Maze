using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed = 3f;
    public LayerMask blockingLayer;
    bool isMoving = false;
    Animator animator;
    BoxCollider2D boxCollider;
    Rigidbody2D rb2D;

    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        animator.SetBool("keyHeldDown", IsKeyDown());

        if (!isMoving) {
            int horizontal = (int)Input.GetAxisRaw("Horizontal");
            int vertical = (int)Input.GetAxisRaw("Vertical");

            if (horizontal != 0)
                vertical = 0;

            if (horizontal != 0 || vertical != 0) {
                StartCoroutine(Move(horizontal, vertical));
            }
        }
    }

    public IEnumerator Move(int xDir, int yDir) {
        isMoving = true;
        animator.SetFloat("xPos", xDir);
        animator.SetFloat("yPos", yDir);
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);
        boxCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;
        end = (hit.transform == null) ? end : start;
        animator.SetBool("isMoving", isMoving);

        float moveTime = 0;
        while (moveTime < 1f) {
            moveTime += Time.deltaTime * moveSpeed;
            transform.position = Vector2.Lerp(start, end, moveTime);
            yield return null;
        }

        isMoving = false;
        animator.SetBool("isMoving", isMoving);
        animator.SetFloat("xPosLast", xDir);
        animator.SetFloat("yPosLast", yDir);
        yield return null;
    }

    bool IsKeyDown() {
        return Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
    }

}