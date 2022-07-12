using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AnacondaSoundPack
{
    public AudioClip firstLetterSound;
    public AudioClip secondLetterSound;
    public AudioClip blendedLettersSound;
}

[CreateAssetMenu(fileName = "Anaconda sounds", menuName = "ScriptableObjects/Sounds for blending game", order = 1)]
public class AnacondaSounds : ScriptableObject
{
    [Header("zvukovi za prvu anakondu")]
    public AnacondaSoundPack firstAnacondaSounds;
    [Header("zvukovi za drugu anakondu")]
    public AnacondaSoundPack secondAnacondaSounds;
    [Header("zvukovi za trecu anakondu")]
    public AnacondaSoundPack thirdAnacondaSounds;
}
