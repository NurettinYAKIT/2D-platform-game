using UnityEngine;
using System.Collections;
using Pathfinding;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAlienShipAI : MonoBehaviour
{
    //what to chase
    public Transform target;
    public float updateRate = 2f;
    public Path path;
    public float speed = 300f;
    public float playerSearchInterval = 0.5f;
    public ForceMode2D fMode;
    [HideInInspector]
    public bool pathIsEnded = false;
    //The max distance from AI to a waypoint for it to continue.
    public float nextWaypointDistance = 3f;
    //Caching
    private Seeker seeker;
    private Rigidbody2D rb;
    private int currentWayPoint = 0;
    private bool searchingForPlayer = false;



    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchPlayer());
            }
            return;
        }

        seeker.StartPath(transform.position, target.position, OnPathComplete);

        StartCoroutine(UpdatePath());
    }

    private IEnumerator SearchPlayer()
    {
        GameObject result = GameObject.FindGameObjectWithTag("Player");
        if (result == null)
        {
            yield return new WaitForSeconds(playerSearchInterval);
            StartCoroutine(SearchPlayer());
        }
        else
        {
            target = result.transform;
            searchingForPlayer = false;
            StartCoroutine(UpdatePath());
            yield return searchingForPlayer;
        }

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
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchPlayer());
            }
            yield return searchingForPlayer;
        }
        else
        {
            seeker.StartPath(transform.position, target.position, OnPathComplete);
            yield return new WaitForSeconds(1f / updateRate);
            StartCoroutine(UpdatePath());
        }

    }

    void FixedUpdate()
    {

        // Vector2 force = -(transform.position-target.position).normalized;
        // float velocity = speed*Time.deltaTime;

        // Debug.Log("Force" + force);
        // Debug.Log("velocity" + velocity);
        // rigidbody.AddForce(force*velocity,forceMode);


        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchPlayer());
            }
            return;
        }

        if (path == null)
        {
            Debug.Log("Path is null");
            return;
        }

        if (currentWayPoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
            {
                return;
            }
            pathIsEnded = true;
            return;
        }
        pathIsEnded = false;

        //Direction to the next waypoint.
        Vector3 direction = (path.vectorPath[currentWayPoint] - transform.position).normalized;
        direction *= speed * Time.fixedDeltaTime;

        // Debug.Log(""+ transform.name+" Enemy moving, direction : " + direction + " fMode : " + fMode +" path.vectorPath[currentWayPoint] : "+ path.vectorPath[currentWayPoint] + " transform.position : "+transform.position+ " speed : "+speed);
        //Move Alien to that direction.
        rb.AddForce(direction, fMode);

        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWayPoint]);
        if (dist < nextWaypointDistance)
        {
            currentWayPoint++;
            return;
        }
    }

}
