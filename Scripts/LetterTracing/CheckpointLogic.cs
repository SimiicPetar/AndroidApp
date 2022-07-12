using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheckpointLogic : MonoBehaviour

{
    public bool firstFromParent;
    public bool Starter;
    public bool canStartFromThis = false;
    public bool MouseUpCheckPoint;

    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("istrigerovan je :" + gameObject.name + " istrigerovao ga je : " + other.name);
       // LetterTracingManager.Instance.SetNextCheckPoint(this);
    }

    public void CheckpointReached()
    {
        if (!Starter) { }
        else
        {
            canStartFromThis = true;
        }
    }

    public void ResetCheckPoint()
    {
       // gameObject.GetComponent<Collider>().isTrigger = true;
    }

    public void UnmaskParent(Transform lastParent)
    {
       // transform.parent.position = new Vector3(lastParent.transform.position.x, lastParent.transform.position.y, 0f);
    }
}
