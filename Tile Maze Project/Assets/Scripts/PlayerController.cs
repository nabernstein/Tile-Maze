using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour {

    public float moveSpeed = 3f;
    public LayerMask blockingLayer;
    public bool isMoving = false;
    public bool isAtHome;

    Animator animator;
    BoxCollider2D boxCollider;
    Rigidbody2D rb2D;
    public GameObject onTile;
    public UIInput UIAxisInput;
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
        PlayerControl = GameManager.instance.playerInControl; //Just so I can see it in the inspector

        if (!isMoving && GameManager.instance.playerInControl) {

            //Prioritizes input from UI buttons over keyboard
            int horizontal = UIAxisInput.horizontal == 0 ? (int)Input.GetAxisRaw("Horizontal") : UIAxisInput.horizontal;
            int vertical = UIAxisInput.vertical == 0 ? (int)Input.GetAxisRaw("Vertical") : UIAxisInput.vertical;

            //Disallows diagonal movment, prioritizes horizontal movement
            if (horizontal != 0)
                vertical = 0;

            //Moves one square in the appropriate direction
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
        end = (CanMove(start, end)) ? end : start;
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
        return UIAxisInput.horizontal != 0 || UIAxisInput.vertical != 0;
    }

    bool CheckPlayerControl() {
        if (!isMoving) {
            if (onTile != null)
                return onTile.tag == "Stop";
            return true;
        }
        return true;
    }

    void MoveUp() {
        if(CheckPlayerControl())
            StartCoroutine(Move(0, 1));
    }
}