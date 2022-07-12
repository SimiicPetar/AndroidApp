using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSlot : MonoBehaviour
{
    public string cardSlotChar;
    public TextMeshProUGUI letterText;
    public Image TargetWordImage;
    public Sprite DefaultCardSprite;
    public Image LetterTextImage;
    Vector3 defaultTextPosition;
    Vector3 defaultLetterTextImagePosition;

    private void Awake()
    {
        defaultTextPosition = letterText.transform.position;
        defaultLetterTextImagePosition = LetterTextImage.transform.position;
    }

    public void InitCardSlot(StickerAlbumInfo info,bool cardOwned,string cardChar, Sprite targetWordSprite = null)
    {
        cardSlotChar = cardChar;
        if (!cardOwned)
        {
            TargetWordImage.gameObject.SetActive(false);
            GetComponent<Image>().sprite = DefaultCardSprite;
            letterText.text = AvatarBase.Instance.typeOfChosenFont == TypeOfChosenLetterFont.UPPERCASE ? cardSlotChar.ToUpper() : cardSlotChar.ToString();
            //ovde ide deo za pronalazenje odgovarajuceg sprajta za slovo
            LetterTextImage.sprite = GameManager.Instance.GetLetterImage(cardSlotChar, AvatarBase.Instance.typeOfChosenFont == TypeOfChosenLetterFont.UPPERCASE ? true : false);
            LetterTextImage.transform.localPosition = Vector3.zero;
        }else
        {
            letterText.text = AvatarBase.Instance.typeOfChosenFont == TypeOfChosenLetterFont.UPPERCASE ? cardSlotChar.ToUpper() : cardSlotChar.ToString();
            LetterTextImage.sprite = GameManager.Instance.GetLetterImage(cardSlotChar, AvatarBase.Instance.typeOfChosenFont == TypeOfChosenLetterFont.UPPERCASE ? true : false);
            //ovde ide deo za pronalazenje odgovarajuceg sprajta za slovo, samim tim mislim da nece biti potrebe za pozicioniranjem teksta
            letterText.transform.localPosition = new Vector3(0f, -23.5f, 0f);
            LetterTextImage.transform.localPosition = new Vector3(0f, -22.62f, 0f);

            GetComponent<Image>().sprite = info.NewCardSprite;
            TargetWordImage.gameObject.SetActive(true);
            TargetWordImage.sprite = targetWordSprite;
        }

        letterText.text = "";
    }

    public void InitLalaCardSlotWhenOwned(StickerAlbumInfo info, bool cardOwned, string cardChar, Sprite targetWordSprite = null)
    {
        GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);

        TargetWordImage.gameObject.SetActive(true);
        TargetWordImage.sprite = targetWordSprite;
        letterText.text = "";
        //ovde mozda takodje dodje do neke sitne izmene
    }


}
