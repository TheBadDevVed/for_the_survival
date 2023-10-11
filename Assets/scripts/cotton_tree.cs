using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class cotton_tree : MonoBehaviour
{
    public GameObject[] points;

    float high_limit = 300f,low_limit = 600f;
    float[] growth_time, count;
    bool[] has_cotton, growing;

    void Start()
    {
        growth_time = new float[3];
        has_cotton = new bool[3];
        count = new float[3];
        growing = new bool[3];

        for (int i = 0; i < 3; i++)
        {
            growth_time[i] = Random.Range(high_limit, low_limit);
            has_cotton[i] = false;
            count[i] = 0;
        }

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 3; i++)
        {
            if (count[i] <= growth_time[i] +1 && !has_cotton[i])
            {
                count[i] += Time.deltaTime;
                growing[i] = true;
            }

            if (count[i] >= growth_time[i] + 1 && !has_cotton[i])
            {
                count[i] = 0;
            }
        }
        check_growth();
    }

    void check_growth()
    {
        for (int i = 0; i < 3; i++)
        {
            if (growth_time[i] <= count[i] && !has_cotton[i])
            {
                points[i].SetActive(true);
                has_cotton[i] = true;
                growing[i] = false;
            }

            if (!growing[i] && points[i].activeSelf ==false)
            { 
                Debug.Log("collected");
                has_cotton[i] = false;
                growing[i]=true;
                count[i] = 0;
            }

            
        }
    }
}
