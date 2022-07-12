using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LalaLetterMachineManager : MonoBehaviour, IGameManager
{
    // Start is called before the first frame update

    public delegate void WordDisplayDone();
    public WordDisplayDone onWordDisplayDone;
    static LalaLetterMachineManager _instance = null;
    public static LalaLetterMachineManager Instance { get { return _instance; } }

    public int NumberOfTrials { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    float nextLetterDelay = 0.1f;

    IEnumerator refference;

    public GameObject GoToNextGameButton;

    [Header("Masks")]
    public List<RectTransform> maskRectTransforms;

    public GameObject FallingLetter;

    List<Vector3> upperMiddlePosOfMask;

    List<Vector3> downMiddlePosOfMask;

    List<GameObject> wordLetterObjects;

    List<Sprite> fallingLetterSprites;

    public LetterMachineInfoScriptableObject gameInfo;
    [Header("prekidac za pokretanje")]
    public bool StartTargetWord = false;

    public bool IsSpinning = false;

    public SpriteRenderer targetWordIlustration;

    LetterMachineAudioManager audioManager;

    GameManager gameManager;

    Task fallingLettersTask;

    public ProgressBarLalaVisualBehaviour lalaNarator;

    public ArmadilloVisualBehaviour armadillo;

    public GameObject HandPointer;

    public AvatarBase avatarBase;

    Task HandPointerTask;

    int wordCounter = 0;

    const int numberOfWords = 4;

    bool buttonApeared = false;

    List<string> allLettersFromUnitsBefore = new List<string>();

    List<string> specificWords = new List<string>()
    {
        "sofa"
    };

   

    List<string> TargetWords = new List<string>()
        {
        };

   
    private void Awake()// Awake
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
        avatarBase = AvatarBase.Instance;
    }

    void DetermineWords()//DetermineWords  TargetWords 
    {
        int holeId = GameManager.Instance.CurrentHoleIndex;
        TargetWords = gameInfo.WordsForUnit.FirstOrDefault(x => x.holeID == holeId).wordsList;
/*        foreach(var letter in allLettersFromUnitsBefore)
        {
            TargetWords.Add(gameInfo.targetwo)*/
    }
    


    public void GoBackToMap()// load mapScene
    {
        SceneManager.LoadScene("MapScene");
    }
    private void Start()// logic for letter
    {
        GameManager.Instance.ReturningFromHoleIndicator = true;

        if (avatarBase != null) {
            if (avatarBase.typeOfChosenFont == TypeOfChosenLetterFont.LOWERCASE)
            {
                fallingLetterSprites = gameInfo.fallingLetterSpritesLowercase;
            }
            else
            {
                fallingLetterSprites = gameInfo.fallingLetterSpritesUppercase;
            }
        }else
            fallingLetterSprites = gameInfo.fallingLetterSpritesLowercase;

        DetermineWords();
        upperMiddlePosOfMask = new List<Vector3>();
        downMiddlePosOfMask = new List<Vector3>();
        wordLetterObjects = new List<GameObject>();

        audioManager = LetterMachineAudioManager.Instance;
        gameManager = GameManager.Instance;


        refference = InstantiateRandomFallingLetter();
        CalculateInstantiatingPositions();
        SubscribeListeners();


    }
    void CalculateInstantiatingPositions() //foreach mask in maskRectTransforms position 
    {
        int i = 0;

        foreach (var mask in maskRectTransforms)
        {
            Vector3[] v = new Vector3[4];
            mask.GetWorldCorners(v);
            upperMiddlePosOfMask.Add(new Vector3((v[1].x + v[2].x) / 2, v[1].y));
            downMiddlePosOfMask.Add(new Vector3((v[0].x + v[3].x) / 2, v[0].y));
            i++;
        }
    }

    

    IEnumerator InstantiateRandomFallingLetter()// foreach instantiatePosition in upperMiddlePosOfMask
    {
        IsSpinning = true;
        lalaNarator.interactable = false;
        Debug.Log("usao sam u instanciranje random padajucih slova");
        int j = 0, numberOfFallingLetters = 5;
        System.Random rnd = new System.Random();
        float rndNum = 0f;
        armadillo.ActivateShake();
        audioManager.PlaySound(audioManager.spinningSound);
        var currentTime = Time.time;
        while (true)
        {
            if (j < numberOfFallingLetters)
            {
                Debug.Log("padajuce slovo je instancirano");
                int i = 0;
                foreach (var instantiatePosition in upperMiddlePosOfMask)
                {
                    var obj = Instantiate(FallingLetter, maskRectTransforms[i]);
                    obj.GetComponent<FallingLetterBehaviour>().Init(maskRectTransforms[i].position + Vector3.down,
                        maskRectTransforms[i].position + Vector3.up,
                       fallingLetterSprites[rnd.Next(fallingLetterSprites.Count)]);
                    Debug.Log($"instancirao sam slovo na poziciju {instantiatePosition}");
                    var rndNum1 = (float)new decimal(rnd.NextDouble());
                    rndNum1 = Mathf.Clamp(rndNum1, 0, nextLetterDelay);
                    yield return new WaitForSeconds(rndNum1);
                    i++;
                }
                 rndNum = (float)new decimal(rnd.NextDouble());
                rndNum = Mathf.Clamp(rndNum, 0, nextLetterDelay);
               
            }
            else
            {
                StartTargetWord = true;
                fallingLettersTask.Stop();
                Debug.Log("proslo je :" + (Time.time - currentTime) + "vremena ");
                
            }
            yield return new WaitForSeconds(rndNum);
            j++;

        }
       
    }

    public void RepeatTutorial()
    {
        audioManager.RepeatTutorial();
    }

    public void EnableClicking()
    {
        armadillo.interactable = true;
        lalaNarator.interactable = true;
    }

    void SubscribeListeners()
    {
        //onWordDisplayDone += DestroyLetters;
        onWordDisplayDone += lalaNarator.ShowThumbsUp;
    }

   

    public void StartFallingLetters()//task InstantiateRandomFallingLetter
    {
        Debug.Log("pocinjem da instanciram padajuca slova");
        Debug.Log("da li je referenca null tj da li je korutina null " + refference);
        // StopCoroutine(refference);
        //  StartCoroutine(refference);
        targetWordIlustration.gameObject.SetActive(false);
        DestroyLetters();
        fallingLettersTask = new Task(InstantiateRandomFallingLetter());
    }

    public Sprite FindWithSameName(char letter)//FindWithSameName
    {
        return gameInfo.FindWithSameName(letter, avatarBase.typeOfChosenFont);
    }


    public void DisableHandPointer(bool disable)//DisableHandPointer
    {
        HandPointer.SetActive(disable);
    }


    public void StopHandPointerTask()//StopHandPointerTask
    {
        HandPointerTask.Stop();
    }

    public void StartHandPointerTask()// StartHandPointerTask
    {
        HandPointerTask = new Task(FiveSecondWaitForPointer());
    }

    IEnumerator FiveSecondWaitForPointer()// delay HandPointer SetActive true
    {
        yield return new WaitForSeconds(10f);
        if (!armadillo.clicked)
        {
            HandPointer.SetActive(true);
        }
        HandPointerTask.Stop();
    }

    public void SkipLetterMachine()// skip button load mapScene
    {
        bool thisIsEnd = GameManager.Instance.CurrentHoleIndex == 4 && UnitStatisticsBase.Instance.GetMostCurrentHoleId() == 3;
        UnitStatisticsBase.Instance.AddNewHole(GameManager.Instance.CurrentHoleIndex);
        if (thisIsEnd)
        {
            GameManager.Instance.EndOfLalaGameIndicator = true;
        }
        SceneManager.LoadScene("MapScene");
    }

    IEnumerator EndOfTheLetterMachineGame()// SetActive true GoToNextGameButton
    {
        bool thisIsEnd = GameManager.Instance.CurrentHoleIndex == 4 && UnitStatisticsBase.Instance.GetMostCurrentHoleId() == 3;
        yield return new WaitForSeconds(audioManager.PlayBinSound().length - 0.8f);
        GoToNextGameButton.SetActive(true);
        
        GoToNextGameButton.transform.DOScale(new Vector3(1.3f, 1.3f), 0.5f).OnComplete(() => { GoToNextGameButton.transform.DOScale(Vector3.one, 0.5f); });
        
        UnitStatisticsBase.Instance.AddNewHole(GameManager.Instance.CurrentHoleIndex);
        if (thisIsEnd)
        {
            GameManager.Instance.EndOfLalaGameIndicator = true;
        }
        buttonApeared = true;
        

    }

    IEnumerator InstantiateTargetWordLetters() // logic for target wordsLetters
    {
        int i = 0;
        System.Random rnd = new System.Random();
        yield return new WaitForSeconds(1f);

        string currentWord = TargetWords[wordCounter % TargetWords.Count];
        audioManager.PlaySound(gameInfo.FindPhonemeAudioClip(currentWord));
        foreach (var letter in currentWord)
        {
            //transform.TransformPoint(tr.rect.center)
            var obj = Instantiate(FallingLetter, maskRectTransforms[i]);
            wordLetterObjects.Add(obj);
            if(i == 3 && gameInfo.wordsWithAccents.Contains(currentWord))
            {
                if(AvatarBase.Instance.typeOfChosenFont == TypeOfChosenLetterFont.LOWERCASE)
                {
                    obj.GetComponent<FallingLetterBehaviour>().Init(maskRectTransforms[i].TransformPoint(maskRectTransforms[i].rect.center),
                maskRectTransforms[i].position + Vector3.up,
                gameInfo.FindAccentedLetter(letter, TypeOfChosenLetterFont.LOWERCASE), letter, true, currentWord);
                }
                else
                {
                    obj.GetComponent<FallingLetterBehaviour>().Init(maskRectTransforms[i].TransformPoint(maskRectTransforms[i].rect.center),
                maskRectTransforms[i].position + Vector3.up,
                gameInfo.FindAccentedLetter(letter, TypeOfChosenLetterFont.UPPERCASE), letter, true, currentWord);
                }
            }
            else
            {
                obj.GetComponent<FallingLetterBehaviour>().Init(maskRectTransforms[i].TransformPoint(maskRectTransforms[i].rect.center),
                maskRectTransforms[i].position + Vector3.up,
                FindWithSameName(letter), letter, true, currentWord);
            }
            
            yield return new WaitForSeconds(1.5f);
            i++;
        }
        wordCounter++;
        
            
       
        yield return new WaitForSeconds(0.5f);
        audioManager.PlayTargetWordSound(currentWord);
        
        if (UnitManager.Instance != null)
        {
            yield return new WaitForSeconds(1.1f);
            targetWordIlustration.gameObject.SetActive(true);
            targetWordIlustration.sprite = gameInfo.FindTargetWordIllustration(currentWord/*UnitManager.Instance.GetCurrentLetter()*/);
           

        }
        else
        {
            targetWordIlustration.sprite = gameInfo.FindTargetWordIllustration(currentWord);
        }
            

        if (wordCounter == TargetWords.Count && !buttonApeared)
        {
            StartCoroutine(EndOfTheLetterMachineGame());
        }else
        {
            audioManager.PlayBinSound();
        }
        onWordDisplayDone.Invoke();
        HandPointerTask = new Task(FiveSecondWaitForPointer());
        armadillo.clicked = false;
        IsSpinning = false;
        lalaNarator.interactable = true;
    }

    void DestroyLetters()//  foreach letter in wordLetterObjects destroy
    {
        foreach(var letter in wordLetterObjects)
        {
            Destroy(letter);
        }
        wordLetterObjects.Clear();
    }

    public void StartTargetWordCoroutine()//bool true
    {
        StartTargetWord = true;
    }

    //trebace jos da se odredi momenat kada ce target word ispadne i onda se stavljaju novi delay-evi da polako izadje kao taj target word
    // Update is called once per frame
    void Update()
    {
        if (StartTargetWord)// bool
        {
            armadillo.DeactivateShakeAnimation();
            StartCoroutine(InstantiateTargetWordLetters());
            StartTargetWord = false;
        }
    }

    public void TurnOffHandPointer()//DisableHandPointer
    {
        DisableHandPointer(false);
    }
}
