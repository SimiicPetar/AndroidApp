using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "AnacondaLetters", menuName = "ScriptableObjects/LettersForAnaconda", order = 1)]
public class AnacondaLetters : ScriptableObject
{
    public List<char> FirstAnacondaLetters;
    public List<char> SecondAnacondaLetters;
    public List<char> ThirdAnacondaLetters;
}
