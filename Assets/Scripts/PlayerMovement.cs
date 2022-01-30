using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //TODO:
    // - Create a Toggle for Pushable Blocks to Combine for Testing


    // Bools - Trying out Systems

    // Managers - Private
    GameManager gameManager;
    AudioManager audioManager;
    
    // RigidBody2D - Private
    private Rigidbody2D rb;

    // Player Number - Used to Identify Players in Pushable Collisions
    public int playerNum;

    #region Archive - Variables
    [Header("Trailing Blocks")]
    [SerializeField] bool dropsTrailingBlocks;
    [SerializeField] GameObject trailingBlock;
    #endregion


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
        // Move - Attempt Move in Given Direction (WASD)
        if (Input.GetKeyDown(KeyCode.W))
            MoveDirectionCheck(Vector2.up);
        else if (Input.GetKeyDown(KeyCode.S))
            MoveDirectionCheck(Vector2.down);
        else if (Input.GetKeyDown(KeyCode.A))
            MoveDirectionCheck(Vector2.left);
        else if (Input.GetKeyDown(KeyCode.D))
            MoveDirectionCheck(Vector2.right);
    }

    // Check if Player can Move
    private void MoveDirectionCheck(Vector2 direction)
    {
        //Increase Turns - Regardless of if Move actually happens
        gameManager.IncreaseTurns();

        // Audio - Play Cat Footsteps with each WASD keystroke
        audioManager.PlayCatFootSteps();

        // Raycast - Look ahead to see if Player can move into intended spot
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(direction.x, direction.y, 0), direction, 0.1f);

        #region Archive
        // Blocked Space -- "!= null" means it's not a free space
        /*
        if (hit.collider != null && hit.collider.tag == "Obstacle") {
        {
            return;
        }
        */
        #endregion

        // Blocking Space -- Obstacle - Do NOT move player
        if (hit.collider != null && hit.collider.tag == "Obstacle")
            return;

        // Non-Block Space -- Move Player
        if (hit.collider == null)
        {
            MovePlayer(direction);
            return;
        }

        // Blocking Space -- Move Into Anothe Players Space - Collisions will Occur on Players
        if (hit.collider.gameObject != this.gameObject && hit.collider.tag == "Player")
        {
            MovePlayer(direction);
            return;
        }

        // Blocking Space -- Wall - - Collisions will Occur on Player via Wall
        if (hit.collider.tag == "Wall" && gameManager.destroyPlayerOnWallCollision)
        {
            MovePlayer(direction);
            return;
        }

        // Pusable Object -- Push the Object and Move Player if possible
        if (hit.collider.tag == "Pushable")
        {
            // Pushable Object -- Get the Pushable Object for checks
            PushableMovement pushableObj = hit.collider.GetComponent<PushableMovement>();

            // Pushable Object -- Check with the obj if it can be pushed
            bool canPush = pushableObj.PotentialMoveCollisionCheck(direction, playerNum);

            // Pushable Object -- Attempting Push into Blocking Space - return;
            if (!canPush)
                return;

            // Pushable Object -- Attempting PUsh into Non-Blocking Space - Move pushableObject & Move Player;
            pushableObj.PlayerPushesThisObject(direction);
            MovePlayer(direction);
            return;
        }
    }

    // Move Player
    void MovePlayer(Vector2 direction)
    {
        transform.Translate(direction);
        #region Archive
        /*
        //Move Player - First get Current Position [Put at top of this method if using Block Trails
        Vector2 posBeforeMove = new Vector2(transform.position.x, transform.position.y - 0.01f);
        if (!dropsTrailingBlocks)
            return;

        if (this.gameObject.activeSelf)
            DropColoredBlock(posBeforeMove);
        */
        #endregion
    }

    // Called from PushableMovement -- Use in situations where player is pushed by a Pushable Object
    internal bool CanMoveInDir(Vector2 direction)
    {
        // Player -- is being Pushed - Check if they are being pushed into open space (true); into a Pushable (true); or Cannot Move (false)
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(direction.x, direction.y, 0), direction, 0.1f);

        // Player -- Pushed into Free Space - Move (true)
        if (hit.collider == null)
            return true;

        // Player -- Pushed into a Pushable Objet - Move Player; Move Pushable Obj if it passes its own CanMoveInDir()
        if (hit.collider.tag == "Pushable")
            return hit.collider.GetComponent<PushableMovement>().CanMoveInDir(direction);

        return false;
    }

    //Collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ~ Destroy Player on Wall Collision
        if (collision.tag == "Wall" && gameManager.destroyPlayerOnWallCollision)
        {
            Debug.Log("Collision with Wall");
            gameManager.PlayerLoses();
            return;
        }

        // If Players Collide - Lose if not all Pickups are Paired; Win Otherwise;
        if (collision.tag == "Player")
        {
            if (gameManager.CheckIfCatsCanMerge())
                gameManager.PlayerWins();
            else
                gameManager.PlayerLoses();
        }
    }

    #region Archived Methods
    void DropColoredBlock(Vector2 dropBlockAtPos)
    {
        Instantiate(trailingBlock, dropBlockAtPos, Quaternion.identity);
    }
    #endregion

}
