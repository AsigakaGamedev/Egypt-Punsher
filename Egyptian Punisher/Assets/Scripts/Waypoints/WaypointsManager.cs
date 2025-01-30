using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointsManager : MonoBehaviour
{
    [SerializeField] private Color debugColor = Color.red;

    [Space]
    public List<Waypoint> Waypoints;

    private void OnEnable()
    {
        ServiceLocator.AddService(this);
    }

    private void OnDisable()
    {
        ServiceLocator.RemoveService(this);
    }

    private void OnDrawGizmos()
    {
        foreach (var waypoint in Waypoints)
        {
            if (waypoint.NextPoint)
            {
                Gizmos.color = debugColor;
                Gizmos.DrawLine(waypoint.transform.position, waypoint.NextPoint.transform.position);
            }
        }
    }

    public Waypoint GetNearestPoint(Vector3 from)
    {
        Waypoints.Sort((a, b) =>
        {
            float distanceToA = Vector3.Distance(a.transform.position, from);
            float distanceToB = Vector3.Distance(b.transform.position, from);
            return distanceToA.CompareTo(distanceToB);
        });

        return Waypoints[0];
    }
}
