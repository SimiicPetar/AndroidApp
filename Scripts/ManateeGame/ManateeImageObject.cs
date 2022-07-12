using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ManateeGameImage", menuName = "ScriptableObjects/ManateeGameImage", order = 1)]
public class ManateeImageObject : ScriptableObject
{
    public Sprite Image;
    public char firstLetter;
    public AudioClip wordSound;
}
