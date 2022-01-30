using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public PushableMovement[] pushableObjects;
    public PlayerMovement[] cats;

    CanvasManager canvasManager;

    public float turns = 0;


    // Start is called before the first frame update
    void Start()
    {
        canvasManager = GameObject.FindObjectOfType<CanvasManager>();
        pushableObjects = GameObject.FindObjectsOfType<PushableMovement>();
        cats = GameObject.FindObjectsOfType<PlayerMovement>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            ResetScene();
    }

    public bool AllPushablesArePaired()
    {
        foreach (var pushable in pushableObjects)
        {
            Debug.Log(pushable.pairedUp);

            if (!pushable.pairedUp)
                return false;
        }

        return true;
    }

    public bool CheckIfCatsCanMerge()
    {
        foreach (var pushable in pushableObjects)
        {
            if (!pushable.pairedUp)
                return false;
        }

        //canvasManager.SetWinScreen(true);
        //DisablePlayers();

        //IF All are paired up, you win!
        return true;
    }

    public void CheckWinCondition()
    {
        //IF All are paired up, you win!
        canvasManager.SetWinScreen(true);
        DisablePlayers();
    }

    public void GameOver()
    {
        // Called if a lose condition is met
        canvasManager.SetLoseScreen(true);
        DisablePlayers();
    }

    public void DisablePlayers()
    {
        foreach (var cat in cats)
        {
            cat.gameObject.SetActive(false);
        }
    }

    public void ResetScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void IncreaseTurns()
    {
        turns += 0.5f;
        canvasManager.SetTurns((int)turns);
    }
}
