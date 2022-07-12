using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LetterSoundPair
{
    private bool isEmpty;
    public char letter;
    public AudioClip letterSound;
    private object p1;
    private object p2;
    private bool v;

    public LetterSoundPair(object p1, object p2, bool v) : this()
    {
        this.p1 = p1;
        this.p2 = p2;
        this.v = v;
    }
}
[CreateAssetMenu(fileName = "Words", menuName = "ScriptableObjects/LetterWordPairsObject", order = 1)]
public class LetterSoundPairs : ScriptableObject
{
    // Start is called before the first frame update
    public List<LetterSoundPair> letterSoundPairs;
    
    public LetterSoundPair GetLetterSoundPair(char letter)
    {
        foreach(var pair in letterSoundPairs)
        {
            if (pair.letter == letter)
                return pair;
        }
        return new LetterSoundPair(null, null, true);
    }
}
