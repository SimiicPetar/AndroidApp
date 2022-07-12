using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnteaterLogic : MonoBehaviour
{
    // Start is called before the first frame update
    char currentLetter;
    TextMeshProUGUI currentLetterText;
    Button AnteaterButton;
    bool isLetterSet = false;
    public bool isAnteaterClicked = false;
    AnteaterGameManager gameManager;
    AnteaterSoundManager audioManager;
    bool interactable = true;

    public AnteaterVisualBehaviour anteaterVisual;

    private void Awake()
    {
       
        currentLetterText = GetComponent<TextMeshProUGUI>();
        AnteaterButton = GetComponent<Button>();
    }


    private void Start()
    {
        audioManager = AnteaterSoundManager.Instance;
        gameManager = AnteaterGameManager.Instance;
        gameManager.onNewLetterAdded += SetCurrentLetter;
    }

    public void ResetText()
    {
        isAnteaterClicked = false;
        currentLetterText.text = "";
    }

    public void DisplayCurrentLetter(char word)
    {
        isAnteaterClicked = true;
        gameManager.DisableHandPointer(false);
        //currentLetterText.text = word.ToString();
        if (!gameManager.CheckIfAllAntsAreVisible())
            return;
        audioManager.StopTutorialRepetition();
        StartCoroutine(AnteaterWaitDelay());

       
    }

    IEnumerator AnteaterWaitDelay()
    {
        interactable = false;
        AudioClip soundOfLetter = gameManager.FindSoundOfLetter();

        audioManager.PlaySound(gameManager.FindSoundOfLetter());

        yield return new WaitForSeconds(soundOfLetter.length);
        interactable = true;
    }

    public void AllowClicking(bool allow)
    {
        AnteaterButton.interactable = allow;
    }

    private void SetCurrentLetter(char letter)
    {
        if (!isLetterSet)
        {
            currentLetter = letter;
            isLetterSet = true;
        }
    }
}
