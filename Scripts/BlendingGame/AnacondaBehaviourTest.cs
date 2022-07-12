using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnacondaBehaviourTest : MonoBehaviour
{
    // Start is called before the first frame update
    BlendingGameManager gameManager;
    public Slider mainSlider;
    public Mask crevoMaska;
    public SpriteRenderer SnakeHeadImage;
    bool alreadyPassed = false;
    bool passedMaskOffset = false;
    bool startedToDrag = false;
    public Animator anacondaAnimator;
    public float maskThreshold = 0.6662384f;
    AudioManagerBlendingGame audioManager;
    public AnacondaSoundPack sounds;
    public Button secondLetterButton;
    public Button firstLetterButton;

    public Image firstLetter;
    public Image secondLetter;

    bool isPlaying = false;

    List<char> BadPositionedLetters = new List<char> { 'f', 'b', 'i', 'l', 'p', 'g' };
    //0.6662384
    void Start()
    {
        gameManager = BlendingGameManager.Instance;
        audioManager = AudioManagerBlendingGame.Instance;
        audioManager.introEnded += SetInteractable;
        gameManager.onReset += ResetButtons;
       
       anacondaAnimator = GetComponent<Animator>();
       mainSlider.onValueChanged.AddListener(delegate { SliderValueCheck(); });
  
    }

    

    void SetInteractable()
    {
        firstLetterButton.interactable = true;
        secondLetterButton.interactable = true;
        firstLetterButton.gameObject.GetComponent<KnobReverse>().enabled = true;
        firstLetterButton.GetComponent<LetterButton>().interactable = true;
        secondLetterButton.GetComponent<LetterButton>().interactable = true;
    }

    public void AllowClickOnLetters(bool allow)
    {
        firstLetterButton.GetComponent<LetterButton>().enabled = allow;
        secondLetterButton.GetComponent<LetterButton>().enabled = allow;
    }

    void ResetButtons()
    {
        firstLetterButton.GetComponent<LetterButton>().isGlued = false;
        secondLetterButton.GetComponent<LetterButton>().isGlued = false;
    }

    public void GetLetters(List<char> letters)
    {
        if(AvatarBase.Instance.typeOfChosenFont == TypeOfChosenLetterFont.LOWERCASE)
        {
            if (BadPositionedLetters.Contains(letters[0])){
                firstLetter.sprite = Resources.Load<Sprite>($"LetterImages/Centered lower/{letters[0].ToString().ToLower()}");
            }
            else
            {
                firstLetter.sprite = Resources.Load<Sprite>($"LetterImages/small letters/{letters[0]}");
            }

            if (BadPositionedLetters.Contains(letters[1]))
            {
                secondLetter.sprite = Resources.Load<Sprite>($"LetterImages/Centered lower/{letters[1].ToString().ToLower()}");
            }
            else
                secondLetter.sprite = Resources.Load<Sprite>($"LetterImages/small letters/{letters[1]}");

        }
        else
        {
            firstLetter.sprite = Resources.Load<Sprite>($"LetterImages/capital letters brown/{letters[0].ToString().ToUpper()}");
            secondLetter.sprite = Resources.Load<Sprite>($"LetterImages/capital letters brown/{letters[1].ToString().ToUpper()}");
        }
       
    }

    public void GetSounds(AnacondaSoundPack soundPack)
    {
        sounds = soundPack;
        //mainSlider.gameObject.GetComponent<BlendingSliderLogic>()
    }

    public void PlayFirstLetterSound()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            StartCoroutine(WaitTimeBetweenPresses(sounds.firstLetterSound));
        }
        
    }


    IEnumerator WaitTimeBetweenPresses(AudioClip clipToWait)
    {
        gameManager.PlaySecondLetterSound(clipToWait);
        yield return new WaitForSeconds(clipToWait.length);
        isPlaying = false;
    }
    IEnumerator WaitTimeBetweenPressesBlendedSound(Slider clickedSlider)
    {
        gameManager.PlayBlendedLetterSound(sounds.blendedLettersSound, clickedSlider);
        yield return new WaitForSeconds(sounds.blendedLettersSound.length);
        isPlaying = false;
    }
    public void PlaySecondLetterSound()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            StartCoroutine(WaitTimeBetweenPresses(sounds.secondLetterSound));
            
        }
        
    }

    public void PlayBlendedLettersSound(Slider clickedSlider)
    {
        if (!isPlaying)
        {
            isPlaying = true;
            StartCoroutine(WaitTimeBetweenPressesBlendedSound(clickedSlider));
        }
        
    }

    public void EnableSnakeHead(bool enable)
    {
        SnakeHeadImage.enabled = enable;
    }

    public bool IsMaskEnabled()
    {
        return SnakeHeadImage.enabled;
    }

    private void Update()
    {
        anacondaAnimator.speed = 0f;
        anacondaAnimator.SetFloat("ControlParameter", mainSlider.value * 1.5f);
      //  mainSlider.interactable = gameManager.AllowDraggingSliders;
    }

    void SliderValueCheck()
    { 
       
        if (!startedToDrag)
        {
            startedToDrag = true;
            
        }
        anacondaAnimator.Play("Mouth opening", -1, mainSlider.value * 1.5f);
        if (mainSlider.value > maskThreshold)
            EnableSnakeHead(true);
        else
            EnableSnakeHead(false);

        if (mainSlider.value > maskThreshold  && !alreadyPassed)
        {
            
            if (mainSlider.value > 0.81 && !passedMaskOffset)
            {
                passedMaskOffset = true;
                alreadyPassed = true;
                crevoMaska.enabled = false;
            }
        }
    }
}
