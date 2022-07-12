using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLogic : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public UnitInfo unitInfo;

    GameManager gameManager;

    UIManager uiManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        uiManager = UIManager.Instance;
    }

    public void PassUnitToGameManager()
    {
        gameManager.SetCurrentUnit(unitInfo);
    }
}
