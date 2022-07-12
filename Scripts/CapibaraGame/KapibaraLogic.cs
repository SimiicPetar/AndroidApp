using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KapibaraLogic : MonoBehaviour
{
    // Start is called before the first frame update

    string currentLetter;

    TextMeshProUGUI currentLetterText;

    public CapibaraBase capibaraVisual;

    public bool interactable = false;

    KapibaraGameManager gameManager;

    CapibaraBase capibaraVisualObject;

    SpriteRenderer sprite;

    LetterBehaviour letterBehaviour;
    private void Awake()
    {
        letterBehaviour = GetComponent<LetterBehaviour>();
        if (AvatarBase.Instance.typeOfChosenFont == TypeOfChosenLetterFont.UPPERCASE)
            transform.localScale = new Vector3(0.1044f, 0.1044f, 0.1044f);
        //currentLetterText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        
        sprite = GetComponent<SpriteRenderer>();
        gameManager = KapibaraGameManager.Instance;
    }

    public void SetCurrentLetter(char letter, Sprite letterSprite)
    {
        
        GetComponent<SpriteRenderer>().sprite = letterSprite;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = letterSprite;
        currentLetter = letter.ToString();
        letterBehaviour.ResetToIdle();
        //currentLetterText.text = letter.ToString();
    }

    public void CheckIfCorrectLetter()
    {
        gameManager.CheckIfCorrectLetter(currentLetter, this);
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && interactable)
        {
            letterBehaviour.Fade();
            interactable = false;
            gameManager.CheckIfCorrectLetter(currentLetter, this);
        }
    }

    public void DisplayLetter(bool display)
    {
        if (display)
            letterBehaviour.ResetToIdle();
        else
            letterBehaviour.Fade();
    }
}
