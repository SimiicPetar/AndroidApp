using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarLogic : MonoBehaviour
{
    // Start is called before the first frame update
    List<ProgressBarNodeLogic> progressBarNodes;

    public  Color CorrectColor;
    public  Color WrongColor;

    static ProgressBarLogic _instance = null;
    public static ProgressBarLogic Instance { get { return _instance; } }
    public ProgressBarLalaVisualBehaviour lala;


    private void Awake()
    {

        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
        progressBarNodes = new List<ProgressBarNodeLogic>();
        foreach (Transform child in transform)
        {
            if(child.gameObject.activeSelf && child.gameObject.GetComponent<ProgressBarNodeLogic>() != null)
                progressBarNodes.Add(child.gameObject.GetComponent<ProgressBarNodeLogic>());
        }
            
    }

    private void Start()
    {
        lala = GetComponentInChildren<ProgressBarLalaVisualBehaviour>();
    }

    public void LalaAnswerReaction(bool correct)
    {
        if (correct)
            lala.ShowThumbsUp();
        else
            lala.ShowWrongAnswer();
    }

    public void ChangeNodeBackground(int trialNumber, bool correct)
    {
        //ovde promeniti logiku tj dodati novu za igre koje nemaju predvidjen broj podeoka
        if(GameObject.FindGameObjectWithTag("MiniGameManager") != null)
        {
            var numberOfTrials = GameObject.FindGameObjectWithTag("MiniGameManager").GetComponent<IGameManager>().NumberOfTrials;
            int numberOfPartsToActivate = (int)progressBarNodes.Count / numberOfTrials;
            int start = trialNumber * numberOfPartsToActivate;
            for (int i = start; i < start + numberOfPartsToActivate; i++)
            {
                progressBarNodes[i].SetBackground(correct);
            }
        }
        else
        {
            progressBarNodes[trialNumber].SetBackground(correct);
        }
        
    }

    public void ResetNodesBackground()
    {
        foreach (var item in progressBarNodes)
        {
            item.ResetBackground();
        }
    }
}
