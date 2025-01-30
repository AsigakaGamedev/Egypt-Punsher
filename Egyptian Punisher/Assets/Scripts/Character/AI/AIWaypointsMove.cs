using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWaypointsMove : AIBehvaiour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float rotSpeed = 2;

    [Space]
    [SerializeField] private Waypoint targetPoint;

    private WaypointsManager waypointsManager;

    public override void OnInit(Character character)
    {
        base.OnInit(character);

        waypointsManager = ServiceLocator.GetService<WaypointsManager>();
    }

    public override void OnEnterLogic()
    {
        base.OnEnterLogic();

        targetPoint = waypointsManager.GetNearestPoint(transform.position);
    }

    public override void OnUpdateLogic()
    {
        base.OnUpdateLogic();

        if (targetPoint == null) return;

        if (Vector3.Distance(transform.position, targetPoint.transform.position) <= stoppingDistance)
        {
            targetPoint = targetPoint.NextPoint;
        }
        else
        {
            Vector3 dir = targetPoint.transform.position - transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotSpeed);
            character.MoveTo(dir, moveSpeed);
        }
    }
}
