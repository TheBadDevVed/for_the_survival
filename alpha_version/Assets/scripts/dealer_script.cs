using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class dealer_script : MonoBehaviour
{
    public TextMeshProUGUI status;
    static_data script;

    // Start is called before the first frame update
    void Start()
    {
        script = GameObject.FindGameObjectWithTag("data").GetComponent<static_data>();
    }

    // Update is called once per frame
    void Update()
    {
        status.text = "money: " + script.money.ToString() + "\n\nammo: " + script.ammo.ToString() + "\n\nmedicine: " + script.medicine.ToString() + "\n\nmeth: " + script.meth.ToString();
    }

    public void buy_meth()
    {
        if (script.money >= 200)
        {
            script.money -= 200;
            script.meth += 1;
        }
    }

    public void buy_ammo()
    {
        if (script.money >= 100)
        {
            script.money -= 100;
            script.ammo += 10;
        }
    }

    public void buy_medicine()
    {
        if (script.money >= 50)
        {
            script.money -= 50;
            script.medicine += 1;
        }
    }
}
