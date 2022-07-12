using Patterns.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NextUnitIndicatorController : Singleton<NextUnitIndicatorController>
{
    // Start is called before the first frame update

    Task ShowPointerTask;

    GameObject currentActiveHandPointer;

    public Waypoint waypointToGO;
    private void Start()
    {
        
    }

    public void InitiateFiveSecondsWaitForPointer(GameObject handPointer, Waypoint wp)
    {
        if (!(UnitStatisticsBase.Instance.GetMostCurrentHoleId() == 4)  && !UIMapManager.Instance.UIOpen)
        {
            waypointToGO = wp;
            currentActiveHandPointer = handPointer;
            ShowPointerTask = new Task(FiveSecondsWaitForPointer(handPointer));
        }
       
    }

    IEnumerator FiveSecondsWaitForPointer(GameObject pointerToActivate)
    {
        if (UnitStatisticsBase.Instance.GetMostCurrentHoleId() == 4)
            yield break;
        if(currentActiveHandPointer == null)
        {
            currentActiveHandPointer = MapAvatarController.Instance.GetHandPointer();
        }
        yield return new WaitForSeconds(5f);
        yield return new WaitUntil(() => !UIMapManager.Instance.UIOpen);
            currentActiveHandPointer.SetActive(true);
    }

    public void ClickedOnUnitWithHandPointer()
    {
        currentActiveHandPointer.SetActive(false);
        ShowPointerTask.Stop();
    }

    public void CorrectUnitClosed()
    {
        ShowPointerTask = new Task(FiveSecondsWaitForPointer(currentActiveHandPointer));
    }
}
