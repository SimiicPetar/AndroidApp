using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using Patterns.Singleton;

public class GameManager : Singleton<GameManager>
{
    // Start is called before the first frame update

    public  delegate void MutedBGMusic(bool muteState);
    public MutedBGMusic onMutedBGMusic;
    public bool StickerIndicator { get => stickerIndicator; set => stickerIndicator = value; }
    public char FinishedLetter { get => finishedLetter; set => finishedLetter = value; }
    public bool BlendingIndicator { get => blendingIndicator; set => blendingIndicator = value; }
    public bool ReturningFromGameIndicator { get => returningFromGameIndicator; set => returningFromGameIndicator = value; }
    public bool GoStraightToMapIndicator { get => goStraightToMapIndicator; set => goStraightToMapIndicator = value; }
    public ArmadilloHoleScript CurrentArmadilloHole { get => currentArmadilloHole; set => currentArmadilloHole = value; }
    public bool ReturningFromHoleIndicator { get => returningFromHoleIndicator; set => returningFromHoleIndicator = value; }
    public ArmadilloHoleScript CurrentHole { get => currentHole; set => currentHole = value; }
    public int CurrentHoleIndex { get => currentHoleIndex; set => currentHoleIndex = value; }
    public bool EndOfLalaGameIndicator { get => endOfLalaGameIndicator; set => endOfLalaGameIndicator = value; }
    public bool DontGiveReward { get => dontGiveReward; set => dontGiveReward = value; }
    public string LettersForConsolidation { get => lettersForConsolidation; set => lettersForConsolidation = value; }
    public bool FinishedBlendingJustNow { get => finishedBlendingJustNow; set => finishedBlendingJustNow = value; }
    public bool MuteBGMusicBool { get => muteBGMusicBool; set => muteBGMusicBool = value; }
    public bool WatchedEndGame { get => watchedEndGame; set => watchedEndGame = value; }
    public string LastSceneName1 { get => LastSceneName; set => LastSceneName = value; }

    public AudioSourcePrefab audioPrefab;


    UnitManager unitManager;

    UIMapManager uiMapManager;

    public bool allowAllGames = false;

    public TargetWords targetWords;

    public UnitInfo currentUnit;

    ArmadilloHoleScript currentArmadilloHole;

    public List<char> ChosenLetters;

    string LastSceneName = "";

    string CurrentSceneName = "";

    bool isInGame = false;

    bool stickerIndicator;

    char finishedLetter;

    bool blendingIndicator = false;

    bool finishedBlendingJustNow = false;

    bool returningFromGameIndicator = false;

    bool goStraightToMapIndicator = false;

    bool returningFromHoleIndicator = false;

    bool endOfLalaGameIndicator = false;

    bool muteBGMusicBool = false;

    ArmadilloHoleScript currentHole;

    int currentHoleIndex = -1;

    bool dontGiveReward = false;

    bool watchedEndGame = false;

    Task helperTask;

    string lettersForConsolidation;

    public static float NaratorWaitTimeWhenStartToTalk = 1.2f;
    public Sprite GetLetterImage(string letterChar, bool isUppercase)
    {
        if (!isUppercase)
        {
            return Resources.Load<Sprite>($"LetterImages/BelaMalaSlovaAnteater/{letterChar}");
        }else
        {
            return Resources.Load<Sprite>($"LetterImages/capital letters white/{letterChar.ToUpper()}");
        }
    }

    public void SetLettersForConsolidation(string unit1ID, string unit2ID, int ind)
    {
        lettersForConsolidation = unit1ID + unit2ID + unit1ID + unit2ID;
        currentHoleIndex = ind;
    }

    public void ResetBoolsOnCharacterChange()
    {
        MapAvatarController.FinishedWholeGame = false;
        endOfLalaGameIndicator = false;
        watchedEndGame = UnitStatisticsBase.Instance.GetMostCurrentHoleId() == 4;
    }

    private void Start()
    {
        AvatarSpineDressUp.onActiveCharacterChanged += ResetBoolsOnCharacterChange;
        DontDestroyOnLoad(this);
        Input.multiTouchEnabled = false;
        uiMapManager = UIMapManager.Instance;
        unitManager = UnitManager.Instance;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    IEnumerator SmallDelay()
    {
        yield return new WaitForSeconds(0.4f);
    }


    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        uiMapManager = UIMapManager.Instance;
        if (CurrentSceneName.Equals("") && LastSceneName.Equals(""))
        {
            CurrentSceneName = arg0.name;
            LastSceneName = arg0.name;
        }
        else
        {
            LastSceneName = CurrentSceneName;
            CurrentSceneName = arg0.name;
            if (LastSceneName.Equals("ChooseAvatarScene"))
            {
                return;
            }
            if (CurrentSceneName.Equals("MapScene") && isInGame)
            {
                smallWait();

            }
        }

        isInGame = GameObject.FindWithTag("GameMarker") != null ? true : false;      
    }


    private void OnDestroy()
    {       
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void smallWait()
    {
        var stickerAlbum = StickerAlbumManager.Instance;
        if(LastSceneName == "LetterSoundScene")
        {
            UIMapManager.Instance.OpenUnitWindow(currentUnit);
            goStraightToMapIndicator = false;
            return;
        }
        if (LastSceneName == "ManateeGame")
        {
            if (goStraightToMapIndicator)
            {
                UIMapManager.Instance.OpenUnitWindow(currentUnit);
                goStraightToMapIndicator = false;
                return;

            }
            if (!stickerIndicator)
            {
                UIMapManager.Instance.OpenStickerAlbumWindow();
                stickerAlbum.GiveRewardAfterTheGame(finishedLetter);
                ReturningFromGameIndicator = true;
            }
        } else if (LastSceneName == "BlendingGame")
        {
            if (UnitProgressManager.Instance.CheckIfUnclockableGameIsFinished(BasicGames.BLENDING) && !blendingIndicator)
            {
                int index = UnitManager.Instance.AllUnitsList.IndexOf(currentUnit);
                ReturningFromGameIndicator = true;
                stickerAlbum.RewardAfterAnaconda();
                finishedBlendingJustNow = true;
                
            }else
                UIMapManager.Instance.OpenUnitWindow(currentUnit);
        } else if (LastSceneName == "EndGameScene")
        {
            if (!dontGiveReward)
            {
                stickerAlbum.RewardAfterAnaconda(endOfLalaGameIndicator);
                ReturningFromGameIndicator = true;
                MapAvatarController.Instance.PlaceAvatarAtTheStart();
                UIMapManager.Instance.SetupLalaArchAfterEndGame();
            }
            dontGiveReward = false;    

         
        }
        else
        {
            if(LastSceneName != "LalaLetterMachine")
                UIMapManager.Instance.OpenUnitWindow(currentUnit);
        }

        if(LastSceneName == "ChooseAvatarScene" && UnitStatisticsBase.Instance.CheckIfFinishedGame())
        {
            MapAvatarController.Instance.PlaceAvatarAtTheStart();
        }
        goStraightToMapIndicator = false;
        ReturningFromGameIndicator = false;
    }
    private void OnEnable()
    {
        uiMapManager = UIMapManager.Instance;
        unitManager = UnitManager.Instance;
    }
    public void SetCurrentUnit(UnitInfo unit)
    {
        currentUnit = unit;
        PassChosenLettersToUnit();
    }

    public void PassChosenLettersToUnit()
    {
        UnitManager.Instance.LoadWordsForUnit(currentUnit);
    }

    public void MuteBGMusic()
    {
        muteBGMusicBool = !muteBGMusicBool;
        onMutedBGMusic?.Invoke(muteBGMusicBool);
    }
    
}
