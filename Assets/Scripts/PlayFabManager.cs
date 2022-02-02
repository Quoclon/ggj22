using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;

public class PlayFabManager : MonoBehaviour
{
    GameManager gameManager;

    [Header("Leaderboard Related")]
    public GameObject leaderboardPanel;
    public GameObject listingPrefab;
    public Transform listingContainer;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        CreatePlayerAndUpdateDisplayName();
        //Login();
    }

    #region Create a Player with a Display Name
    void CreatePlayerAndUpdateDisplayName()
    {
        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest
        {
            //CustomId = "PlayFabGetPlayerProfileCustomId",
            //CustomId = SystemInfo.deviceUniqueIdentifier,
            CustomId = PlayerPrefs.GetString("unique_id"),
            CreateAccount = true
        }, result => {
            Debug.Log("Successfully logged in a player with PlayFabId: " + result.PlayFabId);
            UpdateDisplayName();
        }, error => Debug.LogError(error.GenerateErrorReport()));
    }

    void UpdateDisplayName()
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = PlayerPrefs.GetString("name"),
        }, result => {
            Debug.Log("The player's display name is now: " + result.DisplayName);
        }, error => Debug.LogError(error.GenerateErrorReport()));
    }
    #endregion

    #region Login - old method
    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
        Debug.Log("Successful Login/Account Creation!");
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error while logging in/Creating Account");
        Debug.Log(error.GenerateErrorReport());
    }
    #endregion


    public void SendLeaderBoard(int score)
    {

        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = gameManager.levelName,
                    Value = (int)-gameManager.turns
                }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLoaderboardUpdate, OnError);

        /*
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = gameManager.levelName,
                    Value = (int)-gameManager.turns
                }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLoaderboardUpdate, OnError);
        */
    }

    void OnLoaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        StartCoroutine(ExampleCoroutine());

        //GetLeaderboard();
        Debug.Log("Successful Leaderboard Send");
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = gameManager.levelName,
            StartPosition = 0,
            MaxResultsCount = 5
        };

        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        leaderboardPanel.SetActive(true);

        foreach (var player in result.Leaderboard)
        {
            GameObject tempListing = Instantiate(listingPrefab, listingContainer);
            LeaderboardListing LL = tempListing.GetComponent<LeaderboardListing>();
            LL.playerNameText.text = player.DisplayName;
            LL.playerScoreText.text = (-player.StatValue).ToString(); // Invert back to regular score style

            Debug.Log(player.Position + " " + player.PlayFabId + " " + player.StatValue);
        }
    }

    IEnumerator ExampleCoroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(1);

        GetLeaderboard();

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

    
    public void CloseLeaderboardPanel()
    {
        leaderboardPanel.SetActive(false);
        for (int i = listingContainer.childCount - 1; i >= 2; i--)
        {
            Destroy(listingContainer.GetChild(i).gameObject);
        }

        gameManager.ResetScene();
    }
    
}
