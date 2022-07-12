using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KnobReverse : MonoBehaviour
{
    public Slider mainSlider;
    float lerpDuration = 0.2f;
    Coroutine lerpCoroutine;
    BlendingGameManager gameManager;
    public AnacondaBehaviourTest anacondaParent;
    AudioManagerBlendingGame audioManager;
    private void Start()
    {
        audioManager = AudioManagerBlendingGame.Instance;
        gameManager = BlendingGameManager.Instance;
        gameManager.onReset += ResetAnacondas;

    }

   

    private void OnDisable()
    {
        gameManager.onReset -= ResetAnacondas;

    }

    public void EndDrag()
    {
        if (mainSlider.enabled)
        {
                if (gameManager.GetMaskState(anacondaParent))
                    gameManager.DeactivateHeadMask(anacondaParent,false);
                lerpCoroutine = StartCoroutine(LerpFunctionCoroutine(mainSlider.value));
        }
            
    }


    public void ResetAnacondas()
    {
        lerpCoroutine = StartCoroutine(LerpFunctionCoroutine(mainSlider.value));
    }

    IEnumerator LerpFunctionCoroutine(float currentSliderValue)
    {
        float timeElapsed = 0;

        while (timeElapsed < lerpDuration)
        {
            mainSlider.value= Mathf.Lerp(currentSliderValue, 0f, timeElapsed / (lerpDuration));
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        mainSlider.value = 0f;
    }
    // Start is called before the first frame update 

    public void BeginDrag()
    {
        if (lerpCoroutine != null && mainSlider.enabled)
            StopCoroutine(lerpCoroutine);
    }
}
