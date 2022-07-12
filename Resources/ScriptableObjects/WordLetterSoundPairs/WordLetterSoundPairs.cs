using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WordLetterSoundPair
{
    public char letter;
    public AudioClip WordSound;
    public AudioClip LetterSound;
    public Sprite Image;
}
[CreateAssetMenu(fileName = "WordLetterSoundPairs", menuName = "ScriptableObjects/WordLetterSoundPair", order = 1)]
public class WordLetterSoundPairs : ScriptableObject
{
    // Start is called before the first frame update
    public List<WordLetterSoundPair> wordLetterSoundPairs;

    public WordLetterSoundPair GetPairByLetter(char input)
    {
        foreach (var item in wordLetterSoundPairs)
        {
            if (item.letter.Equals(input))
                return item;
        }

        return new WordLetterSoundPair();
    }
}
