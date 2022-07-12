using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LetterSoundGameManager : MonoBehaviour, IGameManager
{   //vela, ilha, pirata, dedo
    // -1.76
    private static LetterSoundGameManager _instance = null;
    public static LetterSoundGameManager Instance { get { return _instance; } }

    public int NumberOfTrials { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    readonly string PopUpTrigger = "PopUp";
    BasicGames gameCode = BasicGames.LETTER_SOUND;
    char currentLetter;
    List<ManateeImageObject> letterImagePairs;      //koristim iste parove slika slovo kao i za manatee igru jer mi je pogodno 
    GameManager gameManager;
    UnitManager unitManager;
    AvatarBase avatarBase;

    public Image ImageOfAnObject;
    public SpriteRenderer FirstLetterOfAnObject;
    public LalaLogic lala;
    public Button nextButton;
    public Animator IllustrationAnimator;
    public Animator LetterAnimator;
    public TargetLetter targetLetter;
    List<char> BadPositionedLetters = new List<char> { 'f', 'b', 'i', 'l', 'p', 'g', 'j' };
    public ProgressBarLalaVisualBehaviour lalaNarator;
    //f je ok, b nije, i je malo iznad u tracingu
    bool allowClicking = false;

    LetterSoundAudioManager audioManager;

    public GameObject nextGameImage;

    WordLetterSoundPair gamePair;


    public GameObject redoButton;
    public GameObject goToNextGameButton;

    char gameChar;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    void GetGamePair() // gameChar = currentLetter. gamePair = GetPairByLetter (gameChar)
    {
        gameChar = unitManager.GetCurrentLetter();
        gamePair = unitManager.wordLetterSoundPairs.GetPairByLetter(gameChar);
    }

    void SubscribeListeners() // intro ended allowClicking
    {
        audioManager.introEnded += AllowClicking;
    }

    void Start() // set active true or false gameobjects for start
    {
        avatarBase = AvatarBase.Instance;
        audioManager = LetterSoundAudioManager.Instance;
        nextGameImage.GetComponent<Button>().interactable = false;
        unitManager = UnitManager.Instance;
        gameManager = GameManager.Instance;
        redoButton.SetActive(false);
        goToNextGameButton.SetActive(false);
        FirstLetterOfAnObject.gameObject.SetActive(true);
        SubscribeListeners();
        GetGamePair();
        StartCoroutine(LalaIntroduction());
    }


    public void RestartGame() // restart game. turn off buttons and replay tutorial audio 
    {
        ImageOfAnObject.gameObject.SetActive(false);
        allowClicking = false;
        redoButton.SetActive(false);
       // goToNextGameButton.SetActive(false);
        audioManager.ReplayTutorial();
    }

    void AllowClicking() // AllowClicking in this scene
    {
        allowClicking = true;
        redoButton.SetActive(true);
        goToNextGameButton.SetActive(true);
    }

public void ActivateIllustrationPopUp() // setactive true ImageOfAnObject
    {
        ImageOfAnObject.gameObject.SetActive(true);
        IllustrationAnimator.SetTrigger(PopUpTrigger);
    }

    public void ActivateLetterPopUp() // setactive true FirstLetterOfAnObject
    {
        FirstLetterOfAnObject.gameObject.SetActive(true);
        LetterAnimator.SetTrigger(PopUpTrigger);
    }

    public void PlayLetterSound() // play sound LetterSound in this scene
    {
        if (allowClicking)
        {
            allowClicking = false;
            audioManager.PlaySound(gamePair.LetterSound);
            StartCoroutine(WaitForClickAllowing(gamePair.LetterSound.length));
        }
           
    }

    IEnumerator WaitForClickAllowing(float time) //delay allowClicking
    {
        yield return new WaitForSeconds(time);
        allowClicking = true;
    }

    public void PlayWordSound()// play sound WordSound in this scene
    {
        if (allowClicking)
        {
            allowClicking = false;
            audioManager.PlaySound(gamePair.WordSound);
            StartCoroutine(WaitForClickAllowing(gamePair.WordSound.length));
        }
            
    }

    IEnumerator LalaIntroduction() // set image after delay
    {
        yield return new WaitForSeconds(0f);
        SetImageAndText();
    }

    public void SetImageAndText()// game logic for letters in this scene
    {
        //ovde dodati logiku za mala velika slova
        ImageOfAnObject.sprite = gamePair.Image; 
        string path = "";
        if(avatarBase.typeOfChosenFont == TypeOfChosenLetterFont.LOWERCASE)
        {
            if (BadPositionedLetters.Contains(gameChar))
            {
                path = $"LetterImages/Centered lower/{gameChar.ToString().ToLower()}";
            }
            else
                path = $"LetterImages/small letters/{gameChar.ToString().ToLower()}";
        }         
        else
        {
            path = $"LetterImages/capital letters brown/{ gameChar.ToString().ToUpper()}";
            //FirstLetterOfAnObject.transform.parent.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }

        if (gameChar == 'd' && avatarBase.typeOfChosenFont == TypeOfChosenLetterFont.LOWERCASE)
        {
            FirstLetterOfAnObject.transform.position = new Vector3(0.349999994f, -2.21000004f, 0);
        }
        else if (gameChar == 'r')
            FirstLetterOfAnObject.transform.position = new Vector3(0.0500000007f, -0.379999995f, 0);

        Debug.Log(path);
        FirstLetterOfAnObject.sprite = Resources.Load<Sprite>(path);
        if (BadPositionedLetters.Contains(gameChar))
        {
            Vector3 vec = FirstLetterOfAnObject.gameObject.transform.position;
           // FirstLetterOfAnObject.gameObject.transform.position = new Vector3(vec.x, -1.76f, vec.z);
        }
           
        GoToNextLevel();

    }

 
    public void SkipButtonLogic() // skip button onclick load next scene
    {
        UnitProgressManager.Instance.SaveProgressForALetterInUnit(gameChar, gameCode);
        SceneManager.LoadScene("LetterTracingScene");
    }

    public void GoToNextLevel() // saves player progress
    {
        UnitProgressManager.Instance.SaveProgressForALetterInUnit(gameChar, gameCode);
        nextGameImage.GetComponent<Button>().interactable = true;
    }

   
    public void GoBackToMap() // go back to Mapscene
    {
        SceneManager.LoadScene("MapScene");
    }

    public void GoToManateeGame() // load next scene LetterTracingScene
    {
        SceneManager.LoadScene("LetterTracingScene");
    }

    public void RepeatTutorial() // onclick repeat tutorial turn off buttons and play audio again
    {
        ImageOfAnObject.gameObject.SetActive(false);
        allowClicking = false;
        redoButton.SetActive(false);
        goToNextGameButton.SetActive(false);
        audioManager.ReplayTutorial();
    }

    public void TurnOffHandPointer() // onclick turn off pointer hand
    {
        throw new System.NotImplementedException();
    }
}
