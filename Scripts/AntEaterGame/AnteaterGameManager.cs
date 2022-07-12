using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AnteaterGameManager : MonoBehaviour, IGameManager
{
    // Start is called before the first frame update

    public delegate void NewWord(char word);
    public NewWord onNewLetterAdded;

    private static AnteaterGameManager _instance = null;

    public static AnteaterGameManager Instance { get { return _instance; } }

    public int NumberOfTrials { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    

    GameManager gameManager;
    UnitManager unitManager;
    UnitProgressManager unitProgressManager;
    AnteaterSoundManager audiomanager;

    AnteaterGameResult GameResult;

    public GameScoring ScoringSystem;
    [Header("Progress bar")]
    public ProgressBarLogic progressBar;
    [Header("Lala i drugari reference")]
    public List<MravLogic> mravList;
    public AnteaterLogic anteater;
    public LalaLogic lala;
    [Header("Prozor za rezultat mini igre")]
    public MiniGameResultWindow ResultWindow;
    public GameObject HandPointer;
    const int antNumber = 3;
    const int maxTrialNumber = 12;
    int brojPokusaja = 0;
    int score = 0;
    int trialCount = 0;
    char currentLetter;
    List<char> allLetters;
    List<LetterSoundPair> letterSoundPairs;
    public List<GameObject> Anthills;

    public float CORRECTANTDELAY;

    public float LETTERSHOWUPDELAY;


    public GameObject VisualElementsParent;

    public float animationDelayCorrect = 0.3f;
    public float animationDelayWrong = 0.7f;
    private bool progressBarSet = false;

    int randomIndex;

    List<int> passedRandomIndexes;

    int[] orderOfItems;


    Task HandpointerTask;
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }


    void LoadAndShowLastGameProgress()
    {
        if (ES3.KeyExists(EasySaveKeys.AnteaterMiniGame))
        {
            GameResultAnteater result = ES3.Load<GameResultAnteater>(EasySaveKeys.AnteaterMiniGame);
            Debug.Log(result);
        }
    }

    public void ReloadAnteaterGame()
    {
        SceneManager.LoadScene("AntEaterGame");
    }

    void Start()
    {
        passedRandomIndexes = new List<int>();
        orderOfItems = OrderOfGameItemsHelp.GetRandomOrderArray();
        LoadAndShowLastGameProgress();
        unitProgressManager = UnitProgressManager.Instance;
        gameManager = GameManager.Instance;
        unitManager = UnitManager.Instance;
        audiomanager = AnteaterSoundManager.Instance;
        allLetters = unitManager.GetAllUnitLetters();
        List<char> tempList = new List<char>();
        for(int i = 0; i < 4; i++)
        {
            foreach(var letter in allLetters)
            {
                tempList.Add(letter);
            }
        }
        allLetters = tempList;

    }

    public void SetAntSprites()
    {
        //ovde 
        for (int i = 0; i < mravList.Count; i++)
        {
            mravList[i].GetComponent<SpriteRenderer>().sprite = gameManager.currentUnit.unitLetterSprites[i];
        }
    }

    public void SetLala()
    {
        
        anteater.AllowClicking(true);
        if(HandpointerTask == null)
            HandpointerTask = new Task(FiveSecondWaitForPointer());
        currentLetter = allLetters[orderOfItems[trialCount]];
        GameResult = new AnteaterGameResult(gameManager.currentUnit.unitId);
        onNewLetterAdded.Invoke(currentLetter);
    }

    public bool CheckIfAllAntsAreVisible()
    {
        foreach(var ant in mravList)
        {
            if (ant.antVisual.IsAntUnderground())
                return false;
        }
        return true;
    }       

    public void SetMravi()
    {

        bool correctItemSet = false;
        int correctItemIndex = orderOfItems[trialCount] - 1;
        char correctLetter = gameManager.currentUnit.unitLetters[correctItemIndex];
        var tempList = gameManager.currentUnit.unitLetters.Except(new List<char>() { correctLetter}).ToList();
        tempList = Randomize<char>(tempList);
        int j = 0;
        for (int i = 0; i < antNumber; i++)
        {         
            if (mravList[i].antVisual.IsAntUnderground())
                mravList[i].antVisual.JumpOut();
            if(i == correctItemIndex)
            {
                mravList[i].SetCurrentLetter(correctLetter, FindWithSameName(correctLetter));
            }
            else
            {
                mravList[i].SetCurrentLetter(tempList[j], FindWithSameName(tempList[j]));
                j++;
            }
            
        }
    }

    Sprite FindWithSameName(char letter)
    {
        //ovde ide logika za velika slova 
        if(AvatarBase.Instance.typeOfChosenFont == TypeOfChosenLetterFont.LOWERCASE)
        {
            string path = @"LetterImages/small letters white";

            var temp = Resources.LoadAll<Sprite>(path);
            foreach (var elem in temp)
            {
                if (elem.name.Equals(letter.ToString()))
                {
                    return elem;
                }
            }
        }
        else
        {
            string path = @"LetterImages/BelaMalaSlovaAnteater";

            var temp = gameManager.currentUnit.capibaraLetterCapitalSprites;
            foreach (var elem in temp)
            {
                if (elem.name.ToLower().Equals(letter.ToString()))
                {
                    return elem;
                }
            }
        }
       
        return null;
    }

    public static List<T> Randomize<T>(List<T> list1)
    {
        List<T> randomizedList = new List<T>();
        var list = new List<T>(list1);
        System.Random rnd = new System.Random();
        while (list.Count > 0)
        {
            int index = rnd.Next(0, list.Count); //pick a random item from the master list
            randomizedList.Add(list[index]); //place it at the end of the randomized list
            list.RemoveAt(index);
        }
        return randomizedList;
    }

    public void SetAnteaterLetter()
    {
        anteater.DisplayCurrentLetter(currentLetter);
        foreach (var mrav in mravList)
        {
            mrav.interactable = true;
        }
            
    }

    public void ResetAntsAfterQuestion(AntVisualBehaviour ant, float delay = 0f)
    {
        foreach(var lilant in mravList)
        {
            if (lilant != ant)
            {
                if (!lilant.antVisual.IsAntUnderground())
                    lilant.antVisual.CorrectAnswer(true);
                else
                    StartCoroutine(AntJumpOutDelay(lilant.antVisual,CORRECTANTDELAY));
            }
                
        }
    }

    IEnumerator AntJumpOutDelay(AntVisualBehaviour lilant,float delay)
    {
        yield return new WaitForSeconds(delay);

        lilant.JumpOut();
    }

    public void DisableHandPointer(bool disable) // ovde uvek dodje false
    {
        if (HandpointerTask != null)
            HandpointerTask.Stop();
        HandPointer.SetActive(disable);
    }

    IEnumerator StartNewLetter()
    {

        
        progressBarSet = false;
        anteater.AllowClicking(false);
        //lala.SetIntroText();
        if (brojPokusaja == 1)
            score++;
        if (trialCount < maxTrialNumber - 1)
        {
            Debug.Log($"trenutni skor:{score}, broj pitanja:{trialCount}");
            brojPokusaja = 0;
            trialCount++;
            yield return new WaitForSeconds(1f);
            SetMravi();
            anteater.AllowClicking(true);
            HandpointerTask = new Task(FiveSecondWaitForPointer());
            anteater.ResetText();
            currentLetter = allLetters[orderOfItems[trialCount] - 1];
          
            onNewLetterAdded.Invoke(currentLetter);

        }
        else
        {
            GameResult.CalculateScoreForEachLetter(score);
            Debug.Log(GameResult);
            unitProgressManager.AddProgressForUnlockableGame(BasicGames.ANTEATER);
            yield return new WaitForSeconds(2f);
            ResultWindow.gameObject.SetActive(true);
            ResultWindow.SetZvezdicaTextNew(score, 12 - score,ScoringSystem);
            yield return null;
        }
        progressBar.lala.GetComponent<Collider>().enabled = true;
    }

    int GetNextRandomIndex()
    {
        int generatedRandomIndex;

        do
        {
            generatedRandomIndex = UnityEngine.Random.Range(0, allLetters.Count);
            if (!passedRandomIndexes.Contains(generatedRandomIndex))
                break;
        } while (passedRandomIndexes.Contains(generatedRandomIndex));

        passedRandomIndexes.Add(generatedRandomIndex);

        return generatedRandomIndex;
    }


    public AudioClip FindSoundOfLetter()
    {
        foreach(var item in unitManager.letterSoundPairsAnteater.letterSoundPairs)
        {
            if (item.letter.Equals(currentLetter))
                return item.letterSound;
        }

        return null;
    }

    public void TurnOffHandPointer()
    {
        DisableHandPointer(false);
    }

    public void CheckIfCorrectLetter(char letter, MravLogic mrav)
    {
        StartCoroutine(CheckIfLetterIsGood(letter, mrav));
    }

    public void SkipAnteater() //
    {
        GameResult = new AnteaterGameResult(gameManager.currentUnit.unitId);
        GameResult.GenerateResultForSkipping();
        unitProgressManager.AddProgressForUnlockableGame(BasicGames.ANTEATER);
        SceneManager.LoadScene("MapScene");
    }

    IEnumerator FiveSecondWaitForPointer() // wait 5 seconds for handpointer to setActiveTrue
    {
        yield return new WaitForSeconds(10f);
        if (!anteater.isAnteaterClicked)
        {
            HandPointer.SetActive(true);
        }

    }

    public IEnumerator CheckIfLetterIsGood(char letter, MravLogic mrav)
    {
        progressBar.lala.GetComponent<Collider>().enabled = false;
        DisableMravi(false);
        brojPokusaja++;
        DisableHandPointer(false);
        bool correct = currentLetter.Equals(letter);
        progressBar.LalaAnswerReaction(correct);
        if (!progressBarSet)
        {
            progressBar.ChangeNodeBackground(trialCount,correct);
            progressBarSet = true;
        }
        
        audiomanager.PlayQuestionResultSound(correct);
        yield return new WaitForSeconds(0.5f);
        //lala.SetQuestionResultText(currentLetter.Equals(letter));
        if (!correct)
        {
            mrav.interactable = false;

            
            anteater.anteaterVisual.SpitLeaf(mrav.antVisual);
            mrav.FadeLetter();
            yield return new WaitForSeconds(animationDelayWrong);
            mrav.antVisual.WrongAnswer();
            
           
            
            GameResult.AddWrongLetter(currentLetter);
            yield return new WaitForSeconds(2f);
            audiomanager.PlaySound(FindSoundOfLetter());
            yield return new WaitForSeconds(0.2f);
            progressBar.lala.GetComponent<Collider>().enabled = true;
            DisableMravi(true);
        }
        else
        {
            anteater.AllowClicking(false);
            mrav.interactable = false;
            foreach (var ant in mravList)
            {
                ant.FadeLetter();
            }
            anteater.anteaterVisual.EatLeaf(mrav.antVisual);
            yield return new WaitForSeconds(animationDelayCorrect);
            mrav.antVisual.CorrectAnswer();
            DisableMravi(false);
            GameResult.AddProgressForALetter(letter, brojPokusaja == 1 ? 1 : 0);
            yield return new WaitForSeconds(1f);
            StartCoroutine(StartNewLetter());
        }

    }

    public void ActivateAntGameObjects() //
    {
        int i = 0;
        foreach (var ant in mravList)
        {
            Anthills[i].SetActive(false);
            letterShowUpDelay(ant);
            ant.antVisual.gameObject.SetActive(true);
            i++;
        }
    }

    IEnumerator letterShowUpDelay(MravLogic mrav) // delay ants for 1 sec
    {
        yield return new WaitForSeconds(1f);
        mrav.gameObject.SetActive(true);
    }

    public void DisableMravi(bool disable) //disable ants
    {
        foreach(var mrav in mravList)
            mrav.interactable = disable;
    }

    // Update is called once per frame
 
    public void GoBackToMap() // load scene MapScene
    {
        SceneManager.LoadScene("MapScene");
    }

    public void RepeatTutorial()
    {

        

        DisableHandPointer(false);

        HandpointerTask = new Task(FiveSecondWaitForPointer());
        
        DisableMravi(false);
      //  anteater.AllowClicking(false);
        audiomanager.RepeatTutorial();
    }

    public void EnableClicking()
    {
        DisableMravi(true);
        anteater.AllowClicking(true);
    }
}
