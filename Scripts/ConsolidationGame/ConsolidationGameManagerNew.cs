using DG.Tweening;
using Patterns.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConsolidationGameManagerNew : Singleton<ConsolidationGameManagerNew>, IGameManager
{
    public delegate void PlayerInstantiated();
    public PlayerInstantiated onPlayerInstantiated;

    public float XOffsetArmadillo;
    public float YOffsetArmadillo;

    EndOfDiggerArmadillo endArmadillo;

    [SerializeField]
    List<RockBehaviour> rockList;
    RockBehaviour currentRock;
    AudioManagerConsolidationGame audioManager;
    List<string> Letters = new List<string>();
    GameObject _player;
    PlayerBehaviour _playerBehaviour;
    string currentLetter;
    List<int> passedRandomLetterIndexes = new List<int>();
    List<RockBehaviour> activeRocksList;
    bool progressBarSet;

    [SerializeField]
    Vector3 PlayerInstantiateOffset;
    public GameObject PlayerPrefab;
    public ProgressBarLogic progressBar;
    public Sprite ArmadilloReward;

    public GameObject EndOfTheGameArmadilloPrefab;
    // Start is called before the first frame update
    TypeOfChosenLetterFont typeOfChosenFont;
    string letterPath;
    AvatarBase avatarBase;
    int roundCount = 0;
    public int maxNumberOfRounds;
    Vector3 armadilloPosition;

    List<RockBehaviour> clickedRocks;

    public int NumberOfTrials { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    void Start()
    {
        clickedRocks = new List<RockBehaviour>();
        GameManager.Instance.ReturningFromHoleIndicator = true;
        if (AvatarBase.Instance != null)
        {
            avatarBase = AvatarBase.Instance;
            typeOfChosenFont = avatarBase.typeOfChosenFont;
        }
        else
        {
            typeOfChosenFont = TypeOfChosenLetterFont.LOWERCASE;
        }

        DetermineLetters();
        activeRocksList = new List<RockBehaviour>();
        currentLetter = DetermineNextLetter();
        progressBar.lala.GetComponent<Collider>().enabled = false;
        audioManager = AudioManagerConsolidationGame.Instance;
        InstantiatePlayer();
        SubscribeListeners();
    }

    public void SetArmadillo(EndOfDiggerArmadillo ar)
    {
        endArmadillo = ar;
    }

    void ResetRocks()
    {
        foreach (var rock in rockList)
            rock.ResetRock();
        currentRock.ResetRock();
        clickedRocks.Clear();
    }

    void SubscribeListeners()
    {
        audioManager.introEnded += () => { AllowClickingRocks(true); };
        audioManager.letterSoundStarted += ActiveRocksActivation;
        audioManager.letterSoundStarted += AllowClickingOnLala;
    }

    private void AllowClickingOnLala(bool start)
    {
       
    }

    private void ActiveRocksActivation(bool interactable)
    {
        
    }

    public void AllowClickingRocks(bool interactable)
    {
        foreach (var rock in activeRocksList)
            rock.SetInteractable(interactable);
    }

    public void GoToLetterMachine()
    {
        SceneManager.LoadScene("LalaLetterMachine");
    }

    public void JumpToEndArmadillo()
    {
        _playerBehaviour.Jump();
        _player.transform.DOJump(armadilloPosition, 5f, 1, 1f, false).SetEase(Ease.Linear).OnComplete(() =>
        {
            _playerBehaviour.Land();
            SceneManager.LoadScene("LalaLetterMachine");
        });

    }

    void DetermineLetters()
    {
        var helperList = new List<string>();
        
        
            foreach(var letter in GameManager.Instance.LettersForConsolidation)
            {
                helperList.Add(letter.ToString());
            }

        Letters = helperList;
    }

    string DetermineNextLetter()
    {
        int randomNumber;
        if (passedRandomLetterIndexes.Count == 12)
            return"a";


        do
        {
            randomNumber = UnityEngine.Random.Range(0, Letters.Count);
        } while (passedRandomLetterIndexes.Contains(randomNumber) || Letters[randomNumber] == currentLetter);

        passedRandomLetterIndexes.Add(randomNumber);

        return Letters[randomNumber];
    }

    public void CheckIfPressingCorrectRock(string letter, RockBehaviour rockBehaviour)
    {
        StartCoroutine(CheckIfPressingCorrectRockCoroutine(letter, rockBehaviour));
    }

    public void GoBackToMap()
    {
        SceneManager.LoadScene("MapScene");
    }

    internal IEnumerator CheckIfPressingCorrectRockCoroutine(string letter, RockBehaviour rockBehaviour)
    {
        bool correct = letter == currentLetter;
        
        progressBar.LalaAnswerReaction(correct);

        clickedRocks.Add(rockBehaviour);

        if (!progressBarSet)
        {
            progressBar.ChangeNodeBackground(roundCount, correct);
            progressBarSet = true;
        }

        rockBehaviour.SetInteractable(false);

        if (correct)
        {
            audioManager.PlayQuestionResultSound(correct);
            _playerBehaviour.Jump();
            StartCoroutine(LandAnimationDelay(0.7f));
            _player.transform.DOJump(rockBehaviour.transform.position + new Vector3(0f, 1.3f, 0f), 5f, 1, 1f, false).SetEase(Ease.Linear).OnComplete(() => {
                _playerBehaviour.Land();
                roundCount++;
                if (roundCount == maxNumberOfRounds)
                {
                    Debug.Log("stigao si do kraja");
                    AllowClickingRocks(false);
                    endArmadillo.clickable = true;
                    endArmadillo.ShowPointerHandAnim();
                }
                else
                {
                    currentLetter = DetermineNextLetter();
                    currentRock = rockBehaviour;
                    ResetRocks();
                    currentRock.ShowPlaySoundButtonOnCurrentStandingRock();
                    ShowPosibilities();                   
                }
                              
            });
        }
        else
        {
            audioManager.PlayQuestionResultSound(correct);
            rockBehaviour.SetWrongAnswer();
            yield return new WaitForSeconds(0.8f);
            AllowClickingRocks(false);
            yield return new WaitForSeconds(1.5f);
            AllowClickingRocks(true);
            foreach (var rock in clickedRocks)
                rock.SetInteractable(false);
        }
    }

    public void PlayCurrentLetterSound()
    {
        audioManager.PlayLetterSound(currentLetter.ToCharArray()[0]);
    }

    public void InstantiatePlayer()
    {
        
            _player = Instantiate(PlayerPrefab,rockList[0].transform.position + PlayerInstantiateOffset, Quaternion.identity);
            _playerBehaviour = _player.GetComponent<PlayerBehaviour>();
        //_player.GetComponent<AvatarSpineDressUp>().Dressup(AvatarBase.AvatarSpineDictionary[AvatarBase.Instance.ActiveAvatarKey]);
        if (AvatarBase.Instance == null)
            Debug.Log("m");
        else
            _player.GetComponent<AvatarSpineDressUp>().Dressup(AvatarBase.AvatarSpineDictionary[AvatarBase.Instance.ActiveAvatarKey]);
        onPlayerInstantiated.Invoke();
            _player.transform.localScale = new Vector3(0.490740001f, 0.490740001f, 0.490740001f);



        StartCoroutine(LandAnimationDelay(2.8f));
            _player.transform.DOMove(rockList[0].transform.position + new Vector3(0f, 0.9300008f + 0.3f, 0f), 3f, false).SetEase(Ease.Linear).OnComplete(() => {
                
                currentRock = rockList[0];
                currentRock.ShowPlaySoundButtonOnCurrentStandingRock();
                
            });
            
    }

    IEnumerator LandAnimationDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _playerBehaviour.Land();
    }

    public void ShowPosibilities(bool reset = true)
    {
        var currentlyReachableRocks = new List<RockBehaviour>();
        int currentRockIndex = rockList.IndexOf(currentRock);
        var helperList = new List<RockBehaviour>();

        audioManager.PlayLetterSound(currentLetter.ToCharArray()[0]);

        progressBarSet = false;

        if (typeOfChosenFont == TypeOfChosenLetterFont.LOWERCASE)
        {
            letterPath = Paths.WhiteLowercaseLettersPath;
        }
        else
        {
            letterPath = Paths.WhiteUppercaseLetterPath;

        }

        for (int i = currentRockIndex + 1; i < currentRockIndex + 6; i++)
        {
            if (i == rockList.Count - 1)
                break;
            helperList.Add(rockList[i]);
        }
        List<RockBehaviour> helperList2 = new List<RockBehaviour>();
        var help = rockList.OrderByDescending(order => order.transform.position.y).ToList();
        int count = 0;
        foreach(var rock in help)
        {
            if(rock.transform.position.y < currentRock.transform.position.y)
            {
                helperList2.Add(rock);
                count++;
                if (count == 3)
                    break;
            }
        }


        var currentPossibilityList = GetPossibilityList();

        bool isLastRound = roundCount == maxNumberOfRounds - 1;
        bool ArmadilloSet = false;

        if (isLastRound)
        {
            helperList2.OrderByDescending(x => x.transform.position.y);
            var tempList = currentPossibilityList;
            if (tempList[2] != currentLetter)
            {
                int currLetterIndex = tempList.IndexOf(currentLetter);
                var tmp = tempList[currLetterIndex];
                tempList[currLetterIndex] = tempList[2];
                tempList[2] = tmp;
            }
        }
            

        

        for (int i = 0; i < 3; i++)
        {
            Sprite result;
            if (isLastRound && currentPossibilityList[i] == currentLetter)
            {
                armadilloPosition = new Vector3(helperList2[i].transform.position.x + XOffsetArmadillo, helperList2[i].transform.position.y - YOffsetArmadillo);
                Instantiate(EndOfTheGameArmadilloPrefab, armadilloPosition, Quaternion.identity);
            }
            
                if (typeOfChosenFont == TypeOfChosenLetterFont.LOWERCASE)
                    result = Resources.Load<Sprite>(letterPath + currentPossibilityList[i]);
                else
                    result = Resources.Load<Sprite>(letterPath + currentPossibilityList[i].ToUpper());
                helperList2[i].SetLetter(result, currentPossibilityList[i]);
            
        }
        activeRocksList.Clear();
        activeRocksList = helperList;
    }

    List<string> GetPossibilityList()
    {
        System.Random rng = new System.Random();
        var randomizedAllLetters = Letters.OrderBy(a => rng.Next()).ToList();
        var helperList = new List<string>();
        helperList.Add(currentLetter);
        foreach (var letter in randomizedAllLetters)
        {
            if (helperList.Contains(letter))
                continue;
            else
                helperList.Add(letter);

            if (helperList.Count == 3)
                break;
        }

        return helperList.OrderBy(a => rng.Next()).ToList();
    }

    IEnumerator ClickBan()
    {
        AllowClickingRocks(false);
        yield return new WaitForSeconds(0.8f);
        AllowClickingRocks(true);
    }

    public void RepeatTutorial()
    {
       // AllowClickingRocks(false);
        audioManager.RepeatTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOffHandPointer()
    {
        throw new NotImplementedException();
    }
}
