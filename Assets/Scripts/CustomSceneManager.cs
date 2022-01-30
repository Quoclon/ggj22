using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CustomSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Go to Player Cat Naming Screen by Default
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;

        // If the Player Cat already has a name, then skip right into game (consistent leaderboards)
        if (PlayerPrefs.GetString("name") != null)
        {
            if(PlayerPrefs.GetString("name").Length > 0)
            {
                nextScene = SceneManager.GetActiveScene().buildIndex + 2; // Get next scene in Build Index
            }
        }
        SceneManager.LoadScene(nextScene);
    }
}
