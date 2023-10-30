using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class mission_script : MonoBehaviour
{
    static_data script;
    public TextMeshProUGUI name_box, dialog_box;
    public GameObject canvas,accept_button,reject_button;
    public bool on_mission = false;
    bool can_afford = false;

    public GameObject jungle_lady;

    string current_person;

    private void Start()
    {
        script = GameObject.FindGameObjectWithTag("data").GetComponent<static_data>();
        name_box.text = "";
        dialog_box.text = "";
    }

    private void Update()
    {
        if (can_afford == true && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("you won");
        }
    }

    public void get_mission(string name_of_person)
    {
        Debug.Log(name_of_person);
        canvas.SetActive(true);
        name_box.text = name_of_person;
        on_mission = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        current_person = name_of_person;

        if(name_of_person == "surgeon")
        {
            dialog_box.text = "u can get rid of this bad luck\nYou will need 100000 money for the surgery";

            if(script.money >= 100000)
            {
                dialog_box.text += "\n\n since you have the money we can continue (left click to continue)";
                can_afford = true;
            }
        }

        if(name_of_person == "unknown lady")
        {
            dialog_box.text = "I lost my purse in the woods please help me find it";
        }
    }

    public void accept_offer()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        canvas.SetActive(false);

        name_box.text = "";
        dialog_box.text = "";
        on_mission = false;

        if(current_person == "unknown lady")
        {
            //mission script here. Also put a time limit
        }
    }

    public void reject_offer()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        canvas.SetActive(false);

        name_box.text = "";
        dialog_box.text = "";
        on_mission = false;
    }

}
