using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_movement : MonoBehaviour
{
    public GameObject refrence;
    public GameObject gun;
    public GameObject hand;
    public GameObject melon;
    public GameObject kfc;
    public GameObject medicine;
    public GameObject meth;
    static_data script;
    mission_script mission_script;

    bool eating = false;
    float count = 0;
    void Start()
    {
        script = GameObject.FindGameObjectWithTag("data").GetComponent<static_data>();
        mission_script = GameObject.FindGameObjectWithTag("mission data").GetComponent<mission_script>();
    }

    // Update is called once per frame
    void Update()
    {
        if (script.paused == false && mission_script.on_mission == false)
        {
            gameObject.transform.position = refrence.transform.position + (refrence.transform.forward / 3);
            transform.rotation = refrence.transform.rotation;

            gun.transform.position = transform.position + (refrence.transform.right / 4) - (refrence.transform.up / 3) + (refrence.transform.forward / 3);
            gun.transform.rotation = transform.rotation;

            //just holding item
            if (eating == false)
            {
                hand.transform.position = transform.position + (transform.forward * 0.4f);
                melon.transform.position = transform.position + (transform.forward * 0.7f + transform.right * 0.7f - transform.up * 0.5f);
                kfc.transform.position = transform.position + (transform.forward * 0.7f + transform.right * 0.7f - transform.up * 0.5f);
                medicine.transform.position = transform.position + (transform.forward * 0.7f + transform.right * 0.7f - transform.up * 0.5f);
                meth.transform.position = transform.position + (transform.forward * 0.7f + transform.right * 0.7f - transform.up * 0.5f);
            }

            //if told to eat and not eating while
            if (melon.activeSelf && Input.GetKeyDown(KeyCode.Mouse1) && eating == false || kfc.activeSelf && Input.GetKeyDown(KeyCode.Mouse1) && eating == false || medicine.activeSelf && Input.GetKeyDown(KeyCode.Mouse1) && eating == false || meth.activeSelf && Input.GetKeyDown(KeyCode.Mouse1) && eating == false)
            {
                count = 0;
                eating = true;
                melon.transform.position = transform.position + transform.forward + transform.right * -0.2f + transform.up * -0.2f;
                kfc.transform.position = transform.position + transform.forward + transform.right * -0.08f + transform.up * -0.5f;
                medicine.transform.position = transform.position + transform.forward + transform.right * -0.1f + transform.up * -0.5f;
                meth.transform.position = transform.position + transform.forward * 0.6f + transform.right * -0.1f + transform.up * -0.5f;
            }

            if (eating)
                count += Time.deltaTime;

            if (count >= 0.5f)
            {
                eating = false;
                count = 0;
                melon.transform.position = transform.position + transform.forward * -1f + transform.right * 0.2f + transform.up * 0.2f;
                kfc.transform.position = transform.position + transform.forward * -1f + transform.right * 0.08f + transform.up * 0.5f;
                medicine.transform.position = transform.position + transform.forward * -1f + transform.right * 0.1f + transform.up * 0.5f;
                meth.transform.position = transform.position + transform.forward * -0.6f + transform.right * 0.1f + transform.up * 0.5f;

                if (melon.activeSelf)
                {
                    script.melon -= 1;
                    script.hunger += 10;
                }

                if (kfc.activeSelf)
                {
                    script.kfc -= 1;
                    script.hunger += 50;
                }

                if (medicine.activeSelf)
                {
                    script.medicine -= 1;
                    script.health += 10;
                }

                if (meth.activeSelf)
                {
                    script.meth -= 1;
                    script.health += 50;
                }

                if (script.hunger > 100)
                    script.hunger = 100;

                if (script.health > 100)
                    script.health = 100;

                if (melon.activeSelf == true && script.melon == 0 || kfc.activeSelf == true && script.kfc == 0 || medicine.activeSelf == true && script.medicine == 0 || meth.activeSelf == true && script.meth == 0)
                {
                    melon.SetActive(false);
                    kfc.SetActive(false);
                    meth.SetActive(false);
                    medicine.SetActive(false);
                    hand.SetActive(true);
                }
            }
        }

    }
}
