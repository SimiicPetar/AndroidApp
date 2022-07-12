using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectableUnitLetter : MonoBehaviour
{
    public char letter;
    TextMeshProUGUI letterText;
    UnitManager unitManager;
    Image letterImage;
    public Image DoneImage;
    List<string> BadPositionedLetters = new List<string> { "f", "b", "i", "l", "p", "g", "j" };

    Task ClickBanTask;
    private void Awake()
    {
        letterImage = GetComponent<Image>();
       
    }
    private void Start()
    {
        unitManager = UnitManager.Instance;
    }
    public void SetText(char letter)
    {
        this.letter = letter;
        if(letterText != null)
            letterText.text = letter.ToString();
    }

    private void OnEnable()
    {
        UIMapManager.OnWindowClosed += Reset;
       ClickBanTask = new Task (SmallClickBanWhenEnabled(0.4f));

    }


    //kad se otvara prozor da ne kliknemo slucajno
    IEnumerator SmallClickBanWhenEnabled(float duration)
    {
        GetComponent<Button>().interactable = false;
        yield return new WaitForSeconds(duration);
        GetComponent<Button>().interactable = true;
        ClickBanTask.Stop();
    }

    private void OnDisable()
    {
        if(ClickBanTask != null)
            ClickBanTask.Stop();
        UIMapManager.OnWindowClosed -= Reset;
    }

    public void Reset(UIWindow unitView)
    {
        DoneImage.gameObject.SetActive(false);
        letterImage.color = Color.white;
    }

    public void SetTextColor(bool done)
    {
        DoneImage.gameObject.SetActive(false);
        if (letterImage == null)
            letterImage = GetComponent<Image>();
        letterImage.color = new Color(200f, 200f, 200f, 1f);
        if(AvatarBase.Instance.typeOfChosenFont == TypeOfChosenLetterFont.LOWERCASE)
        {
            if(!BadPositionedLetters.Contains(this.letter.ToString().ToLower()))
                letterImage.sprite = Resources.Load<Sprite>($"LetterImages/small letters/{this.letter}");
            else
                letterImage.sprite = Resources.Load<Sprite>($"LetterImages/Centered lower/{this.letter.ToString().ToLower()}");
        }    
        else
            letterImage.sprite = Resources.Load<Sprite>($"LetterImages/capital letters brown/{this.letter.ToString().ToUpper()}");
        if (done)
        {
            letterImage.color = new Color(200f / 255, 200f / 255, 200f / 255, 1f);
            DoneImage.gameObject.SetActive(true);
        }
        else
        {
            letterImage.color = Color.white;
        }

        
       
    }

    public void SetMiniGameLetter()
    {
        unitManager.SetCurrentLetter(letter);
    }
}
