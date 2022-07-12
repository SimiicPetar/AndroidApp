using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<HowToPlayInfo> HowToPlayInfoList;
    public List<HowToPlayInfoDot> IndicatorDots;
    public TextMeshProUGUI HowToPlayText;
    public TextMeshProUGUI TitleOfHowToPlayText;
    public Image HowToPlayGameScreenshot;
    public Button RightArrow;
    public Button LeftArrow;
    int currentInfoIndex = 0;
    void Start()
    {
        InitializeHowToPlayPanel();
    }



    public static void OpenURL()
    {
        Application.OpenURL("www.uni.lu");
    }

    private void OnEnable()
    {
        InitializeHowToPlayPanel();
    }

    private void OnDisable()
    {
        InitializeHowToPlayPanel();
    }

    public void InitializeHowToPlayPanel()
    {
        LeftArrow.interactable = false;
        RightArrow.interactable = true;
        HowToPlayText.text = HowToPlayInfoList[0].HowToPlayText;
        HowToPlayText.text = HowToPlayText.text.Replace("\\n", "\n");
        TitleOfHowToPlayText.text = HowToPlayInfoList[0].TitleOfTheExplanation;
        HowToPlayGameScreenshot.sprite = HowToPlayInfoList[0].ScreenshotSprite;
        currentInfoIndex = 0;
        foreach (var dot  in IndicatorDots)
        {
            dot.DisableDot();
        }
        IndicatorDots[0].EnableDot();
    }

    public void NextGameInfo()
    {
        if(currentInfoIndex + 1 < HowToPlayInfoList.Count)
        {
            IndicatorDots[currentInfoIndex].DisableDot();
            IndicatorDots[currentInfoIndex + 1].EnableDot();
            HowToPlayText.text = HowToPlayInfoList[currentInfoIndex + 1].HowToPlayText;
            HowToPlayText.text = HowToPlayText.text.Replace("\\n", "\n");
            TitleOfHowToPlayText.text = HowToPlayInfoList[currentInfoIndex + 1].TitleOfTheExplanation;
            HowToPlayGameScreenshot.sprite = HowToPlayInfoList[currentInfoIndex + 1].ScreenshotSprite;
            currentInfoIndex++;
            if (currentInfoIndex == HowToPlayInfoList.Count - 1)
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

    public void LastGameInfo()
    {
        if (currentInfoIndex - 1 >= 0)
        {
            IndicatorDots[currentInfoIndex].DisableDot();
            IndicatorDots[currentInfoIndex - 1].EnableDot();
            HowToPlayText.text = HowToPlayInfoList[currentInfoIndex - 1].HowToPlayText;
            HowToPlayText.text = HowToPlayText.text.Replace("\\n", "\n");
            TitleOfHowToPlayText.text = HowToPlayInfoList[currentInfoIndex - 1].TitleOfTheExplanation;
            HowToPlayGameScreenshot.sprite = HowToPlayInfoList[currentInfoIndex - 1].ScreenshotSprite;
            currentInfoIndex--;

            if (currentInfoIndex == 0)
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
}
