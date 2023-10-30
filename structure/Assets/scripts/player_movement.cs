using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    public Transform bulletpos;
    static_data script;
    
    float horizontal,vertical,mousex=0,mousey = 0,count =0;
    float speed = 10f;
    float gravity = -10f;
    float jump_strength = 5;
    float camera_state;
    Vector3 jump,wall_jump;
    Vector3 direction;
    Transform shooting;
    bool isgrounded = false;
    bool can_wall_jump = false;
    bool is_armed = false;

    void Start()
    {
        script = GameObject.FindGameObjectWithTag("data").GetComponent<static_data>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        firstperson.SetActive(true);
        thirdperson.SetActive(false);
        thirdperson_front.SetActive(false);
        camera_state = 2;
    }
    void Update()
    {
        groundcheck();
        movement();
        screen_center();
        checkwall();

        //gun activation and removal
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (is_armed)
            {
                is_armed = false;
                gun.SetActive(false);
                hand.SetActive(true);
            }

            else
            {
                is_armed = true;
                gun.SetActive(true);
                hand.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.V))
            changecamera();

        if (Input.GetKeyDown(KeyCode.Mouse0) && is_armed)
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
        

        if(isgrounded)
        {
            jump = visible_body.transform.up * -0.1f;
        }
        
        jump.y += gravity * Time.deltaTime ;

        player.Move(direction );
        player.Move(jump * Time.deltaTime);
        if(isgrounded == false)
        player.Move(wall_jump * Time.deltaTime);

        
    }
    void changecamera()
    {
        if (camera_state == 3)
        {
            firstperson.SetActive(false);
            thirdperson.SetActive(false);
            thirdperson_front.SetActive(true);
            camera_state = 1;
            return;
        }

        if (camera_state == 2 )
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
        if (collision.gameObject.tag == "weapon")
        {
            Collider baton = collision.gameObject.GetComponent<Collider>();
            baton.enabled = false;
            Debug.Log("player hit");
        }
    }


}
