using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RockBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject PlayLetterSoundButton;
    public Sprite FinalArmadilloSprite;
    public List<Sprite> RockSkins;
    public Sprite ArmadilloSprite;
    public Sprite ActiveRockSprite;
    ConsolidationGameManagerNew gameManager;
    bool _interactable = true;
    Color _wrongColor = Color.red;
    SpriteRenderer _rockImage;
    SpriteRenderer _resultImage;
    public string letter;

    Sprite _startingImage;

    public GameObject SleepingArmadilloSprite;

    int xCoordinate;
    int yCoordinate;
    public bool isFinalRock = false;

    public int XCoordinate { get => xCoordinate; set => xCoordinate = value; }
    public int YCoordinate { get => yCoordinate; set => yCoordinate = value; }


    void Awake()
    {
        _rockImage = GetComponent<SpriteRenderer>();
        _rockImage.sprite = RockSkins[0];
    }
    void Start()
    {
        gameManager = ConsolidationGameManagerNew.Instance;
        
        _resultImage = transform.GetChild(0).GetComponent<SpriteRenderer>();

      

        _resultImage.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);    

       // _rockImage.sprite = RockSkins[Random.Range(0, RockSkins.Count - 1)];

        _startingImage = _rockImage.sprite;
    }

    public void SetInteractable(bool interact)
    {
        _interactable = interact;
    }

    public void ArmadilloSetup()
    {
        _resultImage.gameObject.SetActive(true);
        _resultImage.sprite = gameManager.ArmadilloReward ;
        _interactable = false;
    }

    public void SetLetter(Sprite letterSprite,string letter, bool isLastLetter = false)
    {
        if (isLastLetter)//ovde armadilla ubacimo 
        {
            GetComponent<SpriteRenderer>().sprite = null;
            this.letter = letter;
            _resultImage.gameObject.SetActive(true);
            _resultImage.sprite = letterSprite;
            _resultImage.color = Color.white;
            _interactable = true;
        }
        else
        {
            Debug.Log("usli smo u setovanje spritea od kamena  " + gameObject.name);
            _resultImage = transform.GetChild(0).GetComponent<SpriteRenderer>();
            _resultImage.sprite = letterSprite;
            this.letter = letter;
            _resultImage.gameObject.SetActive(true);
            _interactable = true;
          //  _rockImage.sprite = ActiveRockSprite;
        }
        
    }

    public void SetWrongAnswer()
    {
        SleepingArmadilloSprite.SetActive(true);
        // _resultImage.sprite = ArmadilloSprite;
        _resultImage.sprite = null;
        _interactable = false;
    }

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0) && isFinalRock)
        {
            SceneManager.LoadScene("LalaLetterMachine");
        }
        if(Input.GetMouseButtonDown(0) && _interactable)
        {
            AudioManagerConsolidationGame.Instance.StopTutorialRepetition();
            if(!PlayLetterSoundButton.activeInHierarchy)
                gameManager.CheckIfPressingCorrectRock(letter, this);
        }


    }

    public void Init(int x, int y)
    {
        
        gameObject.name = "kamen" + x + ", " + y;
        xCoordinate = x;
        yCoordinate = y;
    }

    public void Reinit()
    {
        //_rockImage.sprite = RockSkins[Random.Range(0, RockSkins.Count - 1)];
        gameObject.name = "kamen" + XCoordinate + ", " + YCoordinate;
    }

    public void TopRowRockReset()
    {
        if (SleepingArmadilloSprite.activeSelf)
            SleepingArmadilloSprite.SetActive(false);
        _rockImage.enabled = true;
        _rockImage.color = Color.white;
        _rockImage.sprite = _startingImage;
        this.letter = null;
        _resultImage.sprite = null;
        _resultImage.gameObject.SetActive(false);
        _interactable = false;
    }

    public void ResetRock()
    {
        PlayLetterSoundButton.SetActive(false);
        this.letter = null;
        _resultImage.sprite = null;
        _rockImage.sprite = _startingImage;
        _resultImage.gameObject.SetActive(false);
        _interactable = false;
        if (SleepingArmadilloSprite.activeSelf)
            SleepingArmadilloSprite.SetActive(false);

    }

    public void ShowPlaySoundButtonOnCurrentStandingRock()
    {
        PlayLetterSoundButton.SetActive(true);
    }

}
