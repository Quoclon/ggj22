using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{


    [Header("Managers")]
    GameManager gameManager;
    AudioManager audioManager;
    
    private Rigidbody2D rb;
    Vector3 lastMovementDirection;

    public int playerNum;

    [Header("Trailing Blocks")]
    [SerializeField] bool dropsTrailingBlocks;
    [SerializeField] GameObject trailingBlock;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
        audioManager = GameObject.FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {

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

    internal bool CanMoveInDir(Vector2 direction)
    {
        Debug.DrawRay(rb.transform.position, direction, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(direction.x, direction.y, 0), direction, 0.1f);

        if (hit.collider == null)
            return true;

        if(hit.collider.tag == "Pushable")
        {
            Debug.Log("Test");
            return hit.collider.GetComponent<PushableMovement>().CanMoveInDir(direction);
        }

        return false;
    }

    private void MoveDirectionCheck(Vector2 direction)
    {
        //Increase Turns - Regardless of if Move actually happens
        gameManager.IncreaseTurns();
        audioManager.PlayCatFootSteps();

        Debug.DrawRay(rb.transform.position, direction, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(direction.x, direction.y, 0), direction, 0.1f);
        //Debug.Log("Pushable Raycast Collided With: " + hit.collider);

        lastMovementDirection = direction;

        // Non-Block Space -- Move Player
        if (hit.collider != null && hit.collider.tag == "Obstacle")
        {
            return;
        }

        // Non-Block Space -- Move Player
        if (hit.collider == null)
        {
            MovePlayer(direction);
            return;
        }

        // Check if Another Player
        if (hit.collider.gameObject != this.gameObject && hit.collider.tag == "Player"){
            MovePlayer(direction);
            return;
        }


        // Blocking Space Wall - Move Player (then destroy)
        if (hit.collider.tag == "Wall")
        {
            MovePlayer(direction);
            return;
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
            MovePlayer(direction);
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

    void MovePlayer(Vector2 direction)
    {

        //Move Player - But first get current postion
        Vector2 posBeforeMove = new Vector2(transform.position.x, transform.position.y - 0.01f);
        transform.Translate(direction);

        if (!dropsTrailingBlocks)
            return;

        if (this.gameObject.activeSelf)
            DropColoredBlock(posBeforeMove);
    }

    void DropColoredBlock(Vector2 dropBlockAtPos)
    {
        Instantiate(trailingBlock, dropBlockAtPos, Quaternion.identity);
    }












    /// <summary>
    /// Triggers
    /// </summary>
    /// <param name="collision"></param>

    private void OnTriggerEnter2D(Collider2D collision)
    {   
        // ~ Destroy Player on Wall Collision
        if(collision.tag == "Wall")
        {
            gameManager.GameOver();
            return;
        }

        // If Players Collide - Lose if not all Pickups are Paired; Win Otherwise;
        if (collision.tag == "Player")
        {
            bool canCombinePlayersToWin = false;
            canCombinePlayersToWin = gameManager.CheckIfCatsCanMerge();

            // ~ TEMP: Winning based on colliding with Each Other without Pairables
            canCombinePlayersToWin = true;

            if (canCombinePlayersToWin)
                gameManager.CheckWinCondition();
            else
                gameManager.GameOver();
        }

    }
}
