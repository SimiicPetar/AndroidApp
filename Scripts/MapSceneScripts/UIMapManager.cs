using Patterns.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMapManager : Singleton<UIMapManager>
{
    public delegate void onStickerAlbumClosed();
    public static onStickerAlbumClosed OnStickerAlbumClosed;

    public delegate void onWindowOpened(UIWindow window);
    public static onWindowOpened OnWindowOpened;

    public delegate void onWindowClosed(UIWindow window);
    public static onWindowClosed OnWindowClosed;



    public List<UIWindow> UIWindows;

    public UIWindow UnitView;
    public UIWindow StickerAlbum;
    public UIWindow ParentCenter;

    public bool UIOpen = false;

    public GameObject ArchGlowForEndGame;

    public Animator LalaTextOnArchAnimator;

    public void UIStatusCheck()
    {
        
    }

    public void SetupLalaArchAfterEndGame()
    {
        if (UnitStatisticsBase.Instance.GetMostCurrentHoleId() == 4 && GameManager.Instance.WatchedEndGame)
        {
            ArchGlowForEndGame.SetActive(true);
            LalaTextOnArchAnimator.enabled = true;
            LalaTextOnArchAnimator.GetComponent<Button>().enabled = true;
        }
        else
        {
            LalaTextOnArchAnimator.GetComponent<Button>().enabled = false;
            ArchGlowForEndGame.SetActive(false);
            LalaTextOnArchAnimator.enabled = false;
        }
    }

    void Start()//start
    {
        Debug.Log("ja sam uiMapManager");
        Debug.Log("Da li imam referencu na ovaj objekat :" + UnitView);
        SetupLalaArchAfterEndGame();


        ParentCenter.gameObject.SetActive(false);
        StickerAlbum.gameObject.SetActive(false);
    }

    public void GoBackToChooseAvatar() //LoadScene  ChooseAvatarScene
    {
        GameManager.Instance.currentUnit = null;
        SceneManager.LoadScene("ChooseAvatarScene");
    }

    public void GoToUnitAfterSticking()
    {
        UIOpen = true;
        StickerAlbum.gameObject.SetActive(false);
        OnStickerAlbumClosed?.Invoke();
        OnWindowClosed(StickerAlbum);
        if (!GameManager.Instance.EndOfLalaGameIndicator)
        {
            OpenUnitWindow(GameManager.Instance.currentUnit);
            GameManager.Instance.EndOfLalaGameIndicator = false;
            StickerAlbumManager.Instance.VisibleLeftPage.ResetLeftPage();

        }
        else
        {
            GameManager.Instance.EndOfLalaGameIndicator = false;
            AudioManagerMap.Instance.PlayLalaRecommendToPlayAgain();
            StickerAlbumManager.Instance.VisibleLeftPage.ResetLeftPage();
        }
            
    }

    public void GoToEndGameSceneFromRiverButton()// LoadScene  EndGameScene 
    {
        GameManager.Instance.DontGiveReward = true;
        SceneManager.LoadScene("EndGameScene");
       
    }

    public void OpenStickerAlbumWindow()//  StickerAlbum  SetActive  true . Invoke StickerAlbum
    {
        UIOpen = true;
        StickerAlbum.gameObject.SetActive(true);
        OnWindowOpened.Invoke(StickerAlbum);
    }

    public void CloseStickerForAnaconda()
    {
        UIOpen = false;
        StickerAlbum.gameObject.SetActive(false);
        OnStickerAlbumClosed?.Invoke();
     
        OnWindowClosed.Invoke(StickerAlbum);
        if (GameManager.Instance.EndOfLalaGameIndicator)
        {
            AudioManagerMap.Instance.PlayLalaRecommendToPlayAgain();
        }
        else if (!GameManager.Instance.StickerIndicator && !GameManager.Instance.FinishedBlendingJustNow && StickerAlbumManager.Instance.IsAnacondaReward) // ako smo upravo zavrsili blending
        {
            CloseUnitWindow();
        }
        else
            OpenUnitWindow(GameManager.Instance.currentUnit);



        if (GameManager.Instance.FinishedBlendingJustNow)
        {
            GameManager.Instance.FinishedBlendingJustNow = false;
            CloseUnitWindow();
        }
            

    }

    public void CloseStickerAlbumWindow()
    {
        UIOpen = false;
        StickerAlbum.gameObject.SetActive(false);
        OnStickerAlbumClosed?.Invoke();
        if (GameManager.Instance.ReturningFromGameIndicator)
            OpenUnitWindow(GameManager.Instance.currentUnit);
        else
        {
            OnWindowClosed.Invoke(StickerAlbum);
        }
           


    }

    public void OpenParentCenter()
    {
        UIOpen = true;
        ParentCenter.gameObject.SetActive(true);
        ParentCenterManager.Instance.OpenFirstTab();
        OnWindowOpened.Invoke(ParentCenter);
    }

    public void CloseParentCenter() // Invoke  ParentCenter;  bool UIOpen false .
    {
        UIOpen = false;
        ParentCenter.gameObject.SetActive(false);
        ParentCenterManager.Instance.CloseActiveTab();
        OnWindowClosed.Invoke(ParentCenter);
    }
    public void CloseUnitWindow(bool isBeforeSticking = false)// bool UIOpen false . UnitView  SetActive false . ResetView
    {
        //ovo je da ne bi ova promenljiva bila promenjena na false nakon zatvaranja unita kad treba da se lepi slicica nakon toga da ne bi smo dobili onaj pointer preko UI-ja
        if(!isBeforeSticking)
            UIOpen = false;
        UnitView.gameObject.SetActive(false);
        NextUnitIndicatorController.Instance.CorrectUnitClosed();
        UnitView.ResetView();
        AudioManagerMap.Instance.PlayBGMusic();
        OnWindowClosed.Invoke(UnitView);
    }

    public void OpenUnitWindow(UnitInfo info)//UnitView  SetActive true .   bool UIOpen true.
    {
        UIOpen = true;
        UnitView.gameObject.SetActive(true);
        UIManager.Instance.UnitViewSetup();
        
        OnWindowOpened(UnitView);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
