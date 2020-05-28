using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrail : MonoBehaviour
{

    private int moveSpeed = 200;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right*Time.deltaTime * moveSpeed);
        Destroy(gameObject,1);
    }
}
