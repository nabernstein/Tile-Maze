using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float moveTime = 0.1f;
    public LayerMask wall;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime;
    private string playerMoveDir;

	// Use this for initialization
	void Start () {

        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;		
	}

    void Update() {
        if (!GameManager.instance.playerInControl) return;

        int horz = 0, vert = 0;

        horz = (int)Input.GetAxisRaw("Horizontal");
        vert = (int)Input.GetAxisRaw("Vertical");

        if (horz != 0) // prevents diagonal movement
            vert = 0;

        if(horz != 0 || vert != 0)
            AttemptMove(horz, vert);
    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit) {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, wall);
        boxCollider.enabled = true;

        if (hit.transform == null) {
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        return false;
    }

    protected void AttemptMove(int xDir, int yDir) {
        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);

        if (hit.transform == null)
            return;
    }

    //private void OnTrigger2D(Collider2D other) {
    //    string direction = other.tag;
    //}

    protected IEnumerator SmoothMovement(Vector3 end) {
        float remainingDistSqr = (transform.position - end).sqrMagnitude;
        
        while(remainingDistSqr > float.Epsilon) {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            remainingDistSqr = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    //protected void OnCantMove<T>(T component) {
    //    MoveTile moveTile = component as MoveTile;
    //}

    //void Send(string direction) {

    //    while (direction != "Stop") {

    //    }

    //}
}
