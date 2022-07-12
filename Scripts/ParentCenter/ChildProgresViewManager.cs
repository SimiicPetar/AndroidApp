using Patterns.Singleton;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChildProgresViewManager : Singleton<ChildProgresViewManager>
{
    // Start is called before the first frame update

    public List<LetterSuccessBar> LetterSuccessBarList;
    public List<HowToPlayInfoDot> IndicatorDots;
    UnitButtonLogic activeUnitButtonTab;
    public Button RightArrow;
    public Button LeftArrow;
    int currentOpenUnitID = 0;
    

    void Start()
    {
        IndicatorDots[currentOpenUnitID].EnableDot();
        InitialViewOpen();
    }

    private void OnEnable()
    {
       // UnitButtonLogic.onUnitTabClicked += OnUnitTabButtonClicked;
    }

    private void OnDisable()
    {
       // UnitButtonLogic.onUnitTabClicked -= OnUnitTabButtonClicked;
    }

    public void InitialViewOpen()
    {
        LeftArrow.interactable = false;
        RightArrow.interactable = UnitStatisticsBase.Instance.GetMostCurrentUnitIDAsInt() > 1;
        OnUnitTabButtonClicked("afo");
    }

    public void OpenNextUnitStatistic()
    {if(currentOpenUnitID + 1 < UnitManager.Instance.AllUnitsList.Count)
        {
            if (UnitStatisticsBase.Instance.CheckIfUnitIsUnlocked(UnitManager.Instance.AllUnitsList.ElementAt(currentOpenUnitID + 1).unitId))
            {
                IndicatorDots[currentOpenUnitID].DisableDot();
                currentOpenUnitID++;
                if (currentOpenUnitID == IndicatorDots.Count - 1)
                {
                    RightArrow.interactable = false;
                    LeftArrow.interactable = true;
                }
                else
                {
                    LeftArrow.interactable = true;
                    RightArrow.interactable = true;
                }
                IndicatorDots[currentOpenUnitID].EnableDot();
                OnUnitTabButtonClicked(UnitManager.Instance.AllUnitsList[currentOpenUnitID].unitId);
            }
        }else
            RightArrow.interactable = false;

    }

    public void OpenLastUnitStatistic()
    {
        if(currentOpenUnitID - 1 >= 0)
        {
            IndicatorDots[currentOpenUnitID].DisableDot();
            currentOpenUnitID--;
            IndicatorDots[currentOpenUnitID].EnableDot();
            OnUnitTabButtonClicked(UnitManager.Instance.AllUnitsList[currentOpenUnitID].unitId);
            if (currentOpenUnitID == 0)
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

    public void OnUnitTabButtonClicked(string UnitID)
    {
        int i = 0;
        
        char[] charList = UnitID.ToCharArray();
        foreach(var bar in LetterSuccessBarList)
        {
            float fillAmount = UnitStatisticsBase.Instance.GetLetterSuccessRate(charList[i], UnitID);
            if (fillAmount == -1f)
                return;
            bar.FillBar(fillAmount, charList[i]);
            i++;
        }

        
    }
}
