using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class CanvasManager : MonoBehaviour
{
    [SerializeField] GameObject WinScreen;
    [SerializeField] GameObject LoseScreen;
    [SerializeField] TextMeshProUGUI turnsValueTMP;



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

    public void SetLoseScreen(bool state)
    {
        WinScreen.SetActive(false);
        LoseScreen.SetActive(state);
    }

    public void ResetScreens()
    {
        LoseScreen.SetActive(false);
        WinScreen.SetActive(false);
    }

    public void SetTurns(int turns)
    {
        this.turnsValueTMP.text = turns.ToString();
    }
}
