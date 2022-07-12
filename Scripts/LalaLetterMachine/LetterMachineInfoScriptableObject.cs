using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public struct WordsForUnits
{
    public int holeID;
    public List<string> wordsList;
}
[CreateAssetMenu(fileName = "LalaLetterMachineInfo", menuName = "ScriptableObjects/LalaLetterMachineInfo", order = 1)]
public class LetterMachineInfoScriptableObject : ScriptableObject
{
    public List<string> wordsWithAccents = new List<string> { "sofa", "vovo", "cafe", "bone" };

    public List<Sprite> LowercaseAccentedLetters;

    public List<Sprite> UppercaseAccentedLetters;

    public List<Sprite> fallingLetterSpritesLowercase;

    public List<Sprite> fallingLetterSpritesUppercase;

    public List<Sprite> targetWordIllustrations;

    public List<AudioClip> targetWordSounds;

    public List<AudioClip> targetWordPhonemes;

    public List<WordsForUnits> WordsForUnit;
    
    public Sprite FindTargetWordIllustration(string name)
    {
        foreach(var illustration in targetWordIllustrations)
        {
            if (illustration.name.ToLower().Equals(name.ToLower()))
                return illustration;
        }

        return null;
    }

    public Sprite FindWithSameName(char letter, TypeOfChosenLetterFont type)
    {
        if(type == TypeOfChosenLetterFont.LOWERCASE)
        {
            foreach (var letterSprite in fallingLetterSpritesLowercase)
            {
                if (letterSprite.name.ToLower().Equals(letter.ToString()))
                    return letterSprite;
            }
        }
        else
        {
            foreach (var letterSprite in fallingLetterSpritesUppercase)
            {
                if (letterSprite.name.ToLower().Equals(letter.ToString()))
                    return letterSprite;
            }
        }
        

        return null;
    }

    public Sprite FindAccentedLetter(char letter, TypeOfChosenLetterFont type)
    {
        if(type == TypeOfChosenLetterFont.LOWERCASE)
        {
            return LowercaseAccentedLetters.FirstOrDefault(x => x.name.Contains(letter));
        }else
            return UppercaseAccentedLetters.FirstOrDefault(x => x.name.Contains(letter));
    }

    public Sprite FindTargetWordIllustration(char letter)
    {
        foreach (var illustration in targetWordIllustrations)
        {
            //str.Substring(0, 1);
            if (illustration.name.ToLower().Substring(0, 1).Equals(letter.ToString()))
                return illustration;
        }

        return null;
    }



    public AudioClip FindAudioClip(string name)
    {
        foreach(var sound in targetWordSounds)
        {
            if (sound.name.ToLower().Contains(name.ToLower()))
            {
                return sound;
            }
        }
        return null;
    }

    public AudioClip FindPhonemeAudioClip(string name)
    {
        foreach (var sound in targetWordPhonemes)
        {
            if (sound.name.ToLower().Contains(name.ToLower()))
            {
                return sound;
            }
        }
        return null;
    }
}
