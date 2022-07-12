using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordArrows : MonoBehaviour
{
    // Start is called before the first frame update
    List<GameObject> wordArrows;
    Dictionary<GameObject, ManateeImageObject> arrowImagePairs;
    int currentArrowIndex = 0;
    private void Awake()
    {
        arrowImagePairs = new Dictionary<GameObject, ManateeImageObject>();
        foreach(Transform arrow in transform)
        {
            wordArrows.Add(arrow.gameObject);
        }
    }


    void ConnectArrowToImage(ManateeImageObject obj)
    {
        arrowImagePairs.Add(wordArrows[currentArrowIndex], obj);
    }
}
