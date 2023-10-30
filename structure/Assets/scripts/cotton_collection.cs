using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cotton_collection : MonoBehaviour
{
    static_data script;
    private void Start()
    {
        script = GameObject.FindGameObjectWithTag("data").GetComponent<static_data>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "cotton" && Input.GetKey(KeyCode.Mouse0))
        {
            other.gameObject.transform.parent.gameObject.SetActive(false);
            script.cotton++;
        }
    }

}
