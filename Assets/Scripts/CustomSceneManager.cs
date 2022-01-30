using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CustomSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1; // Get next scene in Build Index
        SceneManager.LoadScene(nextScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
