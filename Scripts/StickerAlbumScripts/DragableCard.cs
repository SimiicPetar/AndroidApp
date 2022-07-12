using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragableCard : MonoBehaviour,  IDragHandler, IEndDragHandler
{
    //0 je obicna lala je 1
    public int TypeOfDragableCard; 
    public delegate void OnCardPlacedInSlot();
    public static OnCardPlacedInSlot onCardPlacedInSlot;
    public GameObject RewardCard;
    public Image TargetWordImage;
    public Image LetterTextImage;
    public TextMeshProUGUI LetterText;
    public AudioClip CardFitnInSound;

    public char CardLetter;

    bool openUnitAfterSticking = false;

    bool dragable = false;

    Vector3 CardSlotPosition;

    Vector3 startPosition;

    Animator cardAnimator;

    Quaternion endRotation;

    Transform startingParent;

    public float StickOffset = 1f;

    void Awake()
    {
        cardAnimator = GetComponent<Animator>();
        StickerAlbumManager.OnNewCardReceived += SetDragable;
    }

    private void OnDisable()
    {
        StickerAlbumManager.OnNewCardReceived -= SetDragable;
        dragable = false;
    }
    void SetDragable(char letter)
    {
        if(letter == CardLetter)
        {
            dragable = true;
            startPosition = transform.position;
        }
    }

    public void Init(Vector3 position, Sprite TargetWordSprite, Sprite cardSprite, char LetterChar, Quaternion rotation, bool openAfterSticking = false)
    {
        openUnitAfterSticking = openAfterSticking;

        if (TypeOfDragableCard == 1)
            startingParent = transform.parent;

        transform.parent = transform.parent.parent.parent.parent;
        transform.SetSiblingIndex(4);
        //ovde ide resavanje image-a umesto teksta

        if (TypeOfDragableCard == 0)
            LetterTextImage.sprite = GameManager.Instance.GetLetterImage(LetterChar.ToString(), AvatarBase.Instance.typeOfChosenFont == TypeOfChosenLetterFont.UPPERCASE ? true : false);
        else
            LetterTextImage.gameObject.SetActive(false);
        LetterText.text = "";
        endRotation = rotation;
        dragable = true;
        startPosition = transform.position;
        CardSlotPosition = position;
        CardLetter = LetterChar;
        RewardCard.SetActive(true);
        RewardCard.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        RewardCard.GetComponent<Image>().sprite = cardSprite;
        TargetWordImage.gameObject.SetActive(true);
        TargetWordImage.sprite = TargetWordSprite;
        UIMapManager.Instance.OpenStickerAlbumWindow();
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (dragable)
        {
            var temp = Camera.main.ScreenToWorldPoint(eventData.position);
            transform.position = new Vector3(temp.x, temp.y, 0f);
        }
    }

    IEnumerator WaitForFitSound(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        AudioManagerMap.Instance.PlayBinSound();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragable)
        {
            Vector3 WithoutZ = new Vector3(CardSlotPosition.x, CardSlotPosition.y, 0f);
            float distance = Vector2.Distance(transform.position, WithoutZ);
            if (Vector2.Distance(transform.position, WithoutZ) > StickOffset)
            {
                transform.position = startPosition;
            }
            else
            {
               // if (leftPageToReset != null)
                 //   leftPageToReset.ResetLeftPage();
                dragable = false;
                transform.position = CardSlotPosition;
                onCardPlacedInSlot?.Invoke();
                transform.rotation = endRotation;
                cardAnimator.SetTrigger("Stick");
                //if()
                AudioManagerMap.Instance.PlaySound(CardFitnInSound);
                StartCoroutine(WaitForFitSound(CardFitnInSound.length));
                if (TypeOfDragableCard == 1)
                    transform.parent = startingParent;
                /*if (openUnitAfterSticking)
                    UIMapManager.Instance.OpenUnitWindow(GameManager.Instance.currentUnit);
                openUnitAfterSticking = false;*/
            }
        }
        
    }
}
