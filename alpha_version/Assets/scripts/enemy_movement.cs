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
    public Transform cotton_exit;

    public GameObject baton;
    public Collider baton_collider;
    public Transform handpos;

    Vector3 direction,patrolling_dir;
    float vision_radius = 50f,baton_radius = 5f;
    bool moving_down = true, injungle = false, ischasing = false,infield = false,isdead = false;
    int way_count = 0,fire_once = 0;
    float speed = 9.7f, death_count = 0;

    static_data script;
    int temp = open_screen_ui.flag;
    float[] location = new float[3];

    private void Start()
    {
        script = GameObject.FindGameObjectWithTag("data").GetComponent<static_data>();

        if (temp != 0)
        {
            load();
            police.enabled = false;
            gameObject.transform.position = new Vector3(location[0], location[1], location[2]);
            police.enabled = true;
        }
    }

    void Update()
    {
        if (script.paused == true && fire_once == 0)
        {
            fire_once = 1;
            save_place();
        }

        if (script.paused == false)
        {
            fire_once = 0;

            if (transform.GetChild(0).gameObject.activeSelf == false)
            {
                isdead = true;
                police.enabled = false;
                transform.position = way_point[0].position + new Vector3(0, 2, 0);
            }
            if (death_count < 10 && isdead)
            {
                death_count += Time.deltaTime;
            }
            if (death_count >= 10 && isdead)
            {
                death_count = 0;
                isdead = false;
                transform.GetChild(0).gameObject.SetActive(true);
                police.enabled = true;
            }


            if (isdead == false)
            {

                looking_at.LookAt(target);

                direction = target.position - looking_at.position;
                patrolling_dir = way_point[way_count].position - looking_at.position;

                police.Move(new Vector3(0, -3f, 0));

                checkplayer();

                if (!injungle && !ischasing && !infield)
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
                if (baton_collider.enabled == false && baton.transform.eulerAngles.x < 15)
                    baton_collider.enabled = true;

                if ((looking_at.position - jungle_exit.position).magnitude < 0.5f) //just exiting jungle
                    injungle = false;

                if ((looking_at.position - cotton_exit.position).magnitude < 0.5f) //just exiting field
                    infield = false;

                // to exit jungle
                if (injungle && !ischasing)
                {
                    police.transform.rotation = Quaternion.LookRotation(jungle_exit.position - looking_at.position);
                    police.Move(police.transform.forward * speed * Time.deltaTime);
                }

                if (infield && !ischasing)
                {
                    police.transform.rotation = Quaternion.LookRotation(cotton_exit.position - looking_at.position);
                    police.Move(police.transform.forward * speed * Time.deltaTime);
                }
            }
        }    
    }

    void checkplayer()
    {
        if (direction.magnitude <= vision_radius && direction.magnitude >=1.5f)
        {
            ischasing = true;
            police.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            if(direction.magnitude > 1.5f)
                police.Move(police.transform.forward * speed * Time.deltaTime);

        }
        else
            ischasing = false;
    }

    void movebaton()
    {
        if (moving_down)
        {
            baton.transform.RotateAround(handpos.position, handpos.right, 80 * Time.deltaTime);
            if (baton.transform.rotation.eulerAngles.x > 60)
                moving_down=false;
        }

        if (!moving_down)
        {
            baton.transform.RotateAround(handpos.position, handpos.right, -80 * Time.deltaTime);
            if (baton.transform.rotation.eulerAngles.x < 10)
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
            police.Move(police.transform.forward * speed/1.5f * Time.deltaTime);
        }

        if(patrolling_dir.magnitude < 0.5f)
            way_count++;

        if(way_count == way_point.Length)
            way_count= 0;
    }

    void save_place()
    {
        //for their position
        PlayerPrefs.SetFloat(gameObject + "x", police.transform.position.x);
        PlayerPrefs.SetFloat(gameObject + "y", police.transform.position.y);
        PlayerPrefs.SetFloat(gameObject + "z", police.transform.position.z);

        PlayerPrefs.SetInt(gameObject + "way_count" , way_count);
    }

    void load()
    {
        location[0] = PlayerPrefs.GetFloat(gameObject + "x");
        location[1] = PlayerPrefs.GetFloat(gameObject + "y");
        location[2] = PlayerPrefs.GetFloat(gameObject + "z");
        way_count = PlayerPrefs.GetInt(gameObject + "way_count");
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.tag == "jungle" && direction.magnitude > vision_radius)
        {
            injungle = true;
            way_count = 0;
        }

        if (hit.collider.tag == "cotton_field" && direction.magnitude > vision_radius)
        {
            infield = true;
        }
    }

}
