using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildReportPanel : MonoBehaviour
{
    private void OnEnable()
    {
        ChildProgresViewManager.Instance.InitialViewOpen();
    }
}
