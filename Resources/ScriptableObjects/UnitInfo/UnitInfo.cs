using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "ScriptableObjects/UnitInfo", order = 1)]
public class UnitInfo : ScriptableObject
{
    public int unitNumber;
    public string unitId;
    public List<char> unitLetters;
    public List<Sprite> unitLetterSprites;
    public List<Sprite> capibaraLetterSprites;
    public List<Sprite> capibaraLetterCapitalSprites;

    public Sprite LalaFriendSprite;
    [Header("Capybara game word/sound pairs")]
    public List<WordSoundPair> wordSoundPairsCapibara;
}
