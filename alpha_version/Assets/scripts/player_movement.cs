using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player_movement : MonoBehaviour
{
    public CharacterController player;
    public GameObject visible_body;
    public GameObject bottom;
    public LayerMask ground;
    public LayerMask wall;
    public GameObject firstperson;
    public GameObject thirdperson;
    public GameObject thirdperson_front;
    public GameObject bullet;
    public GameObject left_check;
    public GameObject right_check;
    public GameObject gun;
    public GameObject hand;
    public GameObject melon;
    public GameObject kfc;
    public GameObject medicine;
    public GameObject meth;
    public Transform bulletpos;
    public Transform cave_entry;
    public Transform cave_exit;
    static_data script;
    mission_script mission_script;
    
    float horizontal,vertical,mousex=0,mousey = 0,count =0;
    float speed = 10f;
    float gravity = -10f;
    float jump_strength = 5;
    float camera_state;
    Vector3 jump,wall_jump;
    Vector3 direction;
    bool isgrounded = false;
    bool can_wall_jump = false;
    public bool in_cave = false;
    float[] inventory;//for holding kfc and watermelon and gun
    int item_count = 0;

    void Start()
    {
        script = GameObject.FindGameObjectWithTag("data").GetComponent<static_data>();
        mission_script = GameObject.FindGameObjectWithTag("mission data").GetComponent<mission_script>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        firstperson.SetActive(true);
        thirdperson.SetActive(false);
        thirdperson_front.SetActive(false);
        camera_state = 2;
        inventory = new float[] {1,1,0,0,0,0};
        player.enabled = false;
        transform.position = new Vector3(script.location[0], script.location[1], script.location[2]);
        player.enabled = true;
    }
    void Update()
    {

        if (script.paused == false && mission_script.on_mission == false)
        {
            groundcheck();
            movement();
            screen_center();
            checkwall();


            //inventory activation and removal
            if (Input.mouseScrollDelta == new Vector2(0, 1))
            {
                inventory[2] = script.melon;
                inventory[3] = script.kfc;
                inventory[4] = script.medicine;
                inventory[5] = script.meth;

                change_item(1);
            }

            if (Input.mouseScrollDelta == new Vector2(0, -1))
            {
                inventory[2] = script.melon;
                inventory[3] = script.kfc;
                inventory[4] = script.medicine;
                inventory[5] = script.meth;

                change_item(-1);
            }

            //if melon = 0 or kfc = 0 but is active
            if (melon.activeSelf == true && script.melon == 0 || kfc.activeSelf == true && script.kfc == 0 || medicine.activeSelf == true && script.medicine == 0 || meth.activeSelf == true && script.meth == 0)
            {
                item_count = 0;
            }

            if (Input.GetKeyDown(KeyCode.V))
                changecamera();

            if (Input.GetKeyDown(KeyCode.Mouse0) && gun.activeSelf == true)
                shoot();

            if (Input.GetButtonDown("Jump") && isgrounded)
            {
                jump.y = Mathf.Sqrt(jump_strength * -1 * gravity);
                isgrounded = false;
            }

            else if (Input.GetButtonDown("Jump") && can_wall_jump)
            {
                wall_jump = Mathf.Sqrt(3 * -1 * gravity) * visible_body.transform.up;
                can_wall_jump = false;
                count = 0;
            }

            if (count < 1.8f)
                count += Time.deltaTime;
            else
            {
                count = 0;
                wall_jump = visible_body.transform.up * -0.1f;
            }


            if (isgrounded)
            {
                jump = visible_body.transform.up * -0.1f;
            }

            jump.y += gravity * Time.deltaTime;

            player.Move(direction);
            player.Move(jump * Time.deltaTime);
            if (isgrounded == false)
                player.Move(wall_jump * Time.deltaTime);

        }
    }
    void changecamera()
    {
        if (in_cave == false)
        {
            if (camera_state == 3)
            {
                firstperson.SetActive(false);
                thirdperson.SetActive(false);
                thirdperson_front.SetActive(true);
                camera_state = 1;
                return;
            }

            if (camera_state == 2)
            {
                firstperson.SetActive(false);
                thirdperson.SetActive(true);
                thirdperson_front.SetActive(false);
                camera_state++;
            }

            if (camera_state == 1)
            {
                firstperson.SetActive(true);
                thirdperson.SetActive(false);
                thirdperson_front.SetActive(false);
                camera_state++;
            }
        }
    }

    void groundcheck()
    {
        if (Physics.CheckSphere(bottom.transform.position, 0.11f, ground) == true || Physics.CheckSphere(left_check.transform.position, 0.1f, ground) == true || Physics.CheckSphere(right_check.transform.position, 0.1f, ground) == true)
        {
            isgrounded = true;
        }
        else
        {
            isgrounded = false;
        }
    }

    void shoot()
    {
        if (script.ammo > 0)
        {
            Instantiate(bullet, bulletpos.position + bulletpos.forward * 0.9f, bulletpos.rotation);
            script.ammo--;
        }

    }

    void movement()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        mousex += Input.GetAxis("Mouse X") * 5f;

        direction = transform.right * horizontal * speed * Time.deltaTime + transform.forward * vertical * speed * Time.deltaTime;

        player.transform.rotation = Quaternion.Euler(0, mousex, 0);

    }

    void screen_center()
    {

        mousey -=Input.GetAxis("Mouse Y") * 5f;
        mousey = Mathf.Clamp(mousey, -75, 45);

        bulletpos.rotation = Quaternion.Euler(mousey , mousex, 0);

    }

    void checkwall()
    {
        if(Physics.CheckSphere(left_check.transform.position,0.2f,wall)==true && isgrounded == false)
        {
            visible_body.transform.localRotation = Quaternion.Euler(0,0,-30);
            can_wall_jump = true;
        }

        else if (Physics.CheckSphere(right_check.transform.position, 0.2f, wall) == true && isgrounded == false)
        {
            visible_body.transform.localRotation = Quaternion.Euler(0, 0, 30);
            can_wall_jump = true;
        }

        else
        {
            visible_body.transform.localRotation = Quaternion.Euler(0, 0, 0);
            can_wall_jump = false;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit collision)
    {

        if (collision.gameObject.tag == "portol")
        {
            player.enabled = false;
            gameObject.transform.position = cave_entry.transform.position - new Vector3(0,1,0);
            player.enabled = true;
            in_cave = true;
            script.sun.SetActive(false);

            firstperson.SetActive(true);
            thirdperson.SetActive(false);
            thirdperson_front.SetActive(false);
        }

        if(collision.gameObject.tag == "back")
        {
            player.enabled = false;
            gameObject.transform.position = cave_exit.transform.position;
            player.enabled = true;
            in_cave = false;
            script.sun.SetActive(true);

            firstperson.SetActive(true);
            thirdperson.SetActive(false);
            thirdperson_front.SetActive(false);

        }

        if (collision.gameObject.tag == "weapon")
        {
            Debug.Log("player hit");
            script.health -= 3;
            collision.gameObject.GetComponent<Collider>().enabled = false;
        }
    }

    void change_item(int flag)
    {
        hand.SetActive(false);
        gun.SetActive(false);
        melon.SetActive(false);
        kfc.SetActive(false);
        meth.SetActive(false);
        medicine.SetActive(false);

        while(true)
        {
            if(flag == 1)
            item_count++;

            if(flag == -1)
            item_count--;

            if (item_count < 0)
                item_count = inventory.Length - 1;

            if (item_count >= inventory.Length)
                item_count = 0;

            if (inventory[item_count] > 0)
                break;

            

        }

        switch (item_count) 
        {
            case 0:
                hand.SetActive(true);
                break; 
            
            case 1:
                gun.SetActive(true);
                break; 
            
            case 2:
                melon.SetActive(true);
                break;

            case 3:
                kfc.SetActive(true);
                break;

            case 4:
                medicine.SetActive(true);
                break;

            case 5:
                meth.SetActive(true);
                break;
        
        }

    }

}
