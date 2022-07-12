using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct TargetWordPairs
{
    public Sprite TargetWordImage;
    public char FirstLetter;
}


[CreateAssetMenu(fileName = "Target words", menuName = "ScriptableObjects/TargetWords", order = 1)]
public class TargetWords : ScriptableObject
{
    public List<TargetWordPairs> TargetWordPairs;
}
