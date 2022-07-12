using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Waypoint : MonoBehaviour 
{
    public int WayPointIndex;

    public abstract bool CheckIfWalkable();

    public GameObject PointToWalkTo;

}
