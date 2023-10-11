using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class static_data : MonoBehaviour
{
    public int ammo,cotton;
    public GameObject sun;
    GameObject emptycotton;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sun.transform.Rotate(0.2f * Time.deltaTime, 0, 0);
    }
}
