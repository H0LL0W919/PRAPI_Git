using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    private bool isMoving;
    private Vector2 input;
    private Animator animator;
    public LayerMask solidObjectsLayer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0) input.y = 0; //removes diagonal movement  
            
            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (IsWalkable(targetPos)) //can only move if IsWalkable returns true
                {
                    StartCoroutine(Move(targetPos));
                }
                
            }

        }

        animator.SetBool("isMoving", isMoving);

    }

    IEnumerator Move(Vector3 targetPos) //Moving the player from it's position to target position over a period of time.
    {

        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon) //Checking to see if there's a difference between currentPos and targetPos
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, movementSpeed * Time.deltaTime);
            yield return null; //breaks loop so that the currentPos can be updated and repeated
        }

        transform.position = targetPos;

        isMoving = false;

    }

    private bool IsWalkable(Vector3 targetPos)
    {
        
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer) != null) //checking to see if the tile the player wants to move to isnt blocked by a collision object
        {
            return false;
        }

        return true;

    }
}
