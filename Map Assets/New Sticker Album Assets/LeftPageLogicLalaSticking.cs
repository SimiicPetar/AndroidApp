using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftPageLogicLalaSticking : LeftPageBaseClass
{
    // Start is called before the first frame update
    public List<CardSlot> CardSlots;
    public DragableCard dragableCard;

    public void FillLeftPage(StickerAlbumInfo info, int index = 0)
    {
        //CardSlots[0].InitCardSlot(info, true, "a", info.NewCardSprite);
        CardSlots[0].InitLalaCardSlotWhenOwned(info, true, "", info.NewCardSprite);
    }

    public void FillLeftPageForSticking(StickerAlbumInfo info, char letterFinished, int index = 0)
    {
        CardSlots[0].InitCardSlot(info, false, "");
        ActivateDragableCard(info, letterFinished);
    }

    public void ActivateDragableCard(StickerAlbumInfo info, char letter)
    {
        Sprite targetWordImage = info.NewCardSprite;
        Sprite CardImage = info.NewCardSprite;
        Vector3 position = CardSlots[0].transform.position;
        Quaternion cardRotation = Quaternion.identity;
        dragableCard.Init(position, targetWordImage, info.NewCardSprite, letter, cardRotation);
        UIMapManager.Instance.OpenStickerAlbumWindow();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
