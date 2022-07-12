using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "StickerAlbumInfo")]
public class StickerAlbumInfo : ScriptableObject
{
    [Header("vizuelni asseti")]
    public Sprite LeftPageSprite;
    public Sprite LalasFriendColored;
    public Sprite LalasFriendEmpty;
    public Sprite NameOfTheHouseSprite;
    public Sprite NewCardSprite;
    public List<Sprite> TargetWordSprites;

    [Space]
    [Header("informaije o unitu")]
    public UnitInfo unitInfo;


}
    // Start is called before the first frame update
   

