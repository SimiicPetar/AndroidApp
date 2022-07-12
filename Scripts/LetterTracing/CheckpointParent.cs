using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointParent : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 startPosition;
    public List<CheckpointLogic> checkpoints;

    private void Awake()
    {
        startPosition = transform.localPosition;
        checkpoints = new List<CheckpointLogic>();
        foreach (Transform child in transform)
        {
            checkpoints.Add(child.gameObject.GetComponent<CheckpointLogic>());
        }
    }

    public void ActivateChildColiders(bool activate) {
     
       foreach (Transform child in transform)
        {
         
                child.gameObject.GetComponent<Collider>().enabled = activate;
        }
    }

    public bool CheckIfLastCheckPoint(CheckpointLogic checkpoint)
    {
        if (checkpoint == checkpoints[checkpoints.Count - 1])
        {
            Debug.Log("checkpoint je poslednji u svom parentu");
            return true;
        }

        else
        {
            Debug.Log("checkpoint nije poslednji u svom parentu");
            return false;
        }
           
    }

    public bool CheckIfClickInBoundsOfActiveParent(Vector3 clickPos)
    {
        if (GetComponent<Collider>().bounds.Contains(new Vector3(clickPos.x, clickPos.y, transform.position.z)))
            return true;
        else return false;
    }
  
}
