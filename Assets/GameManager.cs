using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PushableMovement[] pushableObjects;
    CanvasManager canvasManager;


    // Start is called before the first frame update
    void Start()
    {
        canvasManager = GameObject.FindObjectOfType<CanvasManager>();
        pushableObjects = GameObject.FindObjectsOfType<PushableMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckWinCondition()
    {
        foreach (var pushable in pushableObjects)
        {
            Debug.Log(pushable.pairedUp);

            if (!pushable.pairedUp)
                return;
        }

        //IF All are paired up, you win!
        canvasManager.SetWinScreen(true);
    }
}
