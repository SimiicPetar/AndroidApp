using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlendingSliderLogic : MonoBehaviour
{
    public Slider mainSlider;
    char leftLetter = 'f'; // skucano na neko dok se ne napravi cela arhitektura projekta
    char rightLetter;
    BlendingGameManager gameManager;

    public AnacondaBehaviourTest anacondaParent;
    public AudioManagerBlendingGame audioManager;
    public Image SlidingLetterImage;
    public Image RightLetterImage;
    public Color EndColor;
    public Color StartColorSecondLetter;
    ProgressBarLogic progressBar;
    bool blended = false;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        gameManager.onReset -= ResetSlider;
    }

    private void Start()
    {
        progressBar = ProgressBarLogic.Instance;
        audioManager = AudioManagerBlendingGame.Instance;
        gameManager = BlendingGameManager.Instance;
        gameManager.onReset += ResetSlider;
        //leftLetter = gameManager.GetRightLetterForSlider();
        //rightLetter = gameManager.GetRightLetterForSlider();
       
        mainSlider = GetComponent<Slider>();
        mainSlider.onValueChanged.AddListener(delegate { SliderValueCheck(); });

        //slidingLetterText.text = leftLetter.ToString();
        //rightLetterText.text = rightLetter.ToString();
    }



    public void ResetSlider()
    {
        // rightLetterText.color = StartColorSecondLetter;
        //slidingLetterText.color = Color.white;
        SlidingLetterImage.color = Color.white;
        RightLetterImage.color = Color.white;
        mainSlider.enabled = true;
    }

    void SliderValueCheck()
    {
        
        if(mainSlider.value == 1f)
        {
            SlidingLetterImage.color = Color.red;
            RightLetterImage.color = Color.red;
            mainSlider.enabled = false;
            Debug.Log(leftLetter + rightLetter);
            StartCoroutine(FullBlendDelay());
        }
    }

    IEnumerator FullBlendDelay()
    {
        gameManager.DisableClicking();

        yield return new WaitForSeconds(0.5f);
        audioManager.PlayFitSound();
        yield return new WaitForSeconds(0.7f);
        audioManager.PlaySound(anacondaParent.sounds.blendedLettersSound);
        progressBar.LalaAnswerReaction(true);        
        yield return new WaitForSeconds(anacondaParent.sounds.blendedLettersSound.length);
        audioManager.PlaySound(audioManager.soundPack.correctAnswerSound);
        progressBar.ChangeNodeBackground(gameManager.TrialNumber++, true);       
        gameManager.EnableClicking();
        
        anacondaParent.firstLetterButton.GetComponent<LetterButton>().isGlued = true;
        anacondaParent.secondLetterButton.GetComponent<LetterButton>().isGlued = true;
        mainSlider.enabled = false;
    }
}
