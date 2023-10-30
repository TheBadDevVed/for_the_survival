using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cotton_collection : MonoBehaviour
{
    static_data script;
    mission_script mission_script;
    public GameObject status;
    public GameObject buy_menu;
    public GameObject dealer_menu;
    private void Start()
    {
        script = GameObject.FindGameObjectWithTag("data").GetComponent<static_data>();
        mission_script = GameObject.FindGameObjectWithTag("mission data").GetComponent<mission_script>();

    }
    private void OnTriggerStay(Collider other)
    {
        if (script.paused == false)
        {
            if (other.gameObject.tag == "cotton" && Input.GetKey(KeyCode.Mouse0))
            {
                other.gameObject.transform.parent.gameObject.SetActive(false);
                script.cotton++;
            }

            if (other.gameObject.tag == "coal" && Input.GetKey(KeyCode.Mouse0))
            {
                other.gameObject.SetActive(false);
                script.coal += Random.Range(1,5);
            }

            if (other.gameObject.tag == "sellsman" && Input.GetKey(KeyCode.Mouse0))
            {
                status.SetActive(false);
                buy_menu.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            if (other.gameObject.tag == "dealer" && Input.GetKey(KeyCode.Mouse0))
            {
                status.SetActive(false);
                dealer_menu.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            if (other.gameObject.tag == "surgeon" && Input.GetKey(KeyCode.Mouse0))
            {
                status.SetActive(false);
                buy_menu.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            if (other.gameObject.tag == "mission" && Input.GetKey(KeyCode.Mouse0))
            {
                mission_script.get_mission(other.gameObject.name);
            }
        }
        }

        public void back_from_sell()
        {
            status.SetActive(true);
            buy_menu.SetActive(false);
            dealer_menu.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

        }
    
}
