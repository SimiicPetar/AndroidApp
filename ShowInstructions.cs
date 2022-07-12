using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInstructions : MonoBehaviour
{
    public bool interactable = false;

    private void Start()
    {
      //  LetterTracingManager.Instance.onFingerDoneDrawing += SetInteractable;
    }

    void SetInteractable()
    {
        interactable = true;
    }

    private void OnMouseOver()
    {
        if(interactable && Input.GetMouseButtonDown(0))
        {
            LetterTracingManager.Instance.ShowFingerPrompt();
            interactable = false;
        }
    }
}
