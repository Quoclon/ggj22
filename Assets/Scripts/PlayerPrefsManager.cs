using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class PlayerPrefsManager : MonoBehaviour
{
    public TMP_InputField textBox;

    public void DeletePlayerPreNickname()
    {
        PlayerPrefs.DeleteKey("name");
        Debug.Log("DeletedPlayerPrefsNickname");
    }

    public void ClickSaveButton()
    {
        Debug.Log("Text: " + textBox.text);

        // If No Nickname Entered -- Create Random Nickname
        if(textBox.text.Length < 1 || textBox.text == null)
            textBox.text = "Cat " + Random.Range(1, 9999).ToString();

        // Set PlayerPrefs Unique CustomID -- for PlayFab Leadeboard
        // Used this instead of more common PlayFab SystemInfo.deviceUniqueIdentifier
        //SystemInfo.deviceUniqueIdentifier -- assumes that all WebGL is the save unique "device"

        string playerUniqueID = Random.Range(1, 99999).ToString();

        PlayerPrefs.SetString("unique_id", playerUniqueID);
        Debug.Log("Your Unique ID for PlayFab Leaderboard: " + PlayerPrefs.GetString("unique_id"));

        // Set PlayerPrefs Nickname -- for PlayFab Leadeboard
        PlayerPrefs.SetString("name", textBox.text);
        Debug.Log("Your Display Name for PlayFab Leaderboard: " + PlayerPrefs.GetString("name"));

        // Load the Next Scene in Build Order (Level 1, Intro, etc.)
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextScene);

    }

}
