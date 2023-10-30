using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coal_generator : MonoBehaviour
{
    public GameObject[] coal;
    float count;
    bool has_coal;
    int coal_index;

    void Start()
    {
        coal_index = Random.Range(0, coal.Length - 1);
        coal[coal_index].SetActive(true);
        has_coal = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (has_coal == false)
        {
            if (count < 1200)
                count += Time.deltaTime;
            if (count >= 1200)
            {
                coal_index = Random.Range(0, coal.Length - 1);
                has_coal = true;
                coal[coal_index].SetActive(true);
                count = 0;
            }
        }

        if (coal[coal_index].activeSelf == false)
        {
            has_coal = false;
        }
    }
}
