using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    Vector3 lastMovementDirection;
   
    public int associatedPlayerNum;
    public bool pairedUp;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public bool PotentialMoveCollisionCheck(Vector2 direction, int playerNum)
    {
        // Cannot Push - if not same color as Player Pushing
        if (this.associatedPlayerNum != playerNum)
            return false;

        // Raycast Outward - see if the pushed block will bump into a Blocked Space
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(direction.x, direction.y, 0), direction, 0.1f);
        //Debug.DrawRay(rb.transform.position, direction, Color.red);
        //Debug.Log("Pushable Raycast Collided With: " + hit.collider);


        // Blocked - By Another Pushable - Can be pushed, and combined to win
        /*
        if (hit.collider != null)
            if (hit.collider.tag == "Pushable")
                return true;
        */
        if(hit.collider != null)
        {
            if (hit.collider.tag == "Player")
            {
                if (hit.collider.GetComponent<PlayerMovement>().CanMoveInDir(direction))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

        }

        // Blocked - Potential Movement Space 
        if (hit.collider != null)
            return false;

        // Open - Potential Movement Space
        return true;
    }


    public void MovePushable(Vector2 direction)
    {
        transform.Translate(direction);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("You Win");

        //~ TEMP: Pushables can be destroyed by walls
        CheckCollisionWithWall(collision);

        if (collision.GetComponent<PushableMovement>() == null)
            return;

        // ~ TEMP: Winning based on colliding with Each Other

        // Pair Up Pushable Objects and Disable
        //PairUpPushableObjets(collision);
    }

    void CheckCollisionWithWall(Collider2D collision)
    {
        if (collision.gameObject.tag != null)
            if (collision.gameObject.tag == "Wall")
                Destroy(this.gameObject);

    }

    public bool CanMoveInDir(Vector2 direction)
    {
        Debug.DrawRay(rb.transform.position, direction, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(direction.x, direction.y, 0), direction, 0.1f);

        if (hit.collider == null)
            return true;

        // ~ TEMP: Colliding into walls
        if (hit.collider.tag == "Wall")
        {
            Debug.Log("True");
            return false;
        }

        if (hit.collider.tag == "Pushable")
            return true;

        return false;
    }

    void PairUpPushableObjets(Collider2D collision)
    {
        // Pushable Objects are now paired up
        pairedUp = true;
        collision.GetComponent<PushableMovement>().pairedUp = true;

        //Disable Pushable Objects
        gameObject.SetActive(false);
        collision.gameObject.SetActive(false);
    }
}
