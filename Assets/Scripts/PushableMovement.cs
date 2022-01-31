using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableMovement : MonoBehaviour
{
    // Variables
    GameManager gameManager;

    private Rigidbody2D rb;
    public int associatedPlayerNum;
    public bool pairedUp;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Pushable Block - Check if it can be Pushed
    public bool PotentialMoveCollisionCheck(Vector2 direction, int playerNum)
    {
        // Cannot Push - if not same color as Player Pushing
        if (this.associatedPlayerNum != playerNum)
            return false;

        // Raycast Outward - see if the pushed block will bump into a Blocked Space
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(direction.x, direction.y, 0), direction, 0.1f);

        
        // Blocking Space -- Pushing Check, for specific entiries (i.e. Pushable, Player)
        if(hit.collider != null)
        {
            // Pushed into Pushable -- Combine if Game Mode 'pushToCombine'
            if (hit.collider.tag == "Pushable" && gameManager.combinePushablesOnPushableCollision)
            {
                if (this.associatedPlayerNum == hit.collider.gameObject.GetComponent<PushableMovement>().associatedPlayerNum)
                    return false;
                else
                    return true;
            }
                

            // Pushed into Pushable -- Try to push Player, if they have a Free Space to move
            if (hit.collider.tag == "Player")
            {
                if (hit.collider.GetComponent<PlayerMovement>().CanMoveInDir(direction))
                    return true;
                else
                    return false;
            }
        }

        // Blocked -- Cannot be Pushed into a non Free (Null) Space
        if (hit.collider != null)
            return false;

        // Open -- Potential Movement Space
        return true;
    }


    public void PlayerPushesThisObject(Vector2 direction)
    {
        transform.Translate(direction);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //~ TEMP: Pushables can be destroyed by walls
        if(gameManager.destroyPushableOnWallCollision)
            CheckCollisionWithWall(collision);

        if (collision.GetComponent<PushableMovement>() == null)
            return;

        // ~ TEMP: Winning based on colliding with Each Other
        if(gameManager.combinePushablesOnPushableCollision)
            PairUpPushableObjets(collision);
    }

    void CheckCollisionWithWall(Collider2D collision)
    {
        // ~ TEMP: Destroy Pushable Objects if Collision with Wall (if Game Mode is set)
        if (collision.gameObject.tag != null)
            if (collision.gameObject.tag == "Wall" && gameManager.destroyPushableOnWallCollision)
                Destroy(this.gameObject);
    }

    public bool CanMoveInDir(Vector2 direction)
    {
        // Pushable -- is being Pushed - Check if they are being pushed into open space
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(direction.x, direction.y, 0), direction, 0.1f);

        // Free Space -- Move (true)
        if (hit.collider == null)
            return true;

        // ~ TEMP: Blocked Space -- Wall - Allow Movement only if we plan to destroy Pushable (Game Mode)
        if (hit.collider.tag == "Wall" && !gameManager.destroyPushableOnWallCollision)
            return false;

        // Free Space -- Being pushed into another Pushable (which will make its own move checks?)
        if (hit.collider.tag == "Pushable")
            return true;

        // Blocked Space -- Cannot Move
        return false;
    }

    // Pair Up Pushables - Game Mode - Pushables Paired Up to Win
    void PairUpPushableObjets(Collider2D collision)
    {

        //Play Sound when Combined -- 
        gameManager.audioManager.PlayPushableCombiningSound();

        // Pushable Objects are now paired up
        pairedUp = true;
        collision.GetComponent<PushableMovement>().pairedUp = true;

        //Disable Pushable Objects
        gameObject.SetActive(false);
        collision.gameObject.SetActive(false);


        //Check Win Condition -- Game Mode Pushables Combine to Win
        bool allPushablesPaired = gameManager.CheckIfAllPushablesArePaired();
        if (allPushablesPaired && !gameManager.combinePlayersToWin)
            gameManager.PlayerWins();

    }
}
