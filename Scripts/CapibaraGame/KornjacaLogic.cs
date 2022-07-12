using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KornjacaLogic : MonoBehaviour
{

    string currentWord;
    TextMeshProUGUI currentWordText;
    public Button kornjacaButton;
    public bool isWordSet = false;
    public bool isKornjacaClicked = false;
    KapibaraGameManager gameManager;
    AudioManagerCapibara audioManager;

    private void Awake()
    {
        audioManager = AudioManagerCapibara.Instance;
        gameManager = KapibaraGameManager.Instance;
        currentWordText = GetComponent<TextMeshProUGUI>();
        kornjacaButton = GetComponent<Button>();
       // AllowClicking(false);
    }
    private void OnEnable()
    {
        gameManager.onNewWordAdded += SetCurrentWord;
    } 

    public void ResetText()
    {
        currentWordText.text = "";
        isKornjacaClicked = false;
    }

    public void DisplayCurrentWord(WordSoundPair wordSoundPair)
    {
        isKornjacaClicked = true;
        gameManager.DisableHandPointer(false);
        // gameManager.wordSoundPlayed.Invoke();

      
            Debug.Log("kliknuo si na kornjacu da prekines ponavljanje tutoriala");
            audioManager.StopTutorialRepetition();
            StartCoroutine(WaitBeforeEnable(gameManager.GetCurrentWordSound()));      
    }

    IEnumerator WaitBeforeEnable(AudioClip sound)
    {
        kornjacaButton.interactable = false;
        audioManager.PlaySound(sound);
        yield return new WaitForSeconds(sound.length);
        kornjacaButton.interactable = true;
    }

    public void AllowClicking(bool allow)
    {
        kornjacaButton.interactable = allow; 
    }

    public void SetCurrentWord(string word)
    {
        //gameManager.DisableHandPointer(true);

        if (!isWordSet)
        {
            currentWord = word;
            isWordSet = true;
        } 
    }
}
