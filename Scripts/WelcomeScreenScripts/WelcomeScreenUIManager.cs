using Patterns.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomeScreenUIManager : Singleton<WelcomeScreenUIManager>
{
    // Start is called before the first frame update

    public UIWindow HowToPlayWindow;
    public UIWindow InformationWindow;
    UIWindow _currentOpenWindow = null;
    void Start()
    {
        HowToPlayWindow.gameObject.SetActive(false);
        InformationWindow.gameObject.SetActive(false);
    }

    public void OpenHowToPlayWindow()
    {
        HowToPlayWindow.gameObject.SetActive(true);
        _currentOpenWindow = HowToPlayWindow;
        UIMapManager.OnWindowOpened(HowToPlayWindow);
    }

    public void OpenInformationWindow()
    {
        InformationWindow.gameObject.SetActive(true);
        _currentOpenWindow = InformationWindow;
        UIMapManager.OnWindowOpened(HowToPlayWindow);
    }

    public void CloseCurrentWindow()
    {
        _currentOpenWindow.gameObject.SetActive(false);
        _currentOpenWindow = null;
        UIMapManager.OnWindowClosed(_currentOpenWindow);
    }

    // Update is called once per frame
}
