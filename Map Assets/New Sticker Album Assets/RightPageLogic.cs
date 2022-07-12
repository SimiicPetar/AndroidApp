using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightPageLogic : MonoBehaviour
{
    [Header("Dinamicki elementi koji se popunjavaju")]
    public LalaFriendStickerLogic lalaFriendSticker;
    public Image LalasFriendImage;
    public Image NameOfTheHouseImage;
    [Space]
    [Header("pozadine u zavisnosti od parnosti unita")]
    public Sprite LightGreenRightSide;
    public Sprite DarkGreenRightSide;
    public Sprite LalaRightSide;
    void Start()
    {
        
    }

    public void FillRightSideForSticking(StickerAlbumInfo info)
    {
        lalaFriendSticker.gameObject.SetActive(true);
        InitDragableSticker(info);
        LalasFriendImage.sprite = info.LalasFriendEmpty;
        NameOfTheHouseImage.sprite = info.NameOfTheHouseSprite;
        if(info.unitInfo.unitId == "")//ovo znaci da je lala album
        {
            GetComponent<Image>().sprite = LalaRightSide;
        }
        else if (info.unitInfo.unitNumber % 2 == 1)
        {
            GetComponent<Image>().sprite = DarkGreenRightSide;
        }
        else
            GetComponent<Image>().sprite = LightGreenRightSide;
    }

    public void InitDragableSticker(StickerAlbumInfo info)
    {
        lalaFriendSticker.Init(info.LalasFriendColored, LalasFriendImage.transform.position, true);
    }

    public void FillRightSide(StickerAlbumInfo info, bool completed)
    {
        lalaFriendSticker.gameObject.SetActive(false);
        //lalaFriendSticker.Init(null);
        LalasFriendImage.sprite = completed ? info.LalasFriendColored : info.LalasFriendEmpty;
        NameOfTheHouseImage.sprite = info.NameOfTheHouseSprite;
        if (info.unitInfo.unitId == "")//ovo znaci da je lala album
        {
            GetComponent<Image>().sprite = LalaRightSide;
        }
        else if (info.unitInfo.unitNumber % 2 == 1)
        {
            GetComponent<Image>().sprite = DarkGreenRightSide;
        }
        else
            GetComponent<Image>().sprite = LightGreenRightSide;
    }

    // Update is called once per frame
    
}
