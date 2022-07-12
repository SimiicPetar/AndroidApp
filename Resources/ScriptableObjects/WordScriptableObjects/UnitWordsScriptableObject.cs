using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WordSoundPair
{
    public string wordText;
    public AudioClip wordSound;
    public AudioClip letterSound;
}
[CreateAssetMenu(fileName = "Words", menuName = "ScriptableObjects/WordsForUnit", order = 1)]
public class UnitWordsScriptableObject : ScriptableObject
{
    // Start is called before the first frame update
    public char FirstLetter;
    public List<string> Words;
    public List<WordSoundPair> wordSoundPairs;

    public List<WordSoundPair> GetWordSoundPairs()
    {
        return wordSoundPairs;
    }
}
