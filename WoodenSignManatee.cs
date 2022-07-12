using Patterns.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenSignManatee : Singleton<WoodenSignManatee>
{
    // Start is called before the first frame update
    ManateeAudioManager audioManager;
    ManateeGameManager gameManager;
    bool interactable = false;
    void Start()
    {
        audioManager = ManateeAudioManager.Instance;
        gameManager = ManateeGameManager.Instance;
        audioManager.wordSoundsOverEvent += SetInteractable;
        audioManager.introEnded += SetInteractable;
    }

    public void SetInteractable(bool enable)
    {
        interactable = enable;
        GetComponent<Collider>().enabled = enable;
    }

    void SetInteractable()
    {
        interactable = true;
    }

    void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0) && interactable)
        {
            StartCoroutine(PlayLetterSound());
        }
    }
    IEnumerator PlayLetterSound()
    {
        gameManager.progressBar.lala.interactable = false;
        interactable = false;
        AudioClip letterSound = gameManager.FindSoundOfLetter();
        audioManager.PlaySound(letterSound);
        yield return new WaitForSeconds(letterSound.length);
        interactable = true;
        gameManager.progressBar.lala.interactable = true;
    }
}
