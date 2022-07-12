using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfDiggerArmadillo : MonoBehaviour
{
    // Start is called before the first frame update

    public bool clickable = false;

    public GameObject PointerHand;

    void Start()
    {
        
    }

    public void ShowPointerHandAnim()
    {
        StartCoroutine(ShowPointerHand());
    }

    IEnumerator ShowPointerHand()
    {
        yield return new WaitForSeconds(3f);
        PointerHand.SetActive(true);
    }

    private void OnEnable()
    {
        ConsolidationGameManagerNew.Instance.SetArmadillo(this);
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && clickable)
        {
            ConsolidationGameManagerNew.Instance.JumpToEndArmadillo();
            if (PointerHand.activeSelf)
                PointerHand.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
