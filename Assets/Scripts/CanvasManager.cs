using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Using Code from "assets/plugins/webgl/MyPlugin.jslib" - to detect Mobile on WebGL
//using System.Runtime.InteropServices;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] GameObject WinScreen;
    [SerializeField] GameObject LoseScreen;
    [SerializeField] TextMeshProUGUI turnsValueTMP;
    [SerializeField] GameObject mobileButtons;
    //[SerializeField] bool isMobileControls;

    #region WebGL Mobile Controls - FAILED
    /*
    // Canvas Mobile Controls - Only use Mobile Buttons on WebGL if Phone
    [DllImport("__Internal")]
    private static extern bool IsMobile();
    public bool isMobile()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
                    return IsMobile();
#endif
        return false;
    }
    */
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (Application.isMobilePlatform)
            mobileButtons.SetActive(true);
        else
            mobileButtons.SetActive(false);

        /*
        if (isMobile())
        {
            mobileButtons.SetActive(true);
            //isMobileControls = true;
        }else
        {
            mobileButtons.SetActive(false);
            //isMobileControls = false;
        }
        */
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
