using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderOfGameItemsHelp : MonoBehaviour
{
    public static int[][] RandomItemOrders = new int[][] { new int []{1, 3, 2, 1, 2, 1, 3, 2, 3, 1, 2, 3},
                                                           new int []{3, 2, 1, 3, 2, 3, 1, 2, 1, 2, 3, 1},
                                                           new int []{1, 3, 2, 1, 2, 1, 3, 2, 1, 3, 2, 3}};
    public static int[] GetRandomOrderArray()
    {
        int randomIndex = Random.Range(0, RandomItemOrders.Length);
        return RandomItemOrders[randomIndex];
    }

    
}
