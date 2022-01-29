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

    // Update is called once per frame
    void Update()
    {

    }

    public bool PotentialMoveCollisionCheck(Vector2 direction, int playerNum)
    {
        if (this.associatedPlayerNum != playerNum)
            return false;
         

        Debug.DrawRay(rb.transform.position, direction, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(direction.x, direction.y, 0), direction, 0.1f);
        //Debug.Log("Pushable Raycast Collided With: " + hit.collider);

        // Blocked - By Another Pushable (Combine to Win)
        if (hit.collider != null)
            if (hit.collider.tag == "Pushable")
                return true;

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


        if (collision.GetComponent<PushableMovement>() == null)
            return;

        // Pushable Objects are now paired up
        pairedUp = true;
        collision.GetComponent<PushableMovement>().pairedUp = true;

        //Disable Pushable Objects
        gameObject.SetActive(false);
        collision.gameObject.SetActive(false);

        GameObject.FindObjectOfType<GameManager>().CheckWinCondition();   
        //Destroy the Pushable Objects
        //Destroy(gameObject);
        //Destroy(collision.gameObject);

    }
}
