using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLetter : MonoBehaviour
{

    void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            LetterSoundGameManager.Instance.PlayLetterSound();
        }
    }
}
