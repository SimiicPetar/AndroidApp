using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderStateControll : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnEnable()
    {
        UIMapManager.OnWindowOpened += DisableCollider;
        UIMapManager.OnWindowClosed += EnableCollider;
    }

    private void OnDisable()
    {
        UIMapManager.OnWindowOpened -= DisableCollider;
        UIMapManager.OnWindowClosed -= EnableCollider;
    }

    void Start()
    {
      
    }
     
    void DisableCollider(UIWindow win)
    {
        GetComponent<Collider>().enabled = false;
    }

    void EnableCollider(UIWindow win)
    {
        StartCoroutine(SmallWait());      
    }

    IEnumerator SmallWait()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<Collider>().enabled = true;
    }

}
