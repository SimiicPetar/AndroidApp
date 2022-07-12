using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


//pozicija donjeg letter teksta kod dragable : Vector3(0,-78,0)
//lenjivac taman tukan svetao majmun taman delfin svetao vuk taman tapir svetao jaguar taman
public class LeftPageLogic : LeftPageBaseClass
{
    public List<CardSlot> CardSlots;
    public DragableCard dragableCard;

    Transform bigParent;
    bool isSwitched = false;

    private void Awake()
    {
        bigParent = transform.parent;
    }

    public void ResetLeftPage()
    {
        bigParent.GetComponentInChildren<LeftPageLogic>().transform.SetSiblingIndex(1);
        bigParent.GetComponentInChildren<LeftPageLogicLalaSticking>().transform.SetSiblingIndex(0);
    }
    public void FillLeftPage(StickerAlbumInfo info, int index = 0)
    {
        GetComponent<Image>().sprite = info.LeftPageSprite;
        dragableCard.gameObject.SetActive(false);
        if (index == 7)
        {

            //StickerAlbumManager.Instance.SwapSibilingIndexesInHierarchy(bigParent.GetChild(0), bigParent.GetChild(1));
            bigParent.GetComponentInChildren<LeftPageLogic>().transform.SetSiblingIndex(0);
            bigParent.GetComponentInChildren<LeftPageLogicLalaSticking>().transform.SetSiblingIndex(1);
            FillLeftPageForLala(info, index);
            isSwitched = true;
        }
        else
        {
            if(isSwitched)//pitamo da li se sada priakzuje ova lalina stranica tj da li se prikazivala ukoliko vrtimo unazad
            {
                bigParent.GetComponentInChildren<LeftPageLogic>().transform.SetSiblingIndex(1);
                bigParent.GetComponentInChildren<LeftPageLogicLalaSticking>().transform.SetSiblingIndex(0);
                isSwitched = false;
            }
            int i = 0;
            foreach (var letter in info.unitInfo.unitId)
            {
                if (UnitProgressManager.Instance.CheckIfLetterIsFinished(letter, index))
                {
                    CardSlots[i].InitCardSlot(info, true, letter.ToString(),
                        info.TargetWordSprites.FirstOrDefault(x => x.name.ToCharArray()[0].ToString().ToLower().Equals(letter.ToString())));
                }
                else
                {
                    CardSlots[i].InitCardSlot(info, false, letter.ToString());
                }
                i++;
            }
        }

        GetComponent<Image>().sprite = info.LeftPageSprite;


    }
    #region lalaStranicaFunkcije



    public void ActivateDragableCardForLalaSticking(StickerAlbumInfo info, char letter)
    {
       // bigParent.GetChild(0).
        bigParent.GetChild(1).GetComponent<LeftPageLogicLalaSticking>().ActivateDragableCard(info, letter);
    }

    public void FillLeftPageForLala(StickerAlbumInfo info, int index = 0)
    {
        var lalaPage = bigParent.GetChild(1);
        bigParent.GetComponentInChildren<LeftPageLogicLalaSticking>().FillLeftPage(info, index);
    }

    public void FillLeftPageForLalaForSticking(StickerAlbumInfo info, char letterFinished, int index = 0)
    {
        bigParent.GetComponentInChildren<LeftPageLogic>().transform.SetSiblingIndex(0);
        bigParent.GetComponentInChildren<LeftPageLogicLalaSticking>().transform.SetSiblingIndex(1);
        bigParent.GetComponentInChildren<LeftPageLogicLalaSticking>().FillLeftPageForSticking(info, letterFinished, index);
    }
    #endregion
    public void FillLeftPageForSticking(StickerAlbumInfo info, char letterFinished, int index = 0)
    {
        dragableCard.gameObject.SetActive(true);
        if (index == 7)
        {
            
            bigParent.GetChild(1).SetAsLastSibling();
            bigParent.GetChild(0).SetAsFirstSibling();
            FillLeftPageForLalaForSticking(info, letterFinished, index);
        }
        else
        {
            bigParent.GetChild(0).SetAsLastSibling();
            bigParent.GetChild(1).SetAsFirstSibling();
            int i = 0;
            foreach (var letter in info.unitInfo.unitId)
            {
                if (UnitProgressManager.Instance.CheckIfLetterIsFinished(letter, index))
                {
                    if (letter == letterFinished)
                    {
                        CardSlots[i].InitCardSlot(info, false, letter.ToString());
                    }
                    else
                    {
                        CardSlots[i].InitCardSlot(info, true, letter.ToString(),
                        info.TargetWordSprites.FirstOrDefault(x => x.name.ToCharArray()[0].ToString().ToLower().Equals(letter.ToString())));
                    }
                }
                else
                {
                    CardSlots[i].InitCardSlot(info, false, letter.ToString());
                }
                i++;
            }
        }

        GetComponent<Image>().sprite = info.LeftPageSprite;


    }

    public void ActivateDragableCard(StickerAlbumInfo info,  char letter)
    {
        Sprite targetWordImage = info.TargetWordSprites.FirstOrDefault(x => x.name.ToCharArray()[0].ToString().ToLower().Equals(letter.ToString().ToLower()));
        Sprite CardImage = info.NewCardSprite;
        Vector3 position = CardSlots.FirstOrDefault(x => x.cardSlotChar.Equals(letter.ToString())).transform.position;
        Quaternion cardRotation = CardSlots.FirstOrDefault(x => x.cardSlotChar.Equals(letter.ToString())).transform.rotation;
        dragableCard.Init(position, targetWordImage, info.NewCardSprite, letter, cardRotation, true);
        UIMapManager.Instance.OpenStickerAlbumWindow();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
