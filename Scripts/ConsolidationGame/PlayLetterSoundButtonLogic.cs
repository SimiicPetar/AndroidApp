using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayLetterSoundButtonLogic : MonoBehaviour
{
    bool interactable = true;

    private void OnEnable()
    {
        AudioManagerConsolidationGame.Instance.letterSoundStarted += ResetInteractability;
    }

    private void OnDisable()
    {
        AudioManagerConsolidationGame.Instance.letterSoundStarted -= ResetInteractability;
    }

    private void OnMouseOver()
    {
        if(interactable && Input.GetMouseButtonDown(0))
            ConsolidationGameManagerNew.Instance.PlayCurrentLetterSound();
    }

    void ResetInteractability(bool letterSoundEventStatus)
    {
        interactable = !letterSoundEventStatus;
    }
}
