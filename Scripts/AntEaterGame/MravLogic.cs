using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MravLogic : MonoBehaviour
{
    char currentLetter;

    public bool interactable = false;

    TextMeshProUGUI currentLetterText;

    AnteaterGameManager gameManager;

    LetterBehaviour letterBehaviour;

    public AntVisualBehaviour antVisual;

    private void Awake()
    {
        letterBehaviour = GetComponent<LetterBehaviour>();
    }


    private void Start()
    {
        gameManager = AnteaterGameManager.Instance;
    }

    public void SetCurrentLetter(char letter, Sprite letterSprite)
    {
        currentLetter = letter;
       
        StartCoroutine(DelayForLetterFadeIn(1f, letterSprite));
    }

    IEnumerator DelayForLetterFadeIn(float delay, Sprite letterSprite)
    {
        yield return new WaitForSeconds(delay);
        GetComponent<SpriteRenderer>().sprite = letterSprite;
        letterBehaviour.ResetToIdle();
    }

    public void CheckIfCorrectLetter()
    {
        gameManager.CheckIfCorrectLetter(currentLetter, this);
    }

    public void FadeLetter()
    {
        letterBehaviour.Fade();
    }

    public void ShowLetter()
    {
        letterBehaviour.ResetToIdle();
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && interactable)
        {
            //letterBehaviour.Fade();
            gameManager.CheckIfCorrectLetter(currentLetter, this);
            interactable = false;
        }
    }

}
