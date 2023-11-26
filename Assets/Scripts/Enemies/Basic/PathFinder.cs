using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class PathFinder
{

    private Path path;
    private Seeker seeker;
    private Vector3 direction;

    public float nextWaypointDistance = 0.5f;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private Enemy enemy;

    Coroutine updatePath = null;

    public PathFinder(Enemy e, Seeker seeker){
        this.seeker = seeker;
        this.enemy = e;

        updatePath = enemy.StartCoroutine(UpdatePath());
    }

    public void Update(){
        if (path == null)
            return;
        if (currentWaypoint >= path.vectorPath.Count){
            reachedEndOfPath = true;
            return;
        }
        else{
            reachedEndOfPath = false;
        }

        direction = (path.vectorPath[currentWaypoint] - enemy.transform.position).normalized;
        if (Vector2.Distance(enemy.transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance){
            currentWaypoint++;
        }
    }

    IEnumerator UpdatePath(){
        while (true){
            if(path == null || (path != null && Vector3.Distance(enemy.transform.position, enemy.GetTargetPosition()) > nextWaypointDistance)){
                seeker.StartPath(enemy.transform.position, enemy.GetTargetPosition(), OnPathComplete);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public Vector2 GetDirection() => this.direction;

    void OnPathComplete(Path p)
    {
        if (!p.error){
            path = p;
            currentWaypoint = 0;
        }
    }
}