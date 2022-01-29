using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    Vector3 lastMovementDirection;

    public int playerNum;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(0);

        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveDirectionCheck(Vector2.up);
            //movementInput = new Vector3(0, 1f, 0);
            //transform.Translate(lastMovementDirection);
            //rb.MovePosition(transform.position + new Vector3(0, 1, 0) * Time.deltaTime);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            MoveDirectionCheck(Vector2.down);
            //transform.Translate(0, -1f, 0);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            MoveDirectionCheck(Vector2.left);
            //transform.Translate(-1f, 0f, 0);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveDirectionCheck(Vector2.right);
            //transform.Translate(1f, 0f, 0);
        }
    }

    private void MoveDirectionCheck(Vector2 direction)
    {
        Debug.DrawRay(rb.transform.position, direction, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(direction.x, direction.y, 0), direction, 0.1f);
        //Debug.Log("Pushable Raycast Collided With: " + hit.collider);

        lastMovementDirection = direction;



        // Non-Block Space -- Move Player
        if (hit.collider == null)
        {
            transform.Translate(direction);
            return;
        }

        // Blocking Space Wall - Move Player (then destroy)
        if (hit.collider.tag == "Wall")
        {
            //transform.Translate(direction);
            //return;
        }

        // Pusable Object - Push the Object and Move Player if possible
        if (hit.collider.tag == "Pushable")
        {
            // Check if pushableObj move space is blocked;
            PushableMovement pushableObj = hit.collider.GetComponent<PushableMovement>();
            bool canPush = pushableObj.PotentialMoveCollisionCheck(lastMovementDirection, playerNum);
            if (!canPush)
                return;

            // If pushableObj can move; Move pushableObject; Move Player;
            pushableObj.MovePushable(lastMovementDirection);
            transform.Translate(direction);
            return;
        }


        // ~ Destroy Players (Win Condition?) if they collide
        /*
        if (hit.collider.gameObject != this.gameObject && hit.collider.tag == "Player")
        {
            Destroy(hit.collider.gameObject);
            Destroy(this.gameObject);
            return;
        }
        */
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {   
        // ~ Destroy Player on Wall Collision
        /*
        if(collision.tag == "Wall")
        {
            Debug.Log("You Lose");
            return;
        }
        */        
    }
}
