using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BlendingGameManager : MonoBehaviour, IGameManager
{
    // Start is called before the first frame update
    char[] pomocniNizSlovaZaTest = new char[] { 'a', 'b', 'c', 'n', 't', 'i', 'p', 'y', 'r' };

    public delegate void OnStartedPlayingSylable(bool started);
    public  OnStartedPlayingSylable onStartedPlayingSylable;
    public delegate void ResetEvent();
    public ResetEvent onReset;
    bool progressSet = false;
    public bool clickAllowed = true;
    static BlendingGameManager _instance = null;
    public static BlendingGameManager Instance { get { return _instance; } }
    public bool AllowDraggingSliders = false;

    int numberOfTrials = 3;

    bool FinishedGameIndicator = false;

    public int NumberOfTrials { get => numberOfTrials; set => numberOfTrials = value; }

    public int TrialNumber { // realoadButton setactive true
        get { return trialNumber; }
        set {
            trialNumber = value;
            if (trialNumber == 3 && !progressSet){
                
                progressSet = true;
                
                ReloadButton.SetActive(true);
                StartCoroutine(WaitForNextGameButtonEnable());
            }
            }
    }

    int trialNumber = 0;

    public float FULL_WORD_DELAY = 0.2f;
    AudioManagerBlendingGame audioManager;
    public List<AnacondaBehaviourTest> anacondaList;
    public List<Slider> anacondaSliders;
    public AnacondaSounds UnitAnacondaSounds;
    public AnacondaLetters UnitAnacondaLetters;
    public ProgressBarLalaVisualBehaviour lala;
    public GameObject NextGameButton;
    public GameObject ReloadButton;
    public List<KnobReverse> knobs;

    public bool isPlaying = false;

    private void Awake()
    {
        if(_instance != this && _instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        clickAllowed = true;
    }

    IEnumerator WaitForNextGameButtonEnable()// wait for enable
    {
        yield return new WaitForSeconds(audioManager.soundPack.correctAnswerSound.length - 1.2f);
        NextGameButton.SetActive(true);
        NextGameButton.GetComponent<Button>().enabled = true;
    }

    public void SkipBlending() // skip button save Progress and load mapScene
    {
        UnitProgressManager.Instance.AddProgressForUnlockableGame(BasicGames.BLENDING);
        SceneManager.LoadScene("MapScene");
    }

    public void TurnOffHandPointer()
    {

    }

    public void GoToNextGame() // saveProgress and load mapScene
    {
        UnitProgressManager.Instance.AddProgressForUnlockableGame(BasicGames.BLENDING);
        SceneManager.LoadScene("MapScene");
    }

    public void GoBackToMap() //  load scene mapScene 
    {
        SceneManager.LoadScene("MapScene");
    }

    public void DeactivateHeadMask(AnacondaBehaviourTest anaconda, bool enable)//DeactivateHeadMask
    {
        anaconda.EnableSnakeHead(enable);
    }

    public bool GetMaskState(AnacondaBehaviourTest anaconda) //GetMaskState
    {
        return anaconda.IsMaskEnabled();
    }

    //sounds path :
    //letters path:Assets/Resources/ScriptableObjects/BlendingGameObjects/Anaconda Letters/AFO.asset
	
    void GetAnacondaSoundsAndLetters()// UnitAnacondaLetters and UnitAnacondaSounds
    {
        string idToLookFor = GameManager.Instance.currentUnit.unitId;
        UnitAnacondaLetters = Resources.Load<AnacondaLetters>($"ScriptableObjects/BlendingGameObjects/Anaconda Letters/{idToLookFor.ToUpper()}");
        UnitAnacondaSounds = Resources.Load<AnacondaSounds>($"ScriptableObjects/BlendingGameObjects/Anaconda Sounds/{idToLookFor.ToUpper()}");
    }

    void CheckIfGameIsFinishedForStickerAlbum()//check 
    {
        FinishedGameIndicator = UnitStatisticsBase.Instance.GetCurrentUnitStatistic().unitProgress.BlendingCompleted;
        GameManager.Instance.BlendingIndicator = FinishedGameIndicator;
    }

    private void Start()//start
    {
        audioManager = AudioManagerBlendingGame.Instance;
        CheckIfGameIsFinishedForStickerAlbum();
        GetAnacondaSoundsAndLetters();
        SetAnacondaSounds();
        SetAnacondaLetters();
    }
    public void BlendingGameInit()//  interactable foreach  Slider and lala
    {
        foreach (var slider in anacondaSliders)
            slider.interactable = true;
        lala.interactable = true;
    }
    public void PlayFirstLetterSound(AudioClip firstLetterSound)//play audio
    {
        audioManager.PlaySound(firstLetterSound);
        StartCoroutine(SliderHandleBan(firstLetterSound.length));
    }
    
    void SetAnacondaLetters()// gamelogic for letters
    {
        for (int i = 0; i < anacondaList.Count; i++)
        {
            if(i == 0)
            {
                anacondaList[i].GetLetters(UnitAnacondaLetters.FirstAnacondaLetters);
            }else if(i == 1)
            {
                anacondaList[i].GetLetters(UnitAnacondaLetters.SecondAnacondaLetters);
            }
            else
            {
                anacondaList[i].GetLetters(UnitAnacondaLetters.ThirdAnacondaLetters);
            }
        }
    }

    void SetAnacondaSounds()// anaconda sound 0, 1 and 2
    {
        for (int i = 0; i < anacondaList.Count; i++)
        {
            if (i == 0)
                anacondaList[i].GetSounds(UnitAnacondaSounds.firstAnacondaSounds);
            else if(i == 1)
                anacondaList[i].GetSounds(UnitAnacondaSounds.secondAnacondaSounds);
            else
                anacondaList[i].GetSounds(UnitAnacondaSounds.thirdAnacondaSounds);
        }
    }

    IEnumerator SliderHandleBan(float duration)// delay  allowClick
    {

        foreach (var anaconda in anacondaList)
            anaconda.AllowClickOnLetters(false);

        yield return new WaitForSeconds(duration);

        foreach (var anaconda in anacondaList)
            anaconda.AllowClickOnLetters(true);
    }

    public void PlayBlendedLetterSound(AudioClip blendedLetterSound, Slider clickedSlider) //play BlendedLetter sound
    {
        audioManager.PlaySound(blendedLetterSound);
        StartCoroutine(SliderHandleBan(blendedLetterSound.length));
    }

    public void PlaySecondLetterSound(AudioClip secondLetterSound) // play second sound
    {
        audioManager.PlaySound(secondLetterSound);
        StartCoroutine(SliderHandleBan(secondLetterSound.length));
    }

    public void ResetGame()// ResetGame
    {
        onReset.Invoke();
        ProgressBarLogic.Instance.ResetNodesBackground();
        trialNumber = 0;
    }

    public char GetRightLetterForSlider() //System.Random var randomChar
    {
        System.Random rnd = new System.Random(); 
        var randomChar = pomocniNizSlovaZaTest[rnd.Next(pomocniNizSlovaZaTest.Length - 1)];
        Debug.Log(randomChar);
        return randomChar;
    }

    public void RepeatTutorial() // audio RepeatTutorial
    {       
        audioManager.RepeatTutorial();
    }

    public void DisableClicking()// DisableClicking foreach  Slider
    {
        clickAllowed = false;
        foreach (var slider in anacondaSliders)
            slider.interactable = false;
        lala.interactable = false;
    }

    public void EnableClicking() // EnableClicking foreach  Slider
    {
        clickAllowed = true;
        foreach (var slider in anacondaSliders)
            slider.interactable = true;
        lala.interactable = true;
    }
}
