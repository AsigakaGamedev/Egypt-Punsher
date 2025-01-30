using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Waypoint NextPoint;

    private WaypointsManager manager;

    private void Reset()
    {
        if (manager == null) 
            manager = FindAnyObjectByType<WaypointsManager>();

        if (!manager.Waypoints.Contains(this))
            manager.Waypoints.Add(this);
    }
}
