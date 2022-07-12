using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KapibaraGameManager : MonoBehaviour, IGameManager
{
    // Start is called before the first frame update

    public delegate void NewWord(string word);
    public NewWord onNewWordAdded;
    public delegate void WordSoundPlayed();
    public WordSoundPlayed wordSoundPlayed;

    private static KapibaraGameManager _instance = null;

    public static KapibaraGameManager Instance { get { return _instance; } }

    public int NumberOfTrials { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    GameManager gameManager;
    UnitManager unitManager;
    UnitProgressManager unitProgressManager;
    AudioManagerCapibara audioManager;


    public float RESETDELAY = 4.2f;
    public float MINIDELAY;
    public GameScoring ScoringSystem;
    [Header("Visual Objects Parent")]
    public GameObject VisualObjectsParent;
    
    [Header("Crocodiles")]
    public List<CrocodileBehaviour> CrocodileVisuals;
    [Header("Progress bar")]
    public ProgressBarLogic progressBar;
    [Header("Prozor za rezultat mini igre")]
    public MiniGameResultWindow ResultWindow;
    [Header("Lala i drugari reference")]
    public List<KapibaraLogic> kapibaraList;
    public KornjacaLogic kornjaca;
    public LalaLogic lala;
    const int kapibaraNumber = 3;
    int wordCount = 0;
    int brojPokusaja = 0;
    int score = 0;
    string currentLetter;

    List<WordSoundPair> wordSoundPairs;
    List<string> words;

    public GameObject HandPointer;

    bool capibaraIsSet = false;

    bool newWordStarted = false;

    int numberOfPoints;
    KapibaraGameResult GameResult;

    bool progressBarSet = false;

    Task fingerPointerTask;

    public bool lastTrial = false;

    List<int> passedRandomLetters;

    int randomIndex;

    int[] lettersOrder;

    List<string> passedWords = new List<string>();

    char[] gameLetters;

    List<char> lastTempList;

    string currentWord;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
        wordSoundPairs = GameManager.Instance.currentUnit.wordSoundPairsCapibara;
        currentWord = wordSoundPairs[wordCount].wordText;
        lettersOrder = OrderOfGameItemsHelp.GetRandomOrderArray();
    }


    void LoadAndShowLastGameProgress() //  show progress
    {
        if(ES3.KeyExists(EasySaveKeys.KapibaraMiniGame))
        {
            GameResult result = ES3.Load<GameResult>(EasySaveKeys.KapibaraMiniGame);
            Debug.Log(result);
        }
    }

    private void Start() // wordSoundPairs = wordSoundPairsCapibara; gameLetters = ToCharArray. start sound and letters
    {
        audioManager = AudioManagerCapibara.Instance;
       
        passedRandomLetters = new List<int>();
        lastTempList = new List<char>();
        LoadAndShowLastGameProgress();
        gameManager = GameManager.Instance;
        unitManager = UnitManager.Instance;
        unitProgressManager = UnitProgressManager.Instance;

        wordSoundPairs = gameManager.currentUnit.wordSoundPairsCapibara;
        gameLetters = gameManager.currentUnit.unitId.ToCharArray();
        

    }
    

    public void SetLala() // SetLala 
    {

        fingerPointerTask = new Task(FiveSecondWaitForPointer());
        kornjaca.AllowClicking(true);

        currentLetter = wordSoundPairs[wordCount].wordText.Substring(0, 1);
        GameResult = new KapibaraGameResult(gameManager.currentUnit.unitId);

        onNewWordAdded.Invoke(wordSoundPairs[wordCount].wordText);
        passedWords.Add(wordSoundPairs[wordCount].wordText);
        currentWord = wordSoundPairs[wordCount].wordText;

    }

    public static List<T> Randomize<T>(List<T> list) //randomizedList
    {
        List<T> randomizedList = new List<T>();
        var newList = new List<T>(list);

        System.Random rnd = new System.Random();
        while (newList.Count > 0)
        {
            int index = rnd.Next(0, newList.Count); //pick a random item from the master list
            randomizedList.Add(newList[index]); //place it at the end of the randomized list
            newList.RemoveAt(index);
        }
        return randomizedList;
    }

    public void ReloadKapibaraGame()
    {
        SceneManager.LoadScene("KapibaraScena");
    }

    Sprite FindWithSameName(char letter)// gamelogic for letters
    {
        // ovde ide logika biranja malih/velikih slova
        if(AvatarBase.Instance.typeOfChosenFont == TypeOfChosenLetterFont.LOWERCASE)
        {
            foreach (var elem in gameManager.currentUnit.capibaraLetterSprites)
            {
                if (elem.name.Equals(letter.ToString()))// change to string
                {
                    return elem;
                }
            }
        }
        else
        {
            foreach (var elem in gameManager.currentUnit.capibaraLetterCapitalSprites)
            {
                if (elem.name.ToLower().Equals(letter.ToString()))// change to string
                {
                    return elem;
                }
            }
        }
        
        return null;
    }

    bool CheckForDuplicateList(char correctLetter, int correctIndex, List<char> tempList, List<char> lastTempList )//CheckForDuplicateList gamelogic
    {
        var temporaryArray = new char[3];
        temporaryArray[correctIndex] = correctLetter;
        int j = 0;
        for (int i = 0; i < temporaryArray.Length; i++)
        {
            if (i != correctIndex)
            {
                temporaryArray[i] = tempList[j];
                j++;
            }
            else
                temporaryArray[i] = correctLetter;
        }

        for (int i = 0; i < temporaryArray.Length; i++)
        {
            if (lastTempList[i] != temporaryArray[i])
                return true;
        }
        return false;
    }

    public void SetKapibara() // SetKapibara gamelogic
    {
        if (!capibaraIsSet)
        {

            bool correctItemSet = false;
            int correctItemIndex = lettersOrder[wordCount] - 1;
            char correctLetter = gameManager.currentUnit.unitLetters[correctItemIndex];
            var tempList = gameManager.currentUnit.unitLetters.Except(new List<char>() { correctLetter }).ToList();
            if(lastTempList.Count > 0)
            {
                do
                {
                    tempList = Randomize<char>(tempList);
                }
                while (!CheckForDuplicateList(correctLetter, correctItemIndex, tempList, lastTempList));
                lastTempList.Clear();
            }
            else
                tempList = Randomize<char>(tempList);

            

            int j = 0;
            for (int i = 0; i < kapibaraNumber; i++)
            {
                Debug.Log("USAO SAM OVDE");
                kapibaraList[i].interactable = false;
                kapibaraList[i].gameObject.SetActive(true);
                kapibaraList[i].capibaraVisual.gameObject.SetActive(true);
                if(i == correctItemIndex)
                {
                    kapibaraList[i].SetCurrentLetter(correctLetter, FindWithSameName(correctLetter));
                    lastTempList.Add(correctLetter);
                }
                else
                {
                    kapibaraList[i].SetCurrentLetter(tempList[j], FindWithSameName(tempList[j]));
                    lastTempList.Add(tempList[j]);
                    j++;
                }
                
                if (kapibaraList[i].capibaraVisual.IsFeared())
                {
                    kapibaraList[i].capibaraVisual.JumpBackIn();
                }
            }
            capibaraIsSet = true;
            
        }

        
        
    }

    public void DisableHandPointer(bool disable) //DisableHandPointer
    {
        HandPointer.SetActive(disable);
    }

    public void SetKornjacaWord()//  kornjaca DisplayCurrentWord wordCount or randomIndex
    {
        if(wordCount <= 2)
            kornjaca.DisplayCurrentWord(wordSoundPairs[wordCount]);
        else
            kornjaca.DisplayCurrentWord(wordSoundPairs[randomIndex]);
        foreach (var kapibara in kapibaraList)
            kapibara.interactable = true;
    }

    void ShowResultScreen()//ShowResultScreen
    {
        Debug.Log($"statistika igre :{GameResult}");
    }

    IEnumerator FiveSecondWaitForPointer() {//FiveSecondWaitForPointer HandPointer
        yield return new WaitForSeconds(10f);
        if (!kornjaca.isKornjacaClicked)
        {
            HandPointer.SetActive(true);
        }
        fingerPointerTask.Stop();
    }

    public void TurnOffHandPointer()//TurnOffHandPointer
    {
        DisableHandPointer(false);
    }

    public void ReloadCapybaraGame()// load KapibaraScena
    {
        SceneManager.LoadScene("KapibaraScena");
    }

    public IEnumerator StartNewWord()//StartNewWord logic
    {
        
        kornjaca.ResetText();
        progressBarSet = false;
        fingerPointerTask = new Task(FiveSecondWaitForPointer());

        if (brojPokusaja == 1)
            score++;
        wordCount++;
        if (wordCount == wordSoundPairs.Count - 1)
            lastTrial = true;
        if (wordCount < wordSoundPairs.Count)
        {  
            brojPokusaja = 0;
            
            yield return new WaitForSeconds(1f);
            SetKapibara();
            kornjaca.AllowClicking(true);
            if(wordCount <= 2)
            {
                currentLetter = wordSoundPairs[wordCount].wordText.Substring(0, 1);
                passedWords.Add(wordSoundPairs[wordCount].wordText);
                onNewWordAdded.Invoke(wordSoundPairs[wordCount].wordText);
                currentWord = wordSoundPairs[wordCount].wordText;
            }
            else
            {
                randomIndex = lettersOrder[wordCount] - 1;
                currentLetter = gameLetters[randomIndex].ToString();
                onNewWordAdded.Invoke(GetNextWord(gameLetters[randomIndex]));
                
            }
             // ovde dodajem deo za randomizovanje
            
        }
            
        else
        {
            yield return new WaitForSeconds(GameManager.NaratorWaitTimeWhenStartToTalk);
            GameResult.CalculateScoreForEachLetter(score);
            Debug.Log(GameResult);
           // VisualObjectsParent.SetActive(false);
            ResultWindow.gameObject.SetActive(true);
            ResultWindow.SetZvezdicaTextNew(score,12 - score, ScoringSystem);
            unitProgressManager.AddProgressForUnlockableGame(BasicGames.CAPIBARA);
            foreach(var capi in kapibaraList)
                capi.gameObject.SetActive(false);
            yield return null;
        }
        progressBar.lala.GetComponent<Collider>().enabled = true;
        newWordStarted = false;


    }

    public void SkipCapybara() // SkipCapybara button load mapscene
    {
        GameResult = new KapibaraGameResult(gameManager.currentUnit.unitId);

        GameResult.GenerateAScoreForGameSkipping();
        unitProgressManager.AddProgressForUnlockableGame(BasicGames.CAPIBARA);
        SceneManager.LoadScene("MapScene");
    }

    string GetNextWord(char letter)// temp = wordSoundPairs[i].wordText;  return temp;
    {
        string temp = "";
        for (int i = 0; i < wordSoundPairs.Count; i++)
        {
            if (wordSoundPairs[i].wordText.Substring(0, 1).ToLower().ToCharArray()[0] == letter && !passedWords.Contains(wordSoundPairs[i].wordText))
            {
                temp = wordSoundPairs[i].wordText;
                break;
            }
        }
        currentWord = temp;
        passedWords.Add(temp);
        return temp;

    }

    int GetRandomNextCharIndex() //GetRandomNextCharIndex 
    {
        var tempList = new List<char>();
        for(int i = wordCount; i < wordSoundPairs.Count; i++)
        {
            tempList.Add(wordSoundPairs[i].wordText.Substring(0, 1).ToCharArray()[0]);
        }

        int randomNumber;
        int j = 0;
        do
        {
            randomNumber = UnityEngine.Random.Range(3, wordSoundPairs.Count);
            Debug.Log("u ovom ciklusu nasumicno je izgenerisan " + randomNumber);
            if (!passedRandomLetters.Contains(randomNumber))
            {
                break;
            }

            j++;
        }while(passedRandomLetters.Contains(randomNumber));
        Debug.Log("broj koji je nasumicno izgenerisan " + randomNumber);
        passedRandomLetters.Add(randomNumber);

        return randomNumber;
    }



    public void StartReset(CapibaraBase capibaraVisual)//start BabyCapibaraReset  capibaraVisual
    {
        StartCoroutine(BabyCapibaraReset(capibaraVisual,RESETDELAY));
    }

    IEnumerator BabyCapibaraReset(CapibaraBase nonresetCapibara, float delay)// foreach capibara in kapibaraList
	//  ResetForNewQuestion or StartCoroutine BabyCapibaraOnShoreReset
    {
        yield return new WaitForSeconds(delay); 
        foreach(var capibara in kapibaraList)
        {
            if (capibara.capibaraVisual != nonresetCapibara)
            {
                if (!capibara.capibaraVisual.IsFeared())
                {
                    capibara.capibaraVisual.ResetForNewQuestion();
                }
                else
                {
                    StartCoroutine(BabyCapibaraOnShoreReset(capibara.capibaraVisual, MINIDELAY));
                }
                
            }
                
        }
    }

    IEnumerator BabyCapibaraOnShoreReset(CapibaraBase capibaraVisual, float miniDelay)// capibaraVisual JumpBackIn
    {
        yield return new WaitForSeconds(miniDelay);
        capibaraVisual.JumpBackIn();
    }

    public void CheckIfCorrectLetter(string letter, KapibaraLogic kapibara)// start CheckIfLetterIsGood
    {
        StartCoroutine(CheckIfLetterIsGood(letter, kapibara));
    }

    public AudioClip GetCurrentWordSound()// GetCurrentWordSound
    {
        if (wordCount <= 2)
            return wordSoundPairs[wordCount].wordSound;
        else
        {
            return wordSoundPairs.FirstOrDefault(w => w.wordText.ToLower().Equals(currentWord.ToLower())).wordSound;
        }
    }

    public void StartNewWordCapibara() // newWordStarted true and StartNewWord
    {
        if (!newWordStarted)
        {
            newWordStarted = true;
            StartCoroutine(StartNewWord());
        }
        
    }

    void CapibaraBabyInteractionControll(bool enable)//foreach kapibara enable
    {
        foreach (var kapibara in kapibaraList)
            kapibara.interactable = enable;
    }

    public IEnumerator CheckIfLetterIsGood(string letter,KapibaraLogic kapibara)// click on letter CheckIfLetterIsGood
    {
       progressBar.lala.GetComponent<Collider>().enabled = false;
        kornjaca.AllowClicking(false);
        kornjaca.kornjacaButton.interactable = false;
        CapibaraBabyInteractionControll(false);
        HandPointer.SetActive(false);
        brojPokusaja++;
        bool correct = currentLetter.ToLower().Equals(letter);
        progressBar.LalaAnswerReaction(correct);
        audioManager.PlayQuestionResultSound(correct);
        if (!progressBarSet)
        {
            progressBar.ChangeNodeBackground(wordCount, correct);
            progressBarSet = true;
        }

        if (!correct)
        {
            kornjaca.AllowClicking(false);
            GameResult.AddWrongWord(currentWord);
            kapibara.capibaraVisual.Fear();
            kapibara.DisplayLetter(false);
            ScaryCrocodiles(); 
            yield return new WaitForSeconds(0.8f);
            audioManager.PlaySound(GetCurrentWordSound());
            yield return new WaitForSeconds(0.2f);
          //  kapibara.gameObject.SetActive(false); OVDE JE BAG MOZDA
            CapibaraBabyInteractionControll(true);
            progressBar.lala.GetComponent<Collider>().enabled = true;
            yield return new WaitForSeconds(1f);
            kornjaca.AllowClicking(true);
        }
        else
        {
            kapibara.capibaraVisual.Jump();
            capibaraIsSet = false;
            DisableKapibare();
            kornjaca.AllowClicking(false);
            GameResult.AddProgressForALetter(letter.ToCharArray()[0], brojPokusaja == 1 ? 1 : 0);
            yield return new WaitForSeconds(1f);
        }
        
        
    }

    public void ActivateCapiGameobjects() //foreach  kapibara  in kapibaraList  SetActive true
    {
        foreach (var kapibara in kapibaraList)
        {
            kapibara.gameObject.SetActive(true);
            kapibara.capibaraVisual.gameObject.SetActive(true);
        }
    }

    void ScaryCrocodiles()// start CrocDelay
    {
       StartCoroutine(CrocDelay());
    }

    IEnumerator CrocDelay()// foreach  croc OpenMouth
    {
       foreach(var croc in CrocodileVisuals)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 1f));
            croc.OpenMouth();
        }       
    }

    public void RepeatTutorial()// RepeatTutorial
    {

        DisableHandPointer(false);

        fingerPointerTask = new Task(FiveSecondWaitForPointer());

        foreach (var kapibara in kapibaraList)
        {
            kapibara.interactable = false;
        }
       // kornjaca.AllowClicking(false);
        audioManager.RepeatTutorial();
      
    }

    public void EnableKapibare()// foreach kapibara in kapibaraList  enable kapibara 
    {
        foreach (var kapibara in kapibaraList)
        {
            kapibara.interactable = true;
        }
        kornjaca.AllowClicking(true);
    }
    void DisableKapibare()// foreach kapibara in kapibaraList  DisableKapibare
    {
        foreach (var kapibara in kapibaraList)
        {
            kapibara.interactable = false;
            kapibara.DisplayLetter(false);
        }
            
    }

    public void BackToMap()// load mapscene
    {
        SceneManager.LoadScene("MapScene");
    }
    
}
