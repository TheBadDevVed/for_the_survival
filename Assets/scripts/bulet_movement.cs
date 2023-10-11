using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulet_movement : MonoBehaviour
{
    public CharacterController bullet;
    float time = 0;

    // Update is called once per frame
    void Update()
    {
        bullet.Move(transform.forward * 20 * Time.deltaTime);

        if (time > 10)
        {
            Destroy(gameObject);
            time = 0;
        }
        else
            time += Time.deltaTime;
    }

    private void OnControllerColliderHit(ControllerColliderHit victim)
    {
        Destroy(gameObject);

        if(victim.gameObject.tag == "kill")
        Destroy(victim.gameObject);
    }
}
