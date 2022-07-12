using Patterns.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickerAlbumFlipController : Singleton<StickerAlbumFlipController>
{
    // Start is called before the first frame update

    Animator PageFlipAnimator;
    public LeftPageLogic LeftPage1Content;
    public LeftPageLogic LeftPage2Content;
    public RightPageLogic RightPage1Content;
    public RightPageLogic RightPage2Content;

    public List<StickerAlbumInfo> unitAlbumInfoList;

    [Space]
    [Header("Navigation buttons")]
    public GameObject AlbumBeforeButton;

    public GameObject NextAlbumButton;

    public GameObject BackToMapButton;

    public GameObject GoToMapAfterStickingButton;



    int currentOpenAlbumIndex = 0;

    void Start()
    {
        
    }

    public void ChangeContentForNextAlbum()
    {

    }

    public void ChangeContentForPreviousAlbum()
    {

    }

    void SetupForScrolling()
    {
        AlbumBeforeButton.SetActive(true);
        NextAlbumButton.SetActive(true);
        BackToMapButton.SetActive(true);
        GoToMapAfterStickingButton.SetActive(false);
    }

   /* public void FillUpAlbum(int index = 0)
    {

        var currentAlbumInfo = unitAlbumInfoList[index];
        UIMapManager.Instance.OpenStickerAlbumWindow();
        SetupForScrolling();
        lalaFriendImage.sprite = UnitStatisticsBase.Instance.CheckIfUnitIsFinished(currentAlbumInfo.unitInfo.unitId) ? currentAlbumInfo.LalasFriendColored : currentAlbumInfo.LalasFriendEmpty;
        lalaFriendSticker.Init(null, true);

        // houseName.text = currentAlbumInfo.NameOfTheHouse;
        FillLeftSide();
        FillRightSide();
        int i = 0;
        foreach (var letter in currentAlbumInfo.unitInfo.unitId)
        {
            if (UnitProgressManager.Instance.CheckIfLetterIsFinished(letter, index))
            {
                RewardCardsUpsideDown[i].Init(true, currentAlbumInfo.TargetWordSprites[i], letter);
                RewardCardsUpsideDown[i].transform.position = RewardCardSlots[i].transform.position;
            }
            else
            {
                RewardCardsUpsideDown[i].Init(false);
                RewardCardSlots[i].letterText.text = AvatarBase.Instance.typeOfChosenFont == TypeOfChosenLetterFont.UPPERCASE ? letter.ToString().ToUpper() : letter.ToString();
            }
            i++;
        }
    }*/


    void FillLeftSide()
    {
       
    }

    void FillRightSide()
    {
       // RightPage1Content.FillRightSide();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
