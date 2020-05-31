using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAlienShipAI : MonoBehaviour
{
    public Transform target;
    public float updateRate = 2f;
    public Path path;
    public float speed = 300f;
    public ForceMode2D forceMode;
    [HideInInspector]
    public bool pathIsEnded = false;
    //The max distance from AI to a waypoint for it to continue.
    public float wayPointDistance = 3f;
    //Caching
    private Seeker seeker;
    private Rigidbody2D rigidbody;
    private int currentWayPoint = 0;


    void Start()
    {
        seeker = GetComponent<Seeker>();
        rigidbody = GetComponent<Rigidbody2D>();

        if (target == null)
        {
            Debug.LogError("Target null?");
        }

        seeker.StartPath(transform.position, target.position, OnPathComplete);

        StartCoroutine(UpdatePath());
    }

    void OnPathComplete(Path p)
    {
        Debug.Log("We got a path, Path has error? " + p.error);
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            //TODO: Insert A PLAYER SEARCH HERE.
            //return null;
            yield break;
        }
        seeker.StartPath(transform.position, transform.position, OnPathComplete);
        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(UpdatePath());
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            //TODO: Insert A PLAYER SEARCH HERE.
            return;
        }

        //TODO Always look to the player.

        if (path == null)
        {
            return;
        }

        if (currentWayPoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
            {
                return;
            }
            Debug.Log("Path is ended.");
            pathIsEnded = true;
            return;
        }
        pathIsEnded = false;

        //Direction to the next waypoint.
        Vector3 direction = (path.vectorPath[currentWayPoint] - transform.position).normalized;
        direction *= speed * Time.deltaTime;


        //Move Alien to that direction.
        rigidbody.AddForce(direction, forceMode);

        if (Vector3.Distance(transform.position, path.vectorPath[currentWayPoint]) < wayPointDistance)
        {
            currentWayPoint++;
            return;
        }
    }

}
