using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManateeLogic : MonoBehaviour
{
    // Start is called before the first frame update
    public ManateeImageObject imageObject;

    ManateeGameManager gameManager;

    public ManateeVisualBehaviour manateeVisual;

    public GameObject wordArrow;


    public bool interactable = false;

    private void Start()
    {
        gameManager = ManateeGameManager.Instance;
    }
    public void SetImage(ManateeImageObject obj)
    {
        imageObject = obj;
        GetComponent<SpriteRenderer>().sprite = imageObject.Image;
    }

    public void ShowWordArrow(AudioClip clip)
    {
        if(!manateeVisual.IsManateeUnderWater())
            StartCoroutine(ShowArrow(clip));
    }

    IEnumerator ShowArrow(AudioClip clip)
    {
        wordArrow.SetActive(true);
        yield return new WaitForSeconds(clip.length);
        wordArrow.SetActive(false);
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && interactable && !manateeVisual.IsManateeUnderWater())
        {
            //letterBehaviour.Fade();
            gameManager.CheckIfCorrectImage(this);
            interactable = false;
        }
    }
    public void CheckIfCorrectLetter()
    {
        gameManager.CheckIfCorrectImage(this);
    }

}
