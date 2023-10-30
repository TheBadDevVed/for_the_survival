using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worker_script : MonoBehaviour
{
    public Transform sight;
    GameObject target;
    CharacterController slave;
    bool getting_cotton = false;

    public Transform[] points;
    int count = 0;

    // Update is called once per frame

    private void Start()
    {
        slave = transform.parent.GetComponent<CharacterController>();
    }
    void Update()
    {
        
        if (getting_cotton && (target.transform.position-slave.transform.position).magnitude >1.5f)
        {
            sight.transform.parent.gameObject.transform.LookAt(target.transform.position);
            slave.Move(sight.forward * Time.deltaTime * 9f + sight.up * -3 * Time.deltaTime);
            slave.transform.rotation = Quaternion.Euler(slave.transform.rotation.x,sight.transform.rotation.y, slave.transform.rotation.x);
        }

        if (target != null)
            if ((target.transform.position - slave.transform.position).magnitude < 1.5f)
            {
                target.transform.parent.gameObject.SetActive(false);
                getting_cotton=false;
            }
        
        if(getting_cotton == false)
        {
            patrol();
        }
    }

    //to get target
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "cotton" && getting_cotton == false)
        {
            target = other.gameObject;
            target.transform.position = new Vector3(target.transform.position.x, 1, target.transform.position.z);
            getting_cotton = true;

        }
    }

    void patrol()
    {
        sight.transform.parent.gameObject.transform.LookAt(points[count].position);
        slave.Move(sight.forward * Time.deltaTime * 9f);

        if ((sight.position - points[count].position).magnitude < 1)
            count++;

        if (count == points.Length)
            count = 0;


    }
}
