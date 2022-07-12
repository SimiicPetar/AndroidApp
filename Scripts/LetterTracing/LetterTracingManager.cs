
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LetterTracingManager : MonoBehaviour, IGameManager
{
    public delegate void FingerPromptDoneDrawing();
    public FingerPromptDoneDrawing onFingerDoneDrawing;

    static LetterTracingManager _instance = null;

    public static LetterTracingManager Instance { get { return _instance; } }

    public CheckpointLogic NextCheckPoint { get => nextCheckPoint; set => nextCheckPoint = value; }
    public CheckpointLogic LastCheckPoint { get => lastCheckPoint; set => lastCheckPoint = value; }
    public Transform CurrentParent { get => currentParent; set => currentParent = value; }
    public Transform LastParent { get => lastParent; set => lastParent = value; }
    public int NumberOfTrials { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    BasicGames gameCode = BasicGames.TRACING;

    GameManager gameManager;
    UnitManager unitManager;
    UnitProgressManager unitProgressManager;
    LetterTracingAudioManager audioManager;
    AvatarBase avatarBase;

    public List<LetterTracingCrayon> allCrayons;

    public List<CheckpointLogic> checkPoints;

    public bool CanDraw = false;

    public Paintable painting;

    CheckpointLogic nextCheckPoint;

     CheckpointLogic lastCheckPoint;
    
    int currentCheckpointIndex = 0;

    Transform currentParent;
    Transform lastParent;

    public bool holdingMouse = false;

    List<CheckpointParent> checkpointParents;
    List<CheckpointParent> passedParents;

    public Image TargetWordIllustration;

    public List<Paintable> lowercasePrefabs;

    public List<Paintable> uppercasePrefabs;

    public Button GoToManateeGameButton;

    public GameObject FingerPrompt;

    Task HideFingerPrompt;

    bool firstTimeDoingTracing = false;

    bool firstCheckPointNewParentIndicator = false;

    public ProgressBarLalaVisualBehaviour progressBar;

    public GameObject navigationButtonParent;

    bool done = false;

    public List<CheckpointLogic> passedCheckpoints;

    public float FixedDistance;

    public bool newParentIndicator = false;

    private void Awake()// Awake
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    void ActivatePrefab(char letter) // painting = letterTrace; letterTrace SetActive true
    {
        TypeOfChosenLetterFont chosen;
        //ovde ide logika za malo veliko slovo prefab
        if (avatarBase == null) 
        {
            chosen = TypeOfChosenLetterFont.UPPERCASE;
            letter = 'a';
            foreach (var letterTrace in uppercasePrefabs)
            {
                if (letterTrace.letter == letter)
                {
                    letterTrace.gameObject.SetActive(true);
                    painting = letterTrace;


                    return;
                }
            }
        }
            
        if(avatarBase.typeOfChosenFont == TypeOfChosenLetterFont.LOWERCASE)
        {
            foreach (var letterTrace in lowercasePrefabs)
            {
                if (letterTrace.letter == letter)
                {
                    letterTrace.gameObject.SetActive(true);
                    painting = letterTrace;

                    return;
                }
            }
        }
        else
        {
            foreach (var letterTrace in uppercasePrefabs)
            {
                if (letterTrace.letter == letter)
                {
                    letterTrace.gameObject.SetActive(true);
                    painting = letterTrace;
                    return;
                }
            }
        }
        
    }

    public void GoBackToLetterIntro()//LoadScene
    {
        SceneManager.LoadScene("LetterSoundScene");
    }

    public void GoBackToMap()//LoadScene
    {
        SceneManager.LoadScene("MapScene");
    }

    void SubscribeListeners()//SubscribeListeners
    {
       // audioManager.onIntroPart1Ended += ShowFingerPrompt;
    }

    public void ShowFingerPrompt()//HideFingerPrompt task
    {
        var anim = FingerPrompt.GetComponent<Animator>();
        HideFingerPrompt = new Task(HidePrompt(anim.runtimeAnimatorController.animationClips.Single(s => s.name == $"lowercase{unitManager.GetCurrentLetter().ToString().ToUpper()}Intro").length));        
    }

    IEnumerator HidePrompt(float delay)//delay AllowDrawing
    {
        yield return new WaitForSeconds(delay);
        AllowDrawing();
       // FingerPrompt.GetComponent<IntroHandPointerLogic>().AnimationIsOver();
       // FingerPrompt.SetActive(false);
      
    }

    private void Start()//start ActivatePrefab
    {
        gameManager = GameManager.Instance;
        audioManager = LetterTracingAudioManager.Instance;
        avatarBase = AvatarBase.Instance;
        gameManager = GameManager.Instance;
        unitManager = UnitManager.Instance;
       

       passedParents = new List<CheckpointParent>();
        
        
        unitProgressManager = UnitProgressManager.Instance;
        SubscribeListeners();
        passedCheckpoints = new List<CheckpointLogic>();
        if (unitManager != null)
        {
            ActivatePrefab(unitManager.GetCurrentLetter());
        }
        else
            ActivatePrefab('a');
        
    }

   

    public void ShowTargetWordIllustration(bool reset = false)//TargetWordIllustration true or false
    {     
        if (reset)
        {
            TargetWordIllustration.gameObject.SetActive(false);
            return;
        }
        if (unitManager != null)
        {
            TargetWordIllustration.gameObject.SetActive(true); // ovo ispod zakomentarisano cu ubaciti kad bude bio ce floww
            // TargetWordIllustration.sprite = gameManager.targetWords.TargetWordPairs.Single(s => s.FirstLetter == unitManager.GetCurrentLetter()).TargetWordImage;
            

        }
    }

    public void AllowDrawing()//AllowDrawing
    {
        CanDraw = true;
    }

    
    private void Update()// holdingMouse
    {
        if (CanDraw)
        {
            if (Input.GetMouseButton(0))
            {
                holdingMouse = true;
            }
            else
            {
                holdingMouse = false;
            }
        }
        
    }


    public bool CheckIfUserIsDrawing()//CheckIfUser holdingMouse
    {
        return holdingMouse;
    }



    IEnumerator PlayQuestionResultThenTargetWord() // play audio
    {
        var lala = audioManager.lala;
        lala.Talk();
        audioManager.PlaySound(audioManager.letterpartSounds[1]);
        yield return new WaitForSeconds(audioManager.letterpartSounds[1].length);
        lala.Idle();
        lala.ShowThumbsUp();
        audioManager.PlayQuestionResultSound(true);
        lala.Idle();
        ShowTargetWordIllustration();
    }

    public void ActivateNavigationParent(bool activate) // ActivateNavigationParent
    {
        //navigationButtonParent.SetActive(activate);
    }

    public void ResetCheckPoints() // foreach obj in painting SetActive false
    {
        painting.startedToDraw = false;
        done = false;

        foreach (var obj in painting.Covers)
        {
            obj.SetActive(false);
        }
        painting.DestroyTrail();


    }

    

    public void TracingFinished()// SaveProgress  LoadScene  MapScene
    {
        if(unitProgressManager != null && unitManager!= null)
            unitProgressManager.SaveProgressForALetterInUnit(unitManager.GetCurrentLetter(), gameCode);
        SceneManager.LoadScene("MapScene");
    }

    public void GoToManateeGame()//LoadScene ManateeGame
    {
        SceneManager.LoadScene("ManateeGame");
    }

    public void RepeatTutorial()// audio RepeatTutorial
    {
        CanDraw = false;
        audioManager.RepeatTutorial();
    }

    public void TurnOffHandPointer()//TurnOffHandPointer
    {
        throw new System.NotImplementedException();
    }
}
