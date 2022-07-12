using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sounds", menuName = "ScriptableObjects/Sounds", order = 1)]
public class SoundPackObject : ScriptableObject
{
    public AudioClip correctAnswerSound;
    public AudioClip wrongAnswerSound;
    public AudioClip noStarsSound;
    public AudioClip oneStarSound;
    public AudioClip threeAndTwoStarSound;
    public AudioClip fourStarSound;
}
