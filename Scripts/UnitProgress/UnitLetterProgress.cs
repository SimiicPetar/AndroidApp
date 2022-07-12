using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLetterProgress 
{
    public char letter;
    public bool soundLetterCompleted;
    public bool tracingLetterCompleted;
    public bool manateeGameCompleted;
    public UnitLetterProgress(char letter)
    {
        this.letter = letter;
        soundLetterCompleted = false;
        tracingLetterCompleted = false;
        manateeGameCompleted = false;
    }

    public UnitLetterProgress()
    {
        soundLetterCompleted = false;
        tracingLetterCompleted = false;
        manateeGameCompleted = false;
    }

    public void MiniGameCompleted(BasicGames code)
    {
        switch (code)
        {
            case BasicGames.LETTER_SOUND:
                {
                    soundLetterCompleted = true;
                    break;
                }
            case BasicGames.MANATEE:
                {
                    manateeGameCompleted = true;
                    break;
                }
            case BasicGames.TRACING:
                {
                    tracingLetterCompleted = true;
                    break;
                }
        }
    }


   public bool CheckIfUnitIsPlayedOnce()
    {
     
        return soundLetterCompleted;
    }

    public bool CheckIfLetterIsFinished()
    {
        if (soundLetterCompleted && manateeGameCompleted)
            return true;

        if (tracingLetterCompleted && manateeGameCompleted && soundLetterCompleted)
            return true;
        else
            return false;
    }

    public override string ToString()
    {
        return $"{letter} --->soundLetterCompleted:{soundLetterCompleted}|tracingLetterCompleted:{tracingLetterCompleted}|manateeGameCompleted:{manateeGameCompleted}";
    }
}

