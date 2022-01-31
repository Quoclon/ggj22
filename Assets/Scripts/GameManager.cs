using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [Header("Game Modes")]
    public bool combinePlayersToWin = false;
    public bool combinePushablesToWin = false;
  
    public bool combinePushablesOnPushableCollision = false;
    public bool destroyPlayerOnWallCollision = false;
    public bool destroyPushableOnWallCollision = false;


    // ~ TODO: Make pushableObjects have a "destroyed" status, otherwise Pair Cheking won't work
    [Header("Tracked Objects")]
    public PushableMovement[] pushableObjects;
    public PlayerMovement[] cats;

    // Leaderboard 
    [Header("Leaderboard Data")]
    public float turns = 0;
    public string levelName = "Level";
    public bool playerSentPlayFabDataAlready = false;

    //Managers
    PlayFabManager playfabManager;
    CanvasManager canvasManager;
    public AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        // Tracked Objects
        cats = GameObject.FindObjectsOfType<PlayerMovement>();
        pushableObjects = GameObject.FindObjectsOfType<PushableMovement>();

        // Managers
        canvasManager = GameObject.FindObjectOfType<CanvasManager>();
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        playfabManager = GameObject.FindObjectOfType<PlayFabManager>();

        // Game Level Name - Used for PlayFab leaderboards
        levelName = SceneManager.GetActiveScene().name;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            ResetScene();
    }

    // Win Condition -- All Pushables Combined To Win
    public bool CheckIfAllPushablesArePaired()
    {
        if (!combinePushablesOnPushableCollision)
            return false;

        foreach (var pushable in pushableObjects)
        {
            if (!pushable.pairedUp)
                return false;
        }

        // If All Pushables are Paired -- Game Mode - set to Pushable Pairing - Player Wins (return true)
        return true;
    }

    // Win Condition -- Cats Combined To Win ~ NEED: All Pushables combined as well
    public bool CheckIfCatsCanMerge()
    {
        if (!combinePlayersToWin)
            return false;

        // Pushables All Combined -- So now players can combine
        if (combinePushablesToWin)
        {
            foreach (var pushable in pushableObjects)
            {
                if (!pushable.pairedUp)
                    return false;
            }
        }

        // If All Cats are Paired -- Game Mode - set to Cats Pairing - Player Wins (return true)
        return true;
    }

    public void PlayerWins()
    {
        //IF All are paired up, you win!
        if (!playerSentPlayFabDataAlready)
        {
            playfabManager.SendLeaderBoard((int)turns);
            // ~ Make an Async Call here so we can get the updated leaderboard?
            //playfabManager.GetLeaderboard();
            canvasManager.SetWinScreen(true);
            audioManager.PlayCatPur();
            playerSentPlayFabDataAlready = true;
        }
        DisablePlayers();
    }

    public void PlayerLoses()
    {
        // Called if a lose condition is met
        canvasManager.SetLoseScreen(true);
        DisablePlayers();
        audioManager.PlayCatAngry();
    }

    public void DisablePlayers()
    {
        foreach (var cat in cats)
            cat.gameObject.SetActive(false);
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

    public void LoadNextScene()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextScene);
    }

    public void CloseAllScreens()
    {
        //
    }
}
