using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemy_movement : MonoBehaviour
{
    public Transform target;
    public CharacterController police;
    public Transform looking_at;
    public Transform[] way_point;
    public Transform jungle_exit;

    public GameObject baton;
    public Collider baton_collider;
    public Transform handpos;

    Vector3 direction,patrolling_dir;
    float vision_radius = 50f,baton_radius = 5f;
    bool moving_down = true, injungle = false, ischasing = false;
    int way_count = 0;
    float speed = 9.8f;

    void Update()
    {
        looking_at.LookAt(target);

        direction = target.position - looking_at.position;
        patrolling_dir = way_point[way_count].position - looking_at.position;

        police.Move(new Vector3(0,-3f,0));

        checkplayer();

        if(!injungle && !ischasing)
        patrol();

        //when close enough start baton
        if (direction.magnitude <= baton_radius)
        {
            baton.SetActive(true);
            movebaton();
        }

        //if away close baton
        if (direction.magnitude > baton_radius)
            baton.SetActive(false);

        //after taking baton up
        if(baton_collider.enabled == false && baton.transform.eulerAngles.x < 15)
            baton_collider.enabled = true;

        if((looking_at.position - jungle_exit.position).magnitude < 0.5f) //just exiting jungle
            injungle = false;

        // to exit jungle
        if (injungle && !ischasing) 
        {
            police.transform.rotation = Quaternion.LookRotation(jungle_exit.position - looking_at.position);
            police.Move(police.transform.forward * speed * Time.deltaTime);
        }

    }

    void checkplayer()
    {
        if (direction.magnitude <= vision_radius)
        {
            ischasing = true;
            police.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            if(direction.magnitude > 1.5f)
                police.Move(police.transform.forward * speed * Time.deltaTime);

        }
    }

    void movebaton()
    {
        if (moving_down)
        {
            baton.transform.RotateAround(handpos.position, handpos.right, 90 * Time.deltaTime);
            if (baton.transform.rotation.eulerAngles.x > 60)
                moving_down=false;
        }

        if (!moving_down)
        {
            baton.transform.RotateAround(handpos.position, handpos.right, -90 * Time.deltaTime);
            if (baton.transform.rotation.eulerAngles.x < 2)
                moving_down = true;
        }

        if (baton.transform.rotation.eulerAngles.x < 0 || baton.transform.rotation.eulerAngles.x > 90)
            baton.transform.rotation = Quaternion.Euler(0,0,0);
    }

    void patrol()
    {
        if (direction.magnitude > vision_radius)
        {
            police.transform.rotation = Quaternion.LookRotation(new Vector3(patrolling_dir.x, 0, patrolling_dir.z));
            police.Move(police.transform.forward * speed/2 * Time.deltaTime);
        }

        if(patrolling_dir.magnitude < 0.5f)
            way_count++;

        if(way_count == way_point.Length)
            way_count= 0;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.tag == "jungle" && direction.magnitude > vision_radius)
        {
            injungle = true;
        }
    }

}
