using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIElementClickabilityControl : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    { }
    

    private void OnDestroy()
    {

    }

    void ClickControll(float duration)
    {
        StartCoroutine(ClickControllCoroutine(duration));
    }

    IEnumerator ClickControllCoroutine(float duration)
    {
        GetComponent<Button>().interactable = false;
        yield return new WaitForSeconds(duration);
        GetComponent<Button>().interactable = true;
    }
}
