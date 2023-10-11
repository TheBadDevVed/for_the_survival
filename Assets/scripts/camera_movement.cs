using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_movement : MonoBehaviour
{
    public GameObject refrence;
    public GameObject gun;
    public GameObject hand;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        gameObject.transform.position = refrence.transform.position + (refrence.transform.forward/3);
        transform.rotation = refrence.transform.rotation;

        gun.transform.position = transform.position + (refrence.transform.right/4) - (refrence.transform.up/3) + (refrence.transform.forward/3);
        gun.transform.rotation = transform.rotation;

        hand.transform.position = transform.position + (transform.forward * 0.4f);
        
    }
}
