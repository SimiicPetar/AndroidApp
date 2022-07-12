using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecommendationsPanelManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<HowToPlayInfoDot> Dots;
    public List<GameObject> InformationPages;
    public Button LeftArrow;
    public Button RightArrow;
    GameObject _currentActivePage;
    int _currentOpenPageIndex = 0;
    void Initialization()
    {
        _currentOpenPageIndex = 0;
        LeftArrow.interactable = false;
        RightArrow.interactable = true;
        Dots[0].EnableDot();
        Dots[1].DisableDot();
        _currentActivePage = InformationPages[0];
        _currentActivePage.SetActive(true);
        foreach (var page in InformationPages)
        {
            if (page != _currentActivePage)
                page.SetActive(false);
        }
    }
    private void OnEnable()
    {
        Initialization();
    }

    private void OnDisable()
    {
        Initialization();
    }

    public void NextRecommendation()
    {
        if(_currentOpenPageIndex + 1 < InformationPages.Count)
        {
            Dots[_currentOpenPageIndex].DisableDot();
            _currentActivePage.SetActive(false);
            _currentActivePage = InformationPages[++_currentOpenPageIndex];
            _currentActivePage.SetActive(true);
            Dots[_currentOpenPageIndex].EnableDot();
            if (_currentOpenPageIndex == InformationPages.Count - 1)
            {
                RightArrow.interactable = false;
                LeftArrow.interactable = true;
            }
            else
            {
                LeftArrow.interactable = true;
                RightArrow.interactable = true;
            }
        }
    }

    public void ShowLastPage()
    {
        if (_currentOpenPageIndex + 1 > 0)
        {
            Dots[_currentOpenPageIndex].DisableDot();
            _currentActivePage.SetActive(false);
            _currentActivePage = InformationPages[--_currentOpenPageIndex];
            _currentActivePage.SetActive(true);
            Dots[_currentOpenPageIndex].EnableDot();
            if (_currentOpenPageIndex == 0)
            {
                LeftArrow.interactable = false;
                RightArrow.interactable = true;
            }
            else
            {
                LeftArrow.interactable = true;
                RightArrow.interactable = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
