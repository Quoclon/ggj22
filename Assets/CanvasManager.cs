using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CanvasManager : MonoBehaviour
{
    [SerializeField] GameObject WinScreen;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {            

    }

    public void SetWinScreen(bool state)
    {
        WinScreen.SetActive(state);
    }
}
