using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour {

    public float moveSpeed = 3f;
    public LayerMask blockingLayer;
    public bool isMoving = false;

    Animator animator;
    BoxCollider2D boxCollider;
    Rigidbody2D rb2D;
    public GameObject onTile;
    public bool PlayerControl;


    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        PlayerControl = GameManager.instance.playerInControl;
    }

    // Update is called once per frame
    void Update() {
        animator.SetBool("keyHeldDown", IsKeyDown());
        GameManager.instance.playerInControl = CheckPlayerControl();
        PlayerControl = GameManager.instance.playerInControl;
        
        if (!isMoving && GameManager.instance.playerInControl) {
            int horizontal = (int)Input.GetAxisRaw("Horizontal");
            int vertical = (int)Input.GetAxisRaw("Vertical");

            if (horizontal != 0)
                vertical = 0;

            if (horizontal != 0 || vertical != 0) {
                StartCoroutine(Move(horizontal, vertical));
            }
        }

        if (!isMoving && !GameManager.instance.playerInControl && onTile != null) {
            StartCoroutine(TileMove(onTile));
        }
    }

    public IEnumerator Move(int xDir, int yDir) {
        isMoving = true;
        animator.SetFloat("xPos", xDir);
        animator.SetFloat("yPos", yDir);
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);
        end = (CanMove(start,end)) ? end : start;
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
        yield return new WaitForSeconds(1);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        onTile = collision.gameObject;
        animator.SetBool("isSpinning", collision.tag != "Stop");
        GameManager.instance.playerInControl = false;
    }

    public IEnumerator TileMove(GameObject onTile) {
        string tileTag = onTile.tag;
        int xDir = 0, yDir = 0;

        if (tileTag == "Right" || tileTag == "Left")
            xDir = (tileTag == "Right") ? 1 : -1;
        if (tileTag == "Up" || tileTag == "Down")
            yDir = (tileTag == "Up") ? 1 : -1;

        Vector2 start = transform.position,
            end = start + new Vector2(xDir, yDir);

        if (!CanMove(start, end)) {
            animator.SetBool("isSpinning", false);
        }

        if (start != end && CanMove(start, end)) {
            StartCoroutine(Move(xDir, yDir));
        }
        
        yield return null;
    }

    bool CanMove(Vector2 start, Vector2 end) {
        boxCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;
        if (hit)
            onTile = null;
        return !hit;
    }

    bool IsKeyDown() {
        return Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
    }

    bool CheckPlayerControl() {
        if (onTile == null)
            return true;
        else
            return onTile.tag == "Stop";
    }
}