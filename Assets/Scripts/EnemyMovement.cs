using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private Transform[] waypoints;
    [SerializeField]
    private float moveSpeed = 2f;
    private int waypointIndex = 0;
    public bool go = false;
    
    private Animator anim;
    
    void Start()
    {
         anim=GameObject.Find("Target").GetComponent<Animator>();
         transform.position = waypoints[waypointIndex].transform.position;
    }

    
    void Update()
    {
        Move();
        //transform.position = Vector3.MoveTowards(transform.position, waypoint2.position, speed * Time.deltaTime);
        
    }
    private void Move()
    {
        // If Enemy didn't reach last waypoint it can move
        // If enemy reached last waypoint then it stops
        if (waypointIndex <= waypoints.Length - 1 && go == false)
        {

            // Move Enemy from current waypoint to the next one
            // using MoveTowards method
            transform.position = Vector3.MoveTowards(transform.position,
                waypoints[waypointIndex].transform.position,
                moveSpeed * Time.deltaTime);

            // If Enemy reaches position of waypoint he walked towards
            // then waypointIndex is increased by 1
            // and Enemy starts to walk to the next waypoint
            if (transform.position == waypoints[waypointIndex].transform.position)
            {
                waypointIndex += 1;
                // if (transform.position == waypoints[waypoints.Length-1].transform.position)
                // {
                //     _animator.SetBool("ıdle",false); 
                //     
                // }
            }
        }
    }
}
