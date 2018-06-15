using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed = 3f;
    bool isMoving = false;
    Vector3 startPos, endPos;
    float moveTime;
    int h, v;
    Animator animator;
    
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
            
            if (h != 0 || v != 0){
                StartCoroutine(Move(transform));
            }
        }
	}

    public IEnumerator Move(Transform t) {
        isMoving = true;
        animator.SetBool("isMoving", isMoving);
        animator.SetFloat("xPos", h);
        animator.SetFloat("yPos", v);
        startPos = t.position;
        moveTime = 0;
        endPos = new Vector3(startPos.x + h, startPos.y + v, 0f);

        while (moveTime < 1f){
            moveTime += Time.deltaTime * moveSpeed;
            t.position = Vector3.Lerp(startPos, endPos, moveTime);
            yield return null;
        }

        isMoving = false;
        animator.SetBool("isMoving", isMoving);
        animator.SetFloat("xPosLast", h);
        animator.SetFloat("yPosLast", v);
        yield return 0;
    }

}
