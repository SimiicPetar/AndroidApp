using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LetterSound intro", menuName = "ScriptableObjects/letterSound intro", order = 1)]
public class LetterSoundIntros : ScriptableObject
{
    public char GameChar;
    public List<AudioClip> introSounds;
}
