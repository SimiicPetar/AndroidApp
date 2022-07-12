using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ButtonType { LEFT, RIGHT}
public class LetterButton : MonoBehaviour, IPointerDownHandler
{
    public ButtonType TypeOfButton;
    public AnacondaBehaviourTest parent;
    public bool isGlued = false;
    public bool interactable = true;
    AudioManagerBlendingGame audioManager;


    public void OnPointerDown(PointerEventData eventData)
    {
        if (BlendingGameManager.Instance.clickAllowed)
        {
            audioManager.StopTutorialRepetition();
            if (!isGlued)
            {
                if (TypeOfButton == ButtonType.LEFT)
                {
                    parent.PlayFirstLetterSound();
                }
                else
                    parent.PlaySecondLetterSound();
            }
            else
            {
                parent.PlayBlendedLettersSound(parent.mainSlider);
            }
        }
        
    }

   


    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManagerBlendingGame.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
