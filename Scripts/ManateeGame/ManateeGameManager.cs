
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManateeGameManager : MonoBehaviour, IGameManager
{
    // Start is called before the first frame update


    public float LETTERSHOWUPDELAY = 1.1f;

    public delegate void OnImageGuessed(bool equal);
    public OnImageGuessed onImageGuessed;

    BasicGames gameCode = BasicGames.MANATEE;
    //bool tutorialTrial = true;
    bool repeatingActive = false;
    int trialNumber = 0;
    int brojPokusaja = 0;
    int score = 0;
    int lastRandomNumber;
    char lastRandomLetter;
    int numberOfTrials = 6;
    int numberOfTrialsWithTutorial = 7;
    int miniTrialNumber = 0;

    bool tutorialTrialEnded = false;

    public int NumberOfTrials { get => numberOfTrials; set => numberOfTrials = value; }

    UnitManager unitManager;
    UnitProgressManager unitProgressManager;
    ManateeAudioManager audiomanager;

    public GameScoring ScoringSystem;

    const int numberOfDistractors = 2;
    
    const int repeatingLimit = 0;

    public char GameLetter;
    ManateeGameResult gameResult;

    public List<ManateeLogic> manateeList;
    public LalaLogic lala;
    public ProgressBarLogic progressBar;
    public SpriteRenderer letterOnALog;

    Coroutine korutina;

    List<ManateeImageObject> items;
    List<ManateeImageObject> distractors;
    List<ManateeImageObject> wrongTrialImages;
    public MiniGameResultWindow ResultWindow;
    public TextMeshProUGUI currentLetterText;
    List<char> BadPositionedLetters = new List<char> { 'f', 'b', 'i', 'l', 'p', 'g', 'j', 'd' };
    public GameObject VisualElementsParent;

    public BlueMacaoVisualBehaviour narator;

    public GameObject HandPointer;

    public static ManateeGameManager _instance = null;
    
    public static ManateeGameManager Instance { get { return _instance; } }

    public WoodenSignManatee sign;

    bool childHasClickedOnAManatee = false;

    bool progressBarSet = false;

    Task korutinaTask;

    bool FinishedGameIndicator = false;

    int[] correctItemPositionOrder;

    private void Awake()
    {

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void SubscribingListeners()//SubscribingListeners
    {
        onImageGuessed += lala.SetQuestionResultText;
        audiomanager.wordSoundsOverEvent += AllowClicking;
    }

    public void TurnOffHandPointer()//TurnOffHandPointer
    {
        korutinaTask.Stop();
        HandPointer.SetActive(false);
    }

    void LoadLastGameScore()// load game score
    {
        if (ES3.KeyExists(EasySaveKeys.ManateeMiniGame))
        {
            var result = ES3.Load<GameResultManatee>(EasySaveKeys.ManateeMiniGame);
            Debug.Log(result);
        }
        else
        {
            Debug.Log("Jos niste odigrali nijednu partiju ove mini igre tako da nemate nista sacuvano ");
        }
    }

    public void ReloadManateeScene()//ReloadManateeScene
    {
        SceneManager.LoadScene("ManateeGame");
    }

    void SetStickerIndicator()//SetStickerIndicator
    {
        //FinishedGameIndicator = UnitStatisticsBase.Instance.GetCurrentUnitStatistic().manateeGameResultDictionary[unitManager.GetCurrentLetter()].score != 0 ||
        //  UnitStatisticsBase.Instance.GetCurrentUnitStatistic().manateeGameResultDictionary[unitManager.GetCurrentLetter()].WrongPictures != null;
        FinishedGameIndicator = UnitProgressManager.Instance.CheckIfLetterIsFinished(GameLetter, UnitManager.Instance.AllUnitsList.IndexOf(GameManager.Instance.currentUnit));
        GameManager.Instance.StickerIndicator = FinishedGameIndicator;
        
    }

    private void Start()// start
    {
        audiomanager = ManateeAudioManager.Instance;
        unitManager = UnitManager.Instance;
        unitProgressManager = UnitProgressManager.Instance;
        correctItemPositionOrder = OrderOfGameItemsHelp.GetRandomOrderArray();
        wrongTrialImages = new List<ManateeImageObject>();
        gameResult = new ManateeGameResult();
        LoadLastGameScore();
        SubscribingListeners();

        if (unitManager != null)// gamelogic
            GameLetter = unitManager.GetCurrentLetter();
        else
            GameLetter = 'a';
        //ovde promeniti i dodati za malo veliko slovo logiku 
        if(AvatarBase.Instance != null)//gamelogic gameletter
        {
            if (AvatarBase.Instance.typeOfChosenFont == TypeOfChosenLetterFont.LOWERCASE)
            {
                if (!BadPositionedLetters.Contains(GameLetter)) 
                    letterOnALog.sprite = Resources.Load<Sprite>($"LetterImages/small letters/{GameLetter}");
                else
                    letterOnALog.sprite = Resources.Load<Sprite>($"LetterImages/Centered lower/{GameLetter.ToString().ToLower()}"); 
            }    
            else
                letterOnALog.sprite = Resources.Load<Sprite>($"LetterImages/capital letters brown/{GameLetter.ToString().ToUpper()}");
        }else
            letterOnALog.sprite = Resources.Load<Sprite>($"LetterImages/small letters/{GameLetter}");



        LoadManateeImages();
        AllowClicking(false);
        SetImages(false, true);
        SetStickerIndicator();
    }

    public void ManateeInit() // ManateeInit
    {

        narator.ResetToIdle();
        korutinaTask = new Task(FiveSecondWaitForPointer());
        AllowClicking(true);
        //AllowClicking(false);

    }

    public void EnableHandPointer(bool enable) //EnableHandPointer
    {
        HandPointer.SetActive(enable);
    }

    public void SkipManatee()// skip button manatee
    {
        gameResult = new ManateeGameResult(); 
        unitProgressManager.SaveProgressForALetterInUnit(GameLetter, gameCode);
        GameManager.Instance.FinishedLetter = GameLetter;
        GoBackToMap();
    }

    IEnumerator ResetImages()// reset images if you lose
    {

        childHasClickedOnAManatee = false;
        progressBarSet = false;
        if (!repeatingActive && brojPokusaja == 1 && tutorialTrialEnded)
            score++;
        if (trialNumber == numberOfTrials - 1 || (repeatingActive && trialNumber == numberOfTrials - score - 1))
        {
            if(repeatingActive && trialNumber == numberOfTrials - score - 1)
            {
                yield return new WaitForSeconds(2f);
                GameManager.Instance.FinishedLetter = GameLetter;
                ResultWindow.gameObject.SetActive(true);
                ResultWindow.SetZvezdicaTextNew(score, numberOfTrials - score, ScoringSystem, true);
                unitProgressManager.SaveProgressForALetterInUnit(GameLetter, gameCode);
                yield return null;
            }
            
            gameResult.SaveGameReport(score);
            if(score >= repeatingLimit)
            {
                yield return new WaitForSeconds(2f);
                ResultWindow.gameObject.SetActive(true);
                ResultWindow.SetZvezdicaTextNew(score, numberOfTrials - score, ScoringSystem);
                unitProgressManager.SaveProgressForALetterInUnit(GameLetter, gameCode);
                GameManager.Instance.FinishedLetter = GameLetter;
                Debug.Log($"Slike na kojima ste pogresili :");
                foreach (var item in wrongTrialImages)
                    Debug.Log($"{item.Image.name}");
            }
            else
            {
                AditionalTrials();
                Debug.Log("opet radite slike na kojima ste pogresili :");
            }
            
        }
        else
        {
            yield return new WaitForSeconds(1f);
            if (trialNumber == 0 && !tutorialTrialEnded)
                tutorialTrialEnded = true;
            else
                trialNumber++;
            brojPokusaja = 0;
            miniTrialNumber++;

            ReactivateManatees();
            
            SetImages(repeatingActive);
        }
        yield return new WaitForSeconds(1f);
        AllowClicking(true);
        progressBar.lala.GetComponent<Collider>().enabled = true;

    }

    void AditionalTrials()// start SetupAditionalTrials
    {

        StartCoroutine(SetupAditionalTrials());
    }
    IEnumerator SetupAditionalTrials() {
        ReactivateManatees();
        repeatingActive = true;
        trialNumber = 0;
        brojPokusaja = 0;
        //lala.SetText(lala.lalaInfo.ManateeGameRepeatText);
       // progressBar.ResetNodesBackground();
        yield return new WaitForSeconds(0.5f);
        SetImages(true);
        
    }

    IEnumerator FiveSecondWaitForPointer()// HandPointer after 5 sec SetActive true
    {
        yield return new WaitForSeconds(10f);
        if (!childHasClickedOnAManatee && !narator.clicked)
        {
            HandPointer.SetActive(true);
            korutinaTask.Stop();
        }

        korutinaTask.Stop();
    }

    public void CheckIfCorrectImage(ManateeLogic obj)//CheckIfCorrectImage
    {
        StartCoroutine(CheckIfCorrectImageCoroutine(obj));
    }

    AudioClip FindCorrectWordSound()// if manatee Equals GameLetter
    {
        foreach (var manatee in manateeList)
        {
            if (manatee.imageObject.firstLetter.Equals(GameLetter))
                return manatee.imageObject.wordSound;
        }
        return null;
    }

    public AudioClip FindSoundOfLetter()// foreach item in letterSoundPairs
    {
        foreach (var item in unitManager.letterSoundPairsManatee.letterSoundPairs)
        {
            if (item.letter.Equals(GameLetter))
                return item.letterSound;
        }

        return null;
    }


    public void GoBackToMapWithoutFinish()// load mapscene
    {
        GameManager.Instance.GoStraightToMapIndicator = true;
        SceneManager.LoadScene("MapScene");
    }

    public void GoBackToMap()// load mapscene
    {
        GameManager.Instance.GoStraightToMapIndicator = FinishedGameIndicator;
        SceneManager.LoadScene("MapScene");
    }

    IEnumerator CheckIfCorrectImageCoroutine(ManateeLogic obj) // Check If Correct Image
    {
        audiomanager.StopWordSoundTask();
        AllowClicking(false);
        progressBar.lala.GetComponent<Collider>().enabled = false;
        korutinaTask.Stop();
        childHasClickedOnAManatee = true;
        HandPointer.SetActive(false);
        bool equal = GameLetter.ToString().Equals(obj.imageObject.firstLetter.ToString(),
                                 StringComparison.InvariantCultureIgnoreCase);
        onImageGuessed(equal);
        if (!progressBarSet)
        {
            if (!repeatingActive)
            {
                if(tutorialTrialEnded)
                {
                    progressBar.ChangeNodeBackground(trialNumber, equal);
                    progressBarSet = true;
                }
                   
            }
            
        }

        brojPokusaja++;
        audiomanager.PlayQuestionResultSound(equal);

        
        progressBar.LalaAnswerReaction(equal);
        yield return new WaitForSeconds(0.2f);
        if (equal)
        {
            foreach (var manatee in manateeList)
            {
                manatee.manateeVisual.SwimIn(equal);
                manatee.GetComponent<LetterBehaviour>().Fade();
            }
                
            narator.ShowThumbsUp();
            AllowClicking(false);
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(ResetImages());
        }
        else
        {
            if (!repeatingActive)
            {
                
                var wrongImage = FindWrongTrialImage(GameLetter);
                if (!wrongTrialImages.Contains(wrongImage))
                {
                    wrongTrialImages.Add(wrongImage);

                    gameResult.AddWrongWord(obj.imageObject.Image.name);
                }

            }
            AllowClicking(false);
            //obj.gameObject.SetActive(false);
            obj.GetComponent<LetterBehaviour>().Fade();
            obj.manateeVisual.SwimIn(equal);
            narator.Wrong();
            
            yield return new WaitForSeconds(0.8f);
            AllowClicking(true);
            progressBar.lala.GetComponent<Collider>().enabled = true;
        }

        
    }
    
    ManateeImageObject FindWrongTrialImage(char letter) //foreach item in manateelist  
    {
        foreach(var item in manateeList)
        {
            if (char.ToLower(item.imageObject.firstLetter).Equals(char.ToLower(letter)))
                return item.imageObject;
        }

        return null;
    }
    public void AllowClicking(bool allow)// foreach manatee in manateelist allow
    {

        foreach (var manatee in manateeList)
            manatee.interactable = allow;
    }

    List<AudioClip> GetAllWordSounds()// foreach manatee in manateelist
    {
        var localList = new List<AudioClip>();
        foreach (var manatee in manateeList)
        {
           
                localList.Add(manatee.imageObject.wordSound);
        }
            
        return localList;
    }

    public void RepeatWordSounds() // RepeatWordSounds start WordSoundsIntroductionDelay
    {
        progressBar.lala.GetComponent<Collider>().enabled = false;
        sign.SetInteractable(false);
        HandPointer.SetActive(false);
        StartCoroutine(WordSoundsIntroductionDelay());
    }

    
    IEnumerator WordSoundsIntroductionDelay() //WordSoundsIntroductionDelay
    {
       // AllowClicking(false);
        audiomanager.PlayWordSounds(GetAllWordSounds());
        yield return new WaitForSeconds(1f);
       // StartCoroutine(FiveSecondWaitForPointer());
        
    }

    void ReactivateManatees() //ReactivateManatees
    {
        StartCoroutine(ReactivateManatee());
    }

    IEnumerator ReactivateManatee() // delay  AllowClicking  and manatee SetActive(true)
    {
        yield return new WaitForSeconds(0.5f);
        //ovde ce da kaze zvukove od svih reci
        AllowClicking(true);
        foreach (var manatee in manateeList)
        {
            manatee.gameObject.SetActive(true);
            manatee.manateeVisual.SwimOut();
            manatee.GetComponent<LetterBehaviour>().ResetToIdle();
        }
           
    }


    void SetImages(bool trial, bool firstTime = false) // gamelogic for this scene var random
    {
        var rnd = new System.Random();
        bool itemSet = false;
        var tempList = distractors.OrderBy(x => rnd.Next()).Take(numberOfDistractors);
        foreach (var manatee in manateeList)
        {
          
            manatee.GetComponent<LetterBehaviour>().ResetToIdle();
        }

        int randomIndex = correctItemPositionOrder[miniTrialNumber] - 1;

        if (!trial)
        {
            for (int i = 0; i < manateeList.Count; i++)
            {
                if (!itemSet && i == randomIndex)
                {
                    manateeList[i].SetImage(items[miniTrialNumber]);
                    itemSet = true;
                }
                else if (i == numberOfDistractors  && !itemSet)
                {
                    manateeList[i].SetImage(items[miniTrialNumber]);
                    itemSet = true;
                }
                else
                {
                    int vrednost;
                    do
                    {
                        vrednost = rnd.Next(0, distractors.Count - 1);
                    } while (vrednost == lastRandomNumber || distractors[vrednost].firstLetter == lastRandomLetter);
                    
                    
                        manateeList[i].SetImage(distractors[vrednost]);
                    Debug.Log($"distrektor broj {i} : {distractors[vrednost].name}");
                        lastRandomNumber = vrednost;
                        lastRandomLetter = distractors[lastRandomNumber].firstLetter;



                }
            }
        }
        //ispravljaj ovo kad se budes vratio 
        else
        {
            for (int i = 0; i < manateeList.Count; i++)
            {
                if (!itemSet && rnd.NextDouble() > 0.4)
                {
                    manateeList[i].SetImage(wrongTrialImages[miniTrialNumber % 3]);
                    itemSet = true;
                }
                else if (i == numberOfDistractors - 1 && !itemSet)
                {
                    manateeList[i].SetImage(wrongTrialImages[miniTrialNumber % 3]);
                    itemSet = true;
                }
                else
                {
                    int vrednost;
                    do
                    {
                        vrednost = rnd.Next(0, distractors.Count - 1);
                    } while (vrednost == lastRandomNumber || distractors[vrednost].firstLetter == lastRandomLetter);


                    manateeList[i].SetImage(distractors[vrednost]);
                    lastRandomNumber = vrednost;
                    lastRandomLetter = distractors[lastRandomNumber].firstLetter;

                }
            }
        }



        //ovde ce se cuti zvukovi od svake reci posebno 
        narator.clicked = false;
        if(!firstTime)
            korutinaTask = new Task(FiveSecondWaitForPointer());//1998
    }

    public void GoToMap() // loadScene mapscene
    {
        SceneManager.LoadScene("MapScene");
    }

    void LoadManateeImages() //
    {
        items = Resources.LoadAll<ManateeImageObject>(Paths.ImageLetterObjectPairsPath + GameLetter).ToList();
        char randomLetter;
        var rand = new System.Random();
        do
        {
            if (unitManager != null)
                randomLetter = unitManager.GetAllUnitLetters()[rand.Next(unitManager.GetAllUnitLetters().Count - 1)];
            else
                randomLetter = 'f';
        } while (randomLetter == GameLetter);
        //distractors = Resources.LoadAll<ManateeImageObject>(Paths.ImageLetterObjectPairsPath + randomLetter).ToList();
        distractors = Resources.Load<ManateeDistractorsContainer>($"ScriptableObjects/ManateeDistractors/{GameLetter.ToString().ToLower()}").DistractorsList;
        foreach (var item in items)
            Debug.Log($"{item.name}");
        foreach (var item in distractors)
            Debug.Log($"{item.name}");
    }

    public void RepeatTutorial() // RepeatTutorial
    {
        TurnOffHandPointer();

        korutinaTask = new Task(FiveSecondWaitForPointer());

        AllowClicking(false);
       // narator.interactable = false;
        sign.SetInteractable(false);
        audiomanager.RepeatTutorial();
    }

    public void EnableClicking() //EnableClicking
    {
        AllowClicking(true);
        narator.interactable = true;
        sign.SetInteractable(true);
    }
}
