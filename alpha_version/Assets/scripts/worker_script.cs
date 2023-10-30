using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class worker_script : MonoBehaviour
{
    public Transform sight;
    GameObject target;
    CharacterController slave;
    bool getting_cotton = false;

    public Transform[] points;
    int count = 0;

    static_data script;

    // Update is called once per frame

    private void Start()
    {
        slave = transform.parent.GetComponent<CharacterController>();
        script = GameObject.FindGameObjectWithTag("data").GetComponent<static_data>();
    }
    void Update()
    {
        if (script.paused == false)
        {
            if (getting_cotton && (target.transform.position - slave.transform.position).magnitude > 1.5f)
            {
                sight.LookAt(target.transform.parent.transform.position);
                slave.Move(sight.forward * Time.deltaTime * 9f + sight.up * -3 * Time.deltaTime);
                slave.transform.rotation = Quaternion.Euler(slave.transform.rotation.x, sight.transform.rotation.y, slave.transform.rotation.x);
            }

            if (target != null)
                if ((slave.transform.position - target.transform.position).magnitude < 2.5f)
                {
                    target.transform.parent.gameObject.SetActive(false);
                    getting_cotton = false;
                }
            if (target == null)
                getting_cotton = false;

            if (getting_cotton == false)
            {
                patrol();
            }
        }
    }

    //to get target
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "cotton" && getting_cotton == false)
        {
            target = other.gameObject;
            getting_cotton = true;

        }
    }

    void patrol()
    {
        sight.LookAt(points[count].position);
        slave.Move(sight.forward * Time.deltaTime * 9f);

        if ((sight.position - points[count].position).magnitude < 1)
            count++;

        if (count == points.Length)
            count = 0;


    }
}
